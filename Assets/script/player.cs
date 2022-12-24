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

    // ����,���� �̵� ��
    float x;
    float y;


    //����
    bool is_trun; //�� ,�� ��ȯ ����
    bool is_ground; //�� ����
    bool ray_wall; //�� ����
    public bool is_hook_range_max; // ���� ���� �ִ� ����
    //public bool is_hook_range_min; // ���� ���� �ּ� ����

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
    }

    // �̵��� ȿ���� ���� ���⿡ �ִ´�.
    void FixedUpdate()
    {
        player_move();
    }

    void player_move()
    {
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
        else if (is_hook_range_max)
        {
            if(y != -1)
            {
                transform.Translate(new Vector3(0, y * apply_speed * Time.deltaTime, 0));
                is_hook_range_max = false;
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
}
