using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hooking : MonoBehaviour
{
    grapping grap;
    public DistanceJoint2D joint2D;
    void Start()
    {
        //find �Լ��� �÷��̾�(�̸�)�ȿ��ִ� ������Ʈ�� �����Ѵ�.
        grap = GameObject.Find("player").GetComponent<grapping>();
        //���� �ִ� ����Ʈ�� Ȱ��ȭ
        joint2D = GetComponent<DistanceJoint2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ring")) //tag�� ���ϋ�
        {
            joint2D.enabled = true; // Ȱ��ȭ
            grap.is_attach = true;
            grap.hook_ef.SetActive(false);
        }
        
    }
}
