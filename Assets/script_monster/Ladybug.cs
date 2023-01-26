using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladybug :Enermy
{
    // Start is called before the first frame update
    int weapon_damage;
    GameObject effect;
    Animator animator;
    public Transform ray;
    int Hp = 5;
    Transform target;
    [Header("�����Ÿ�")]
    [SerializeField] [Range(0f, 3f)] float contactDistance = 1f;
    [Header("�νĺҰ��Ÿ�")]
    [SerializeField] [Range(0f, 6f)] float dontcatch = 5f;
    private void Awake()
    {
        op = Random.Range(0, 3);
        animator = GetComponent<Animator>();
        home = transform.position;//��ü�� ��ġ
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        GameObject weapon = Instantiate(prefab_weapon);
        weapon_damage = weapon.GetComponent<granade>().granade_dmg;
        effect = weapon.GetComponent<granade>().granade_effect;
    }
 
    // Update is called once per frame
    void Update()
    {
        if (isFollow == false && isEnd == false && isDamage == false && isDie == false)//�Ѿư��� ���� �������� �̷� ��������Ѵ�.
        {
            if (op == 0)
            {
                transform.Translate(Vector2.left * speed * Time.deltaTime);
                isLeft = -1;
                animator.SetFloat("Isleft", isLeft);
                animator.SetBool("Walk", true);
            }
            else if (op == 1)
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);
                isLeft = 1;
                animator.SetFloat("Isleft", isLeft);
                animator.SetBool("Walk", true);
            }
            else
            {
                transform.Translate(Vector2.zero);
                animator.SetFloat("Isleft", isLeft);//���� �������� �Ӹ��� ����
                animator.SetBool("Walk", false);
            }

            if (isDelay == false)
            {
                isDelay = true;
                StartCoroutine(Move());
            }
        }
      //  Debug.Log(isLeft);
    }

    
    private void FixedUpdate()
    {
        FollowTarget();
        backhome();
    }
    void backhome()
    {
        if(Vector2.Distance(home,transform.position)>10f)
        {
            isEnd = true;
            isFollow = false;
        }

        if(Vector2.Distance(home, transform.position) >= 0.5f && isEnd ==true&&isFollow==false)
        {
            StartCoroutine(goHome());
        }
        else if(isFollow==true)
        {
            isEnd=false;
        }
        else if(Vector2.Distance(home, transform.position) <= 0.5f && isEnd == true)
        {
            Debug.Log("Move");
            isEnd = false;
        }
    }
    void FollowTarget()
    {
        if (isEnd == false&&isFollow==true)
        {
            DirectionEnemy(target.position.x, transform.position.x);
            if (Vector2.Distance(transform.position, target.position) > contactDistance && isFollow == true)
            {
                
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                Debug.Log("follow");
                //if () ;//�÷��̾���ʿ� �ִ��� �����ʿ� �ִ����� �˾ƾ� �Ѵ�.
            }
            else if (Vector2.Distance(transform.position, target.position) < contactDistance && isFollow == true&&isDie==false)
            {
                Attack();
            }

            if(isEnd == true)
            {
                speed += 1.5f;              
            }
            Debug.Log(isLeft);
            box.SetActive(false);
        }
    }


    public void DirectionEnemy(float target, float basobj)
    {
        if (target < basobj)
        {
            animator.SetFloat("Isleft", -1);//������ ����
            isLeft = -1;
        }
        else
        {
            animator.SetFloat("Isleft", 1);
            isLeft = 1;
        }
    }

    IEnumerator Die()
    {
        animator.SetTrigger("Die");
        isDie = true;
        //isDamage = true;//�� �������̰�
        yield return new WaitForSeconds(3.5f);
        //�����ϵ����ϱ�

        Explosion();
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
    public void Explosion()
    {
        expolsion.SetActive(true);
        //if(Collision2D )
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(boxpos.position, 5f);
        //�ڽ��� ��ġ�� �ڽ��� ũ�⿡ �׸��� ȸ������ �ִ´�
        foreach (Collider2D colider in collider2Ds)
        {
            // Debug.Log("�浹");
            if (colider.tag == "Player")//�ݶ��̴��� �ױ׸� ���ؼ� �÷��̾���� �־���´�
            {
                Debug.Log("explosion damage");
                colider.GetComponent<Rigidbody2D>().AddForce(new Vector2(400f * isLeft, 500f));
            }
        }
    }

    public void TakeDamage(int damage, int h)
    {
        
        Hp = h - damage;
        Debug.Log(Hp);
        if (Hp <= 0)
        {
            StartCoroutine(Die());
        }
       // return h;
    }

    void attacked()
    {
        animator.SetBool("Attacked", false);
    }

    IEnumerator Attacked_weapon(GameObject collision)
    {
        iseffect = true;
        Transform d = collision.transform;
        Destroy(collision);
        GameObject eff = Instantiate(effect, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.3f);
        Destroy(eff);
        yield return new WaitForSeconds(0.1f);
        iseffect = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "weapon" && isDie == false)
        {
            if (collision.tag == "weapon")
                StartCoroutine(Attacked_weapon(collision.gameObject));
            Debug.Log("weapondamage");
            TakeDamage(weapon_damage, Hp);
            isDamage = true;
            Debug.Log("isnot move");
            animator.SetBool("Attacked", true);
            Debug.Log(isDamage);
            Invoke("attacked", 0.4f);
            Invoke("damage", 0.4f);

        }
        else if (collision.tag == "effect" && iseffect == false && isDie == false)
        {
            Debug.Log("effectdamage");
            TakeDamage(weapon_damage, Hp);
            isDamage = true;
            Debug.Log("isnot move");
            animator.SetBool("Attacked", true);
            Debug.Log(isDamage);
            Invoke("attacked", 0.4f);
            Invoke("damage", 0.4f);
            Debug.Log(isDamage);

        }

        if (collision.CompareTag("Player"))
        {
            isFollow = true;
        }
    }

    public Vector2 boxSize;
    public Transform boxpos;
    public GameObject expolsion;
    public GameObject box;

    public void Attack()
    {
        box.SetActive(true);
        
       
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(boxpos.position, boxSize, 0);
        //�ڽ��� ��ġ�� �ڽ��� ũ�⿡ �׸��� ȸ������ �ִ´�
        foreach (Collider2D colider in collider2Ds)
        {
            // Debug.Log("�浹");
            if (colider.tag == "Player")//�ݶ��̴��� �ױ׸� ���ؼ� �÷��̾���� �־���´�
            {
                Debug.Log("player damage");
                colider.GetComponent<Rigidbody2D>().AddForce(new Vector2(200f * isLeft, 10f));
            }
        }
    }

    private IEnumerator Move()
    {
        yield return new WaitForSeconds(0.8f);
        op = Random.Range(0, 3);
        isDelay = false;
    }

    private IEnumerator goHome()
    {
        yield return new WaitForSeconds(1.5f);
        transform.position = Vector2.MoveTowards(transform.position, home, speed * 5f * Time.deltaTime);
        /*yield return new WaitForSeconds(0.8f);
        isFollow = false;
        Debug.Log("no");
        yield return new WaitForSeconds(0.8f);
        isEnd = false;*/
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(boxpos.position, new Vector2(1f, 1f));
    }

}


