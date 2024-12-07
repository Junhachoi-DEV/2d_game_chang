using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popcornbug : Enermy
{
    Animator animator;
    bool isFind = false;
    public int Hp = 3;
    int weapon_damage;
    //GameObject effect;
    player p;
    //public float r;
    SpriteRenderer sprite;
    obj_manager obj_m;
    // Start is called before the first frame update
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        p = FindObjectOfType<player>();
        player_position = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        op = Random.Range(0, 3);
        home = transform.position;//물체의 위치
        Physics2D.IgnoreLayerCollision(3, 11);//플레이어와의 충돌 무시
        Physics2D.IgnoreLayerCollision(11,12);//몬스터와의 충돌무시
        Physics2D.IgnoreLayerCollision(11, 11);
        //GameObject weapon = Instantiate(prefab_weapon);
        weapon_damage = p.player_grenade_dmg;
        obj_m = FindObjectOfType<obj_manager>();
        Hp = 3;
        //effect = weapon.GetComponent<granade>().granade_effect;
    }

    // Update is called once per frame
    private void Update()
    {


        if (isFollow == false && isEnd == false && isDamage == false && isDie == false&&walktime==2f)//쫓아가고 있지 않을때만 이런 동작허용한다.
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
                animator.SetFloat("Isleft", isLeft);//전에 방향으로 머리를 향함
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
    float walkspeed = 2f;//애니메이션 속도 조절
    private void FixedUpdate()
    {
        
        if (isEnd == false && isDie == false&&walktime==2f)
        {
            RaycastHit2D raycast = Physics2D.Raycast(transform.position, transform.right * isLeft, distance, isLayer);//플레이어와만 충돌할수 있다
            Debug.DrawRay(transform.position, Vector2.right * isLeft * distance, new Color(0, 1, 0)); //듀레이션 없애야 계속 ray가 안 생긴다.
            if (raycast.collider != null)//플레이어와 충돌시에 
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
            animator.SetFloat("Isleft", -1);//방향의 설정
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

        //Debug.Log(Hp);
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
        //isDamage = true;//적 못움직이게
        yield return new WaitForSeconds(3.5f);
        gameObject.SetActive(false);
    }

    IEnumerator Attacked_weapon(GameObject collision)
    {
        iseffect = true;
        Transform d = collision.transform;
        //Destroy(collision);
        //GameObject eff = Instantiate(effect, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.3f);
        //Destroy(eff);
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
            
            TakeDamage(weapon_damage, Hp);
            game_manager.Instance.gm_ef_sound_mng("grenade_sound");
            isDamage = true;
            
            animator.SetBool("Attacked", true);
            
            sprite.color = new Color(1, 0, 0, 1);
            Invoke("attacked", 0.4f);
            Invoke("damage", 0.4f);

        }
        else if (collision.tag == "effect" && iseffect == false && isDie == false)
        {
            
            TakeDamage(weapon_damage, Hp);
            isDamage = true;
            
            animator.SetBool("Attacked", true);
            
            sprite.color = new Color(1, 0, 0, 1);
            Invoke("attacked", 0.4f);
            Invoke("damage", 0.4f);
            


        }

        if (collision.tag == "Endpoint" && isEnd == false && isDie == false)
        {
            // Debug.Log("collision");
            isEnd = true;
            animator.SetBool("Walk", false);//벽에 부딫치면은 안움직이기에 walk모션을 취하지 않는다.
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
            if (boxpos.localPosition.x > 0)//부모와의 거리가 양수일때 음수가 정상 왼쪽
            {
                boxpos.localPosition = new Vector2(boxpos.localPosition.x * -1, boxpos.localPosition.y);//음수로 만든다
                //axepos.localPosition = new Vector2(axepos.localPosition.x * -1, axepos.localPosition.y);
            }
        }
        else
        {
            // direct.localScale = new Vector3(direct.localScale.x*-1, direct.localScale.y, direct.localScale.z);

            if (boxpos.localPosition.x < 0)//부모와의 거리가 음수일때 양수가 정상 오른쪽
            {
                boxpos.localPosition = new Vector2(Mathf.Abs(boxpos.localPosition.x), boxpos.localPosition.y);//절대값으로 양수로 만든다.
                //axepos.localPosition = new Vector2(Mathf.Abs(axepos.localPosition.x), axepos.localPosition.y);
            }

        }
        // Debug.Log("yes");

        bullet = obj_m.make_obj("popcornbug_bullet");
        bullet.transform.position = boxpos.transform.position;
        //bullet= Instantiate(popcorn_bullet, boxpos.position, transform.rotation);
       
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
