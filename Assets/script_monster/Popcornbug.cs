using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popcornbug : Enermy
{
    Animator animator;
    bool isFind = false;
    public int Hp = 30;
    int weapon_damage;
    GameObject effect;
    player p;
    //public float r;
    SpriteRenderer sprite;
    // Start is called before the first frame update
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        p = FindObjectOfType<player>();
        player_position = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        op = Random.Range(0, 3);
        home = transform.position;//��ü�� ��ġ
        Physics2D.IgnoreLayerCollision(3, 11);//�÷��̾���� �浹 ����
        Physics2D.IgnoreLayerCollision(11,12);//���Ϳ��� �浹����
        Physics2D.IgnoreLayerCollision(11, 11);
        GameObject weapon = Instantiate(prefab_weapon);
        weapon_damage = weapon.GetComponent<granade>().granade_dmg;
        effect = weapon.GetComponent<granade>().granade_effect;
    }

    // Update is called once per frame
    private void Update()
    {


        if (isFollow == false && isEnd == false && isDamage == false && isDie == false&&walktime==2f)//�Ѿư��� ���� �������� �̷� ��������Ѵ�.
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
    bool iscollider = false;
    float walktime = 2f;
    Vector3 collider_position;
    float walkspeed = 2f;//�ִϸ��̼� �ӵ� ����
    private void FixedUpdate()
    {
        
        if (isEnd == false && isDie == false&&walktime==2f)
        {
            RaycastHit2D raycast = Physics2D.Raycast(transform.position, transform.right * isLeft, distance, isLayer);//�÷��̾�͸� �浹�Ҽ� �ִ�
            Debug.DrawRay(transform.position, Vector2.right * isLeft * distance, new Color(0, 1, 0)); //�෹�̼� ���־� ��� ray�� �� �����.
            if (raycast.collider != null)//�÷��̾�� �浹�ÿ� 
            {
                collider_position = raycast.collider.transform.position;
                iscollider = true;
                walktime -= Time.deltaTime;
            }
        }
        if(iscollider==true&&isDie==false)
        {
            walktime -= Time.deltaTime;
            DirectionEnemy(transform.position.x, collider_position.x);
            animator.SetBool("Run",true);
            animator.SetFloat("Walkspeed", walkspeed);
            transform.position = Vector3.MoveTowards(transform.position, collider_position, Time.deltaTime * speed*-1*4f); ;
            if(bullet_cooltime==0.8f)
            StartCoroutine(Attack());
           // Attack();
           /*else
            {
                box.SetActive(false);
                isAttack = false;
                animator.SetBool("Attack", isAttack);
            }*/
           bullet_cooltime-=Time.deltaTime;
            
            if (bullet_cooltime < 0)
                bullet_cooltime = 0.8f;
        }
        
        if(walktime<=0&&iscollider==true && isDie == false)
        {
            isLeft *= -1;
            iscollider=false;
            animator.SetBool("Run", false);
            StartCoroutine(walk());
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
       // Debug.Log(isLeft);
    }

    IEnumerator walk()
    {
        yield return new WaitForSeconds(2f);
        walktime = 2f;
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
        sprite.color = new Color(1, 1, 1, 1);
        animator.SetBool("Attacked", false);
    }

  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "weapon" && isDie == false)
        {
            if (collision.tag == "weapon")
                StartCoroutine(Attacked_weapon(collision.gameObject));
            Debug.Log("weapondamage");
            TakeDamage(weapon_damage, Hp);
            game_manager.Instance.gm_ef_sound_mng("grenade_sound");
            isDamage = true;
            Debug.Log("isnot move");
            animator.SetBool("Attacked", true);
            Debug.Log(isDamage);
            sprite.color = new Color(1, 0, 0, 1);
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
            sprite.color = new Color(1, 0, 0, 1);
            Invoke("attacked", 0.4f);
            Invoke("damage", 0.4f);
            Debug.Log(isDamage);


        }

        if (collision.tag == "Endpoint" && isEnd == false && isDie == false)
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
        if (collision.gameObject.tag == "p_melee" && !isDie)
        {
            TakeDamage(p.player_dmg, Hp);
            isDamage = true;
            sprite.color = new Color(1, 0, 0, 1);
            animator.SetBool("Attacked", true);
            Invoke("attacked", 0.4f);
            Invoke("damage", 0.4f);
        }
    }
    public Vector2 boxSize;
    public Transform boxpos;
    public Transform direct;
    public GameObject box;
    public GameObject popcorn_bullet;
    float bullet_followtime = 0.2f;
    float bullet_cooltime = 0.8f;
    IEnumerator Attack()
    {
        
        isAttack = true;
        animator.SetBool("Attack", isAttack);
        GameObject bullet;
        box.SetActive(true);
       // Debug.Log(isLeft);
        if (isLeft == 1)
        {
            //direct.localScale = new Vector3(direct.localScale.x, direct.localScale.y, direct.localScale.z);
            if (boxpos.localPosition.x > 0)//�θ���� �Ÿ��� ����϶� ������ ���� ����
            {
                boxpos.localPosition = new Vector2(boxpos.localPosition.x * -1, boxpos.localPosition.y);//������ �����
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
       // Debug.Log("yes");
        
        
        bullet= Instantiate(popcorn_bullet, boxpos.position, transform.rotation);
       
        //bullet.transform.Translate(player_position);  //Vector2.MoveTowards(bullet.transform.position, player_position.position, Time.deltaTime * speed);
        //bullet.transform.position= Vector2.MoveTowards(bullet.transform.position, player_position.position, Time.deltaTime * speed);
        yield return new WaitForSeconds(2f);
        box.SetActive(false);
        isAttack = false;
        animator.SetBool("Attack", isAttack);
       
       // yield return new WaitForSeconds(1f);
       // Destroy(bullet);
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
