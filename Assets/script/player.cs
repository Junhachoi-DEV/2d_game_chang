using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    //�ӵ�
    public float apply_speed; // ���� ���ǵ�
    public float crouch_speed = 0f;
    public float run_speed = 6f;
    public float walk_speed= 3f;
    public float dash_speed= 8f;

    //���� ����
    public float ray_dis;
    public float ray_wall_dis;


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

    //�Ѿ� �� ��ź ��, ��Ÿ ��
    public float g_force;

    //����
    bool is_trun; //�� ,�� ��ȯ ����
    bool is_ground; //�� ����
    bool is_air; //���� ����
    bool is_wall_jump_ready; //�� ���� �غ� ����
    bool is_dash; //�뽬 ����
    bool ray_wall; //�� ����
    public bool is_hook_range_max; // ���� ���� �ִ� ����

    // ������Ʈ
    Rigidbody2D rigid;
    grapping grap;
    Animator anime;
    void Start()
    {
        anime = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>(); // ����
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
    }

    // �̵��� ȿ���� ���� ���⿡ �ִ´�.
    void FixedUpdate()
    {
        player_move();
    }

    void player_move()
    {
        if (is_wall_jump_ready)
        {
            return; //���� �����̸� �Լ��� ����.
        }
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        if (Input.GetButton("Horizontal"))
        {
            anime.SetBool("is_run", true);
            hiar.transform.localPosition = new Vector3(0.05f, 0.15f, 0);
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
        ray_wall = Physics2D.Raycast(rigid.position, Vector2.right * (is_trun ? ray_wall_dis : ray_wall_dis * -1), ray_wall_dis, LayerMask.GetMask("wall"));
    }

    void player_jump()
    {
        if (is_wall_jump_ready)
        {
            anime.SetBool("do_jump", false);
            return; //���� �����̸� �Լ��� ����.
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
            rigid.velocity = new Vector2(rigid.velocity.x, hook_jump_force);
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
        if(ray_wall && !is_ground) //���� �پ��� ���� ������.
        {
            #region ���Ŵ޸��� ���
            //���� ������ ���������� ���� ���Ŵ޸��� ���
            
            if (!is_trun && Input.GetKey(KeyCode.D))
            {
                is_wall_jump_ready = false;
                anime.SetBool("is_wall", false);
                anime.SetBool("do_jump", true);
                return;
            }
            //������ ������ �������� ���� ���Ŵ޸��� ���
            if (is_trun && Input.GetKey(KeyCode.A))
            {
                is_wall_jump_ready = false;
                anime.SetBool("is_wall", false);
                anime.SetBool("do_jump", true);
                return;
            }
            #endregion
            
            anime.SetBool("is_wall", true);

            is_wall_jump_ready = true; //������ �غ� �Ϸ�.
            rigid.velocity = Vector2.zero; // ����.
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
        else
        {
            rigid.gravityScale = 2.5f;
        }
    }
    void wall_jump_deley()
    {
        is_wall_jump_ready = false;
        rigid.velocity = new Vector2(0, rigid.velocity.y);
    }

    void player_use_granade()
    {
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
}
