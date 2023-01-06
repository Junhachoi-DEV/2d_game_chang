using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hooking : MonoBehaviour
{
    grapping grap;
    public DistanceJoint2D joint2D;
    SpriteRenderer sprite;
    void Start()
    {
        //find �Լ��� �÷��̾�(�̸�)�ȿ��ִ� ������Ʈ�� �����Ѵ�.
        grap = GameObject.Find("player").GetComponent<grapping>();
        //���� �ִ� ����Ʈ�� Ȱ��ȭ
        joint2D = GetComponent<DistanceJoint2D>();
        sprite = GetComponent<SpriteRenderer>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ring")) //tag�� ���ϋ�
        {
            joint2D.enabled = true; // Ȱ��ȭ
            grap.is_attach = true;
            sprite.color = new Color(0.25f, 0.57f, 0.48f, 1);
            grap.hook_ef.SetActive(false);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ring")) //tag�� ���ϋ�
        {
            sprite.color = new Color(1, 1, 1, 0);
        }
    }
}
