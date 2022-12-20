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

    float x; // ���� �̵�

    Rigidbody2D rigid; 
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>(); // ����
        apply_speed = run_speed; // ���� ���ǵ�� run�ӵ�
    }

    void Update()
    {
        
    }
    // �̵��� ȿ���� ���� ���⿡ �ִ´�.
    void FixedUpdate()
    {
        player_move();
    }
    void player_move()
    {
        x = Input.GetAxisRaw("Horizontal");

        if (Input.GetButton("Horizontal"))
        {
            if(x != 0)
            {
                transform.localScale = new Vector3(x, 1, 1); //ĳ���� ������
            }
            transform.Translate(new Vector3(x * apply_speed * Time.deltaTime, 0, 0));
        }
    }
}
