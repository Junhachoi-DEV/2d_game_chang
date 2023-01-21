using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombbug : Enermy
{
    Animator animator;
    bool isFind = false;
    int Hp = 40;
    int weapon_damage;
    GameObject effect;
    //public float r;

    // Start is called before the first frame update
    private void Awake()
    {
        animator = GetComponent<Animator>();
        op = Random.Range(0, 3);
        home = transform.position;//��ü�� ��ġ
        Physics2D.IgnoreLayerCollision(3, 11);//�÷��̾���� �浹 ����
        GameObject weapon = Instantiate(prefab_weapon);
        weapon_damage = weapon.GetComponent<granade>().granade_dmg;
        effect = weapon.GetComponent<granade>().granade_effect;
    }

    // Update is called once per frame
    private void Update()
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
    }

    private void FixedUpdate()
    {
        if (isEnd == false && isDie == false)
        {

            RaycastHit2D raycast = Physics2D.Raycast(transform.position, transform.right * isLeft, distance, isLayer);//�÷��̾�͸� �浹�Ҽ� �ִ�
            Debug.DrawRay(transform.position, Vector2.right * isLeft * distance, new Color(0, 1, 0)); //�෹�̼� ���־� ��� ray�� �� �����.
            if (raycast.collider != null)//�÷��̾�� �浹�ÿ� 
            {
                //  Debug.Log("isfollow");
                if (isFind == false)
                {
                    isFollow = true;
                    animator.SetBool("Find", isFollow);//true
                    StartCoroutine(Find());

                }
                else if (isFind == true)
                {

                    if (Vector2.Distance(transform.position, raycast.collider.transform.position) <= 1f)
                    {
                        if (isDamage == false)
                            Attack();//�����ġ��
                    }
                    else
                    {
                        if (isDamage == false)
                            transform.position = Vector3.MoveTowards(transform.position, raycast.collider.transform.position, Time.deltaTime * speed * 3f);
                    }
                }
            }
            else
            {
                isFollow = false;
                isFind = false;
                animator.SetBool("Find", isFind);//false
                animator.ResetTrigger("Run");//�ٽ� ����
                box.SetActive(false);
            }
        }
        else
        {
            isFollow = false;
            isFind = false;
            animator.SetBool("Find", isFollow);
            animator.ResetTrigger("Run");
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
    IEnumerator Die()
    {
        animator.SetTrigger("Die");
        isDie = true;
        //isDamage = true;//�� �������̰�
        yield return new WaitForSeconds(3.5f);
        Destroy(gameObject);
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
    void attacked()
    {
        animator.SetBool("Attacked",false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="weapon"&&isDie==false)
        {
            if (collision.tag == "weapon")
                StartCoroutine(Attacked_weapon(collision.gameObject));
            Debug.Log("weapondamage");
            TakeDamage(weapon_damage,Hp);
            isDamage = true;
            Debug.Log("isnot move");
            animator.SetBool("Attacked", true);
            Debug.Log(isDamage);
            Invoke("attacked", 0.4f);
            Invoke("damage",0.4f);
          
        }
        else if(collision.tag=="effect"&&iseffect==false && isDie == false)
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

        if (collision.tag == "Endpoint"&&isEnd==false && isDie == false)
        {
           // Debug.Log("collision");
            isEnd = true;
            animator.SetBool("Walk", false);//���� �΋Hġ���� �ȿ����̱⿡ walk����� ������ �ʴ´�.
            if (isLeft == -1)
            {           
                isLeft = 1;              
            }
            else
            {               
                isLeft = -1;
            }
            transform.position = Vector2.MoveTowards(transform.position, home, Time.deltaTime * speed * 1.4f);
            StartCoroutine(Endpoint());
        }
    }
    public Vector2 boxSize;
    public Transform boxpos;
    public Transform direct;
    public GameObject box;
    public void Attack()
    {
        box.SetActive(true);
        Debug.Log("yes");
        if (isLeft == -1)
        {
            //direct.localScale = new Vector3(direct.localScale.x, direct.localScale.y, direct.localScale.z);
            if (boxpos.localPosition.x > 0)//�θ���� �Ÿ��� ����϶� ������ ���� ����
            {
                boxpos.localPosition = new Vector2(boxpos.localPosition.x*-1, boxpos.localPosition.y);//������ �����
                //axepos.localPosition = new Vector2(axepos.localPosition.x * -1, axepos.localPosition.y);
            }
            

        }
        else
        {
           // direct.localScale = new Vector3(direct.localScale.x*-1, direct.localScale.y, direct.localScale.z);
            
            if (boxpos.localPosition.x < 0)//�θ���� �Ÿ��� �����϶� ����� ���� ������
            {
                boxpos.localPosition = new Vector2(Mathf.Abs(boxpos.localPosition.x), boxpos.localPosition.y);//���밪���� ����� �����.
                //axepos.localPosition = new Vector2(Mathf.Abs(axepos.localPosition.x), axepos.localPosition.y);
            }

        }
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(boxpos.position, boxSize, 0);
        //�ڽ��� ��ġ�� �ڽ��� ũ�⿡ �׸��� ȸ������ �ִ´�
        foreach (Collider2D colider in collider2Ds)
        {
           // Debug.Log("�浹");
            if (colider.tag == "Player")//�ݶ��̴��� �ױ׸� ���ؼ� �÷��̾���� �־���´�
            {
                Debug.Log("player damage");
                colider.GetComponent<Rigidbody2D>().AddForce(new Vector2(200f*isLeft,10f));
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(boxpos.position, new Vector2(1f, 1f));
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

    private IEnumerator Find()
    {
        yield return new WaitForSeconds(1f);
        animator.SetTrigger("Run");
        isFind = true;
    }
}