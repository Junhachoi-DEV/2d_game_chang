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

    //����
    public float jump_force = 10f; //���� �� ��
    public float hook_jump_force = 8f; //��ũ���� �� ��
    public float wall_jump_force = 5f; //������ �� ��

    // ����,���� �̵� ��
    float x;
    float y;

    //�Ѿ� �� ��ź
    public GameObject granade;

    //�Ѿ� �� ��ź �� ��
    public float g_force;

    //����
    bool is_trun; //�� ,�� ��ȯ ����
    bool is_ground; //�� ����
    bool is_wall_jump_ready; //�� ���� �غ� ����
    bool ray_wall; //�� ����
    public bool is_hook_range_max; // ���� ���� �ִ� ����

    // ������Ʈ
    Rigidbody2D rigid;
    grapping grap;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>(); // ����
        apply_speed = run_speed; // ���� ���ǵ�� run�ӵ�
        grap = GetComponent<grapping>();
    }

    void Update()
    {
        check_wall_and_bottom();
        player_jump();
        player_wall_jump();
        player_use_granade();
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
            if(x != 0)
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
        Debug.DrawRay(rigid.position, Vector2.down * 1f, new Color(0, 1, 0));
        is_ground = Physics2D.Raycast(rigid.position, Vector2.down * 1f, 1f, LayerMask.GetMask("bottom"));
        
        //��üũ
        float cont_num = 0.5f;
        Debug.DrawRay(rigid.position, Vector2.right * (is_trun ? cont_num : cont_num * -1), new Color(0, 1, 0));
        ray_wall = Physics2D.Raycast(rigid.position, Vector2.right * (is_trun ? cont_num : cont_num * -1), cont_num, LayerMask.GetMask("bottom"));
    }

    void player_jump()
    {
        if (is_wall_jump_ready)
        {
            return; //���� �����̸� �Լ��� ����.
        }

        if (Input.GetButtonDown("Jump") && is_ground)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, jump_force);
        }
        else if (Input.GetButtonDown("Jump") && grap.is_attach) // ������ ������
        {
            rigid.velocity = Vector2.zero;
            rigid.velocity = new Vector2(rigid.velocity.x, hook_jump_force);
        }
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
                return;
            }
            //������ ������ �������� ���� ���Ŵ޸��� ���
            if (is_trun && x == -1)
            {
                is_wall_jump_ready = false;
                return;
            }
            #endregion
            is_wall_jump_ready = true; //������ �غ� �Ϸ�.
            rigid.velocity = Vector2.zero; // ����.
            rigid.gravityScale = 0;

            
            if (Input.GetButtonDown("Jump")) //������ ������ ��������.
            {
                // is_trun�� Ʈ��� �������� �޽��� ���������� ƨ�� (�� ���ʺ����� ������ ������ ���������� ƨ��)
                rigid.velocity = new Vector2(wall_jump_force * (is_trun ? -1 : 1), wall_jump_force * 1.5f);
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
}
