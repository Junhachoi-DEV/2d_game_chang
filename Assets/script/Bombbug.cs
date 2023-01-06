using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombbug : Enermy
{
    
    // Start is called before the first frame update
    private void Awake()
    {
            op = Random.Range(0, 3);
        home = transform.position;//��ü�� ��ġ
    }

    // Update is called once per frame
    private void Update()
    {
        if (isFollow == false && isEnd == false)//�Ѿư��� ���� �������� �̷� ��������Ѵ�.
        {
            if (op == 0)
            {
                transform.Translate(Vector2.left * speed * Time.deltaTime);
                isLeft = -1;
            }
            else if (op == 1)
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);
                isLeft = 1;
            }
            else
            {
                transform.Translate(Vector2.zero);
                isLeft=0;
            }

            if (isDelay == false)
            {
                isDelay = true;
                StartCoroutine(Move());
            }
        }
    }
    private void FixedUpdate()
    {
        if (isEnd == false)
        {
            
            RaycastHit2D raycast = Physics2D.Raycast(transform.position, transform.right * isLeft, distance, isLayer);
            Debug.DrawRay(transform.position, Vector2.right * isLeft * distance, new Color(0, 1, 0)); //�෹�̼� ���־� ��� ray�� �� �����.
            if (raycast.collider != null)
            {
              //  Debug.Log("isfollow");

                transform.position = Vector3.MoveTowards(transform.position, raycast.collider.transform.position, Time.deltaTime * speed * 1.3f);
                isFollow = true;
            }
            else
            {
                isFollow = false;
            }
        }
        else isFollow = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Endpoint"&&isEnd==false)
        {
           // Debug.Log("collision");
            isEnd = true;
            if (isLeft == -1)
            {
               // transform.eulerAngles = new Vector3(0, 180, 0);
                isLeft = 1;
            }
            else
            {
                //transform.eulerAngles = new Vector3(0, 0, 0);
                isLeft = -1;
            }
            transform.position = Vector2.MoveTowards(transform.position, home, Time.deltaTime * speed * 1.4f);
            StartCoroutine(Endpoint());
        }
    }
   
    private IEnumerator Endpoint()
    {
        yield return new WaitForSeconds(0.5f);
        transform.position = Vector2.MoveTowards(transform.position, home, Time.deltaTime * speed * 3f);
        yield return new WaitForSeconds(0.5f);
        isEnd = false;
    }
    private IEnumerator Move()
    {
        yield return new WaitForSeconds(0.8f);
        op = Random.Range(0, 3);
        isDelay = false;
    }
}
