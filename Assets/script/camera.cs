using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    public Transform target; //�÷��̾�
    public float speed; //ī�޶� �ӵ�

    Transform cam_limit; // ī�޶� ȭ�� ���� ����

    public Transform[] limits;

    float height;
    float width;

    void Start()
    {
        // ȭ���� ������� �������� �Ѵ�.
        height = Camera.main.orthographicSize; //ī�޶� ���� ���� ���(�ַ� 2d���� �����)
        width = height * Screen.width / Screen.height; //(�� ���α��̿��� ���� ��ŭ ������ �� ���� ��)
        change_limit(0); //
    }

    public void change_limit(int x)
    {
        cam_limit = limits[x]; // ķ���� ��ũ�� ����
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * speed);

        //����
        float lx = cam_limit.localScale.x * 0.5f - width; //ķ���� ��ũ���� ���ݿ� ���� ����(����) ���� ����. 
        float clamp_x = Mathf.Clamp(transform.position.x, -lx + cam_limit.position.x, lx + cam_limit.position.x); // -lx �� �ּҰ� lx �� �ִ밪
        //����
        float ly = cam_limit.localScale.y * 0.5f - height;//ķ���� ��ũ���� ���ݿ� ���� ����(����) ���� ����.
        float clamp_y = Mathf.Clamp(transform.position.y, -ly + cam_limit.position.y, ly + cam_limit.position.y);// -ly �� �ּҰ� ly �� �ִ밪

        transform.position = new Vector3(clamp_x, clamp_y, -10f); //z���� -10�� �⺻ ī�޶� ��ġ ���̴�.
    }
}
