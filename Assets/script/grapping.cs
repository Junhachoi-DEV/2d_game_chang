using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grapping : MonoBehaviour
{
    public LineRenderer line;
    public Transform hook;

    public float hook_speed;
    public float hook_distence;

    bool is_hook_key_down;
    bool is_line_max;
    public bool is_attach;

    Vector2 mouse_direction;
    Rigidbody2D rigid;
    player p;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        p = GetComponent<player>();
        //���� �׸���
        line.positionCount = 2; //�׷��� ���� ����Ʈ ����
        line.endWidth = line.startWidth = 0.05f; // �׷��� ���� ����
        line.SetPosition(0, transform.position); //index ����Ʈ, ��ġ
        line.SetPosition(1, hook.position);
        line.useWorldSpace = true; // ������ǥ�� �Ѵٴ� ��
        is_attach = false;
    }


    void Update()
    {
        //��ġ�� ��� ������Ʈ ����� ������ ������ ���δ�.
        line.SetPosition(0, transform.position);
        line.SetPosition(1, hook.position);

        if (Input.GetMouseButtonDown(1) && !is_hook_key_down) // ���콺 �������� ������ ��Ű�� �ȴ�������
        {
            hook.position = transform.position; // ������ ó����ġ
            // ȭ��(��ũ��)���� ��ǥ�� ���콺 ��ġ�� �ɸ��� ��ġ�� ���� = ���콺�� ����
            mouse_direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            is_hook_key_down = true;   //Ű�� ������ =true
            is_line_max = false;       //���� �Ÿ��� ª���ϱ�
            hook.gameObject.SetActive(true); // Ȱ��ȭ
        }

        if (is_hook_key_down && !is_line_max && !is_attach)// ������ false �̰� �Ⱥپ�����
        {
            //translate�� �̿��ؼ�  (���ư��� ����.����ȭ ��Ű�� * �ð� * ���ǵ�(��))
            hook.Translate(mouse_direction.normalized * Time.deltaTime * hook_speed);

            if (Vector2.Distance(transform.position, hook.position) > hook_distence)// ��ũ�� �Ÿ��� hook_distence���� Ŭ��
            {
                is_line_max = true;
            }
        }
        else if (is_hook_key_down && is_line_max && !is_attach)// ������ true �Ⱥپ�����
        {
            //movetowards�� Ÿ������ ���� �Լ��̴�.
            hook.position = Vector2.MoveTowards(hook.position, transform.position, Time.deltaTime * hook_speed);
            if (Vector2.Distance(transform.position, hook.position) < 0.1f)//�÷��̾���� �Ÿ��� 0.1���� �۴ٸ�
            {
                is_hook_key_down = false;
                is_line_max = false;
                hook.gameObject.SetActive(false); // Ȱ��ȭ

            }
        }
        else if (is_attach) //������
        {
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space)) // �������¿��� �ٽ� ���콺 �������� ������ �Ǵ� �������¿��� �ٽ� ����Ű�� ������
            {
                is_attach = false;
                is_hook_key_down = false;
                is_line_max = false;
                hook.GetComponent<hooking>().joint2D.enabled = false;
                hook.gameObject.SetActive(false);
                //rigid.velocity = new Vector2(p.x, 0);
            }
        }
    }
}