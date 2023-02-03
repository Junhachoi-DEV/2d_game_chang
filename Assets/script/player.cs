using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    //�÷��̾� ����
    public int player_hp;
    //�ӵ�
    public float apply_speed; // ���� ���ǵ�
    public float crouch_speed = 0f;
    public float run_speed = 6f;
    public float walk_speed= 3f;
    public float dash_speed= 8f;

    //���� ����
    public float ray_dis;
    public float ray_wall_dis;

    //���Ƚ�� ��Ʈ
    int stop_cnt; //������ ���߰��ϴ°� �ѹ��� ����

    //Ÿ��
    float dash_timer;
    [Range(0.1f, 3)] public float dash_time;

    //����
    public float jump_force = 10f; //���� �� ��
    public float hook_jump_force = 8f; //��ũ���� �� ��
    public float wall_jump_force = 5f; //������ �� ��

    // ����,���� �̵� ��
    float x;
    float y;

    //��Ÿ ������Ʈ
    public GameObject granade;
    public GameObject hiar;
    public GameObject p_melee;

    //�Ѿ� �� ��ź ��, ��Ÿ ��
    public float g_force;

    //����
    bool is_trun; //�� ,�� ��ȯ ����
    bool is_ground; //�� ����
    bool is_air; //���� ����
    bool is_wall_jump_ready; //�� ���� �غ� ����
    bool is_dash; //�뽬 ����
    bool ray_wall; //�� ����
    bool is_hitted; //�ǰ� ����
    bool is_attacking; //���� ��
    bool do_atk; //���� �� 2
    public bool is_hook_range_max; // ���� ���� �ִ� ����

    // ������Ʈ
    Rigidbody2D rigid;
    grapping grap;
    Animator anime;
    SpriteRenderer sprite;
    void Start()
    {
        anime = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>(); // ����
        sprite = GetComponent<SpriteRenderer>();
        apply_speed = run_speed; // ���� ���ǵ�� run�ӵ�
        grap = GetComponent<grapping>();
        is_trun = true;
    }

    void Update()
    {
        check_wall_and_bottom();
        player_jump();
        player_wall_jump();
        player_use_granade();
        player_dash();
        player_attack();
    }

    // �̵��� ȿ���� ���� ���⿡ �ִ´�.
    void FixedUpdate()
    {
        if (is_hitted)
        {
            return;
        }
        player_move();
    }

    void player_move()
    {
        if (is_wall_jump_ready || is_attacking)
        {
            return; //���� �����̸� �Լ��� ����.
        }
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        if (Input.GetButton("Horizontal"))
        {
            anime.SetBool("is_run", true);
            hiar.transform.localPosition = new Vector3(0.1f, 0.15f, 0);
            if (x != 0)
            {
                transform.localScale = new Vector3(x, 1, 1); //ĳ���� ������
            }
            if (grap.is_attach) // ������ ������
            {
                rigid.AddForce(new Vector2(x * apply_speed * Time.deltaTime, 0),ForceMode2D.Impulse); //��,�� �̵�
            }
            else
            {
                transform.Translate(new Vector3(x * apply_speed * Time.deltaTime, 0, 0)); // �⺻ �̵�
            }
        }
        else
        {
            anime.SetBool("is_run", false);
        }
        if (Input.GetButton("Vertical") && !is_hook_range_max)
        {
            if (grap.is_attach) // ������ ������
            {
                transform.Translate(new Vector3(0, y * apply_speed * Time.deltaTime, 0)); //��,�Ʒ� �̵�
            }
        }
    }

    //�� && �ٴ� üũ.
    void check_wall_and_bottom()
    {
        if (x != 0)
        {
            if (x == 1)
            {
                is_trun = true;
            }
            else
            {
                is_trun = false;
            }
        }
        //��üũ
        Debug.DrawRay(rigid.position, Vector2.down * ray_dis, new Color(0, 1, 0));
        is_ground = Physics2D.Raycast(rigid.position, Vector2.down * ray_dis, ray_dis, LayerMask.GetMask("bottom"));
        
        //��üũ
        Debug.DrawRay(rigid.position, Vector2.right * (is_trun ? ray_wall_dis : ray_wall_dis * -1), new Color(0, 1, 0));
        ray_wall = Physics2D.Raycast(rigid.position, Vector2.right * (is_trun ? ray_wall_dis : ray_wall_dis * -1), ray_wall_dis, LayerMask.GetMask("bottom"));
    }

    void player_jump()
    {
        if (is_wall_jump_ready||is_attacking)
        {
            anime.SetBool("do_jump", false);
            return; //���� �����̸� �Լ��� ����.
        }
        if (!is_ground)
        {
            anime.SetBool("do_jump", true);
        }
        else
        {
            anime.SetBool("do_jump", false);
        }
        if (Input.GetButtonDown("Jump") && is_ground)
        {
            is_air = false;
            Invoke("jump_ani_deley", 0.5f);
            rigid.velocity = new Vector2(rigid.velocity.x, jump_force);
            anime.SetBool("do_jump", true);
        }
        else if (Input.GetButtonDown("Jump") && grap.is_attach) // ������ ������
        {
            is_air = false;
            Invoke("jump_ani_deley", 0.5f);
            rigid.velocity = Vector2.zero;
            rigid.AddForce(new Vector2(rigid.velocity.x, hook_jump_force),ForceMode2D.Impulse);
            anime.SetBool("do_jump", true);
        }
        if(is_air && is_ground)
        {
            anime.SetBool("do_jump", false);
        }
    }
    void jump_ani_deley()
    {
        is_air = true;
    }
    void player_wall_jump()
    {
        if (is_attacking)
        {
            return;
        }
        if (is_hitted)
        {
            anime.SetBool("is_wall", false);
            is_wall_jump_ready = false;
            rigid.gravityScale = 2.5f;
            stop_cnt = 0;
  
            return;
        }
        if(ray_wall && !is_ground) //���� �پ��� ���� ������.
        {
            anime.SetBool("is_wall", true);
            #region ���Ŵ޸��� ���
            //���� ������ ���������� ���� ���Ŵ޸��� ���

            if (!is_trun && Input.GetKey(KeyCode.D))
            {
                is_wall_jump_ready = false;
                anime.SetBool("is_wall", false);
                anime.SetBool("do_jump", true);
                stop_cnt = 0;
                return;
            }
            //������ ������ �������� ���� ���Ŵ޸��� ���
            if (is_trun && Input.GetKey(KeyCode.A))
            {
                is_wall_jump_ready = false;
                anime.SetBool("is_wall", false);
                anime.SetBool("do_jump", true);
                stop_cnt = 0;
                return;
            }
            #endregion
            
            is_wall_jump_ready = true; //������ �غ� �Ϸ�.
            
            if (stop_cnt <= 0)
            {
                
                rigid.velocity = Vector2.zero; // ����.
                stop_cnt++;
            }
            
            rigid.gravityScale = 0;

            
            if (Input.GetButtonDown("Jump")) //������ ������ ��������.
            {
                // is_trun�� Ʈ��� �������� �޽��� ���������� ƨ�� (�� ���ʺ����� ������ ������ ���������� ƨ��)
                rigid.velocity = new Vector2(wall_jump_force * (is_trun ? -1 : 1), wall_jump_force * 1.5f);
                anime.SetBool("is_wall", false);
                anime.SetBool("do_jump", true);
                Invoke("wall_jump_deley", 0.15f); // ƨ��� ������
            }
        }
        else if (ray_wall && is_ground)
        {
            anime.SetBool("is_wall", false);
            rigid.gravityScale = 2.5f;
            is_wall_jump_ready = false;
            stop_cnt = 0;
        }
        else
        {
            anime.SetBool("is_wall", false);
            rigid.gravityScale = 2.5f;
        }
    }
    void wall_jump_deley()
    {
        is_wall_jump_ready = false;
        stop_cnt = 0;
        rigid.velocity = new Vector2(0, rigid.velocity.y);
    }

    void player_use_granade()
    {
        if (is_attacking)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // ����� �߷°��� 2�̴�.
            GameObject ins_granade = Instantiate(granade, transform.position, transform.rotation); // ĳ���� ��ġ���� ����, ���߿� ������Ʈ Ǯ�� ���ٰ���.
            Rigidbody2D rigid_granade = ins_granade.GetComponent<Rigidbody2D>(); // ���� ����
            
            //ĳ������ġ���� (��* ��* ����) + (������ * ��* ĳ���� �ٶ󺸴� ����) = �밢������ �������� �׸���.
            rigid_granade.velocity = (transform.up * g_force *0.7f ) + (transform.right * g_force * (is_trun ? 1 : -1));
           
        }
    }
    
    void player_dash()
    {
        if (is_attacking)
        {
            return;
        }
        if(x != 0) //������ ���� ��
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                is_dash = true;

                if (is_dash)
                {
                    dash_timer += Time.deltaTime;
                    if (dash_timer >= dash_time)
                    {
                        apply_speed = run_speed;
                    }
                    else
                    {
                        apply_speed = dash_speed;
                    }
                }
            }
            
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            dash_timer = 0;
            is_dash = false;
            apply_speed = run_speed;
        }
    }

    void player_attack()
    {
        if (do_atk || is_hitted || is_wall_jump_ready)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            anime.SetTrigger("is_atk");
        }
        
    }
    #region ���� ���� //����Ƽ �ִϸ��̼����� �����Ѵ�.
    //���� �� �ٸ� ���� ����
    public void atk_true()
    {
        is_attacking = true;
    }
    public void atk_false()
    {
        is_attacking = false;
    }
    //���� �� ���ݸ�� ����
    public void doing_atk_true()
    {
        do_atk = true;
    }
    public void doing_atk_false()
    {
        do_atk = false;
    }
    //���� ���� �ڽ� on/off
    public void atk_collider_on()
    {
        p_melee.SetActive(true);
    }
    public void atk_collider_off()
    {
        p_melee.SetActive(false);
    }
    #endregion  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "monster_melee")
        {
            is_hitted = true;
            anime.SetTrigger("is_hitted");
            gameObject.layer = 14; // ����
            sprite.color = new Color(1, 1, 1, 0.5f); //��������
            player_hp -= damage_manager.Instance.en_dmg;
            hitted_anime_stop();
            Invoke("hitted_deley", 0.3f);
            Invoke("hitted_back", 2f);
        }
    }
    void hitted_back()
    {
        gameObject.layer = 3; // ���� �ٽ� ���ƿ�
        sprite.color = new Color(1, 1, 1, 1);
    }
    void hitted_deley()
    {
        is_hitted = false;
    }
    void hitted_anime_stop()
    {
        is_hitted = true;
        is_attacking = false;
        is_wall_jump_ready = false;
        is_dash = false;
    }
}
