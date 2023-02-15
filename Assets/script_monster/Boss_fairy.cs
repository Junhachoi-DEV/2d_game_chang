using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_fairy : MonoBehaviour 
{
    public GameObject center;//�� ��ü�� �������� �¿�� �����δ�
    public float distance;
    public LayerMask isLayer;
    bool isDie = false;
    bool isEnd = false;
    bool iscool = false;
    bool isFollow = false;
    bool isDamage = false;
    bool isAttack = false;
    bool isground=false;
    bool isDown = false;
    int Hp;
    float isLeft;
    public float speed;
    public GameObject prefab_weapon;
    protected bool iseffect = false;
    Rigidbody2D rb;
    Transform target;
    Transform Player;
    float player_follow_limit;
    int op;
    public float min_P;//�����ϼ� �ִ� ������ǥ
    public float max_P;//�����ϼ� �ִ� �ִ���ǥ
    float limit_time= 2f;
    float sign = 1;
    // Start is called before the first frame update
    private void Awake()
    {
        Player= GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        target = center.transform;
        op = Random.Range(0, 3);
        min_P = target.position.x - 5f;
        max_P = target.position.x + 5f;
    }

    // Update is called once per frame
    void Update()
    {
        op = 0;
        // Debug.Log(op); 
        if (op == 0 && ((this.transform.position.x <= min_P + 1f || this.transform.position.x >= max_P - 1f)||isground==true)&&iscool==false)
        {
            
            isAttack = true;
            //rb.velocity = Vector3.zero;
            StartCoroutine(op0());
            // StartCoroutine(cooltime());
            if (isground == true && isEnd == true && isDown == true)
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, new Vector2(temp_x, target.position.y+2.5f), speed*3 * Time.deltaTime);
                
                StartCoroutine(cooltime());
            }
        }
        else if (op == 1)
        {
           // StartCoroutine(cooltime());
        }
        else if (op == 2)
        {
          //  StartCoroutine(cooltime());
        }
    }
    float currentTime;
    private void FixedUpdate()
    {
        if(isAttack==false)//���� ���ݽÿ� �ȿ�����
        FollowTarget();
    }

    void FollowTarget()
    {
        currentLerpTime+= Time.deltaTime;
        {
            StartCoroutine(Move());
        }
    }
    float currentLerpTime = 0f;
    public float lerpTime = 5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "bottom"&&isAttack==true)
        {
           // rb.gravityScale = 0;
            rb.velocity = new Vector3(0, 0, 0);
           //�������� �̵�
            //rb.AddForce(new Vector3(1,0,0));
            isground = true;
        }
        if(collision.tag == "Endpoint")
        {
            isEnd = true;
        }
       /* Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(boxpos.position, boxSize, 0);
        //�ڽ��� ��ġ�� �ڽ��� ũ�⿡ �׸��� ȸ������ �ִ´�
        foreach (Collider2D colider in collider2Ds)
        {
            // Debug.Log("�浹");
            if (colider.tag == "Player")//�ݶ��̴��� �ױ׸� ���ؼ� �÷��̾���� �־���´�
            {
                Debug.Log("player damage");
                damage_manager.Instance.damage_count(1 / 10);
                //StartCoroutine(attack());
            }
        }
       */
    }
    float temp_x;
    IEnumerator op0()
    {
         yield return new WaitForSeconds(0.1f);
        if (isground == false)
            rb.AddForce(new Vector2(-1.41f * speed, -1.41f * speed));
        
        if(isground == true&&isEnd==false)
        {
            rb.velocity = new Vector3(-10, 0, 0);
        }

        if (isground == true && isEnd==true&&isDown==false)
        {
            isDown = true;
            this.transform.position = new Vector2(Player.position.x, Player.position.y + 12f);
           temp_x =Player.position.x;
            rb.velocity = Vector2.zero;
            // isground = false;
            // yield return new WaitForSeconds(0.5f);
        }
       
    }
    IEnumerator Move()
    {
        if (currentLerpTime >= 2*lerpTime)//�������� ������� 
        {
            currentLerpTime = 0;   
        }
        float t = currentLerpTime / lerpTime;
        t = Mathf.Sin(t * Mathf.PI * 0.5f);
        
        //if(this.transform.position.x==min_P)
        this.transform.position = Vector3.Lerp(new Vector2(min_P,target.position.y+8f),new Vector2(max_P, target.position.y + 8f), t);//�¿�� �ݺ����� �����̰� �Ѵ�
        //Debug.Log("t");
        //Debug.Log(t);
        //Debug.Log("CT");
        //Debug.Log(currentLerpTime);
        //yield return new WaitForSeconds(0.1f);
        yield return null;
    }
    
    IEnumerator cooltime()
    {
        yield return new WaitForSeconds(5f);
        iscool = true;
        isground = false;
        isEnd = false;
        isDown = false;
        isAttack = false;
        yield return new WaitForSeconds(3f);
        iscool=false; 
        op= Random.Range(0, 3);
    }
}
