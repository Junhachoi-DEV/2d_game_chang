using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grapping : MonoBehaviour
{
    public LineRenderer line;
    public Transform hook;
    public GameObject hook_ef;

    //����
    public float hook_speed;
    public float hook_distence;
    [Range(0f, 10f)] public float hook_ef_cooltime;

    bool is_hook_key_down;
    bool is_line_max;
    public bool is_attach;

    Vector2 mouse_direction;
    player p;
    void Start()
    {
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
        //point_play();
        //��ġ�� ��� ������Ʈ ����� ������ ������ ���δ�.
        line.SetPosition(0, transform.position);
        line.SetPosition(1, hook.position);
        hook_ef.transform.position = transform.position;// ����Ʈ�� ��ġ�� �÷��̾� ���߾�


        if (Input.GetMouseButtonDown(1) && !is_hook_key_down) // ���콺 �������� ������ ��Ű�� �ȴ�������
        {
            line.enabled = false;
            hook.position = transform.position; // ������ ó����ġ

            // ȭ��(��ũ��)���� ��ǥ�� ���콺 ��ġ�� �ɸ��� ��ġ�� ���� = ���콺�� ����
            mouse_direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            is_hook_key_down = true;   //Ű�� ������ =true
            is_line_max = false;       //���� �Ÿ��� ª���ϱ�
            hook.gameObject.SetActive(true); // Ȱ��ȭ

            //����Ʈ ����
            //������ atan2(y �� , x ��) * Mathf.Rad2Deg = ���� ���� ���� �ٲ��ִ� �Լ�
            //ź������ ���Լ��̴�.
            float r = Mathf.Atan2(mouse_direction.y, mouse_direction.x) * Mathf.Rad2Deg;
            hook_ef.transform.rotation = Quaternion.Euler(0, 0, r+(-90)); //����Ʈ�� �ٶ󺸴� ����
            hook_ef.SetActive(true);
            Invoke("hook_ef_disapear", hook_ef_cooltime /10);
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
                hook.gameObject.SetActive(false); // ��Ȱ��ȭ

            }
        }
        else if (is_attach) //������
        {
            line.enabled = true;
            //p.anime.SetBool("is_hook", true);
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Space)) // �������¿��� �ٽ� ���콺 �������� ������ �Ǵ� �������¿��� �ٽ� ����Ű�� ������
            {
                is_attach = false;
                is_hook_key_down = false;
                is_line_max = false;
                hook.GetComponent<hooking>().joint2D.enabled = false;
                hook.gameObject.SetActive(false);
            }
            if (Vector2.Distance(transform.position, hook.position) > hook_distence) // ���� ���¿��� ���Ʒ��� �����϶� ���� ���� �� ����
            {
                p.is_hook_range_max = true;
                transform.position = Vector2.MoveTowards(transform.position, hook.position, Time.deltaTime * 2); // ������ ������ ���� �ǵ����� true�� false�� �ǵ�����.
            }
            else
            {
                p.is_hook_range_max = false;
            }
        }
    }
    void hook_ef_disapear()
    {
        hook_ef.SetActive(false);
    }
    
}