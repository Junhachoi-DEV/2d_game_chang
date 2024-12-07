using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bombbug : Enermy
{
    Animator animator;
    bool isFind = false;
    bool isexplosion = false;
    public int Hp = 20;
    int weapon_damage;
    //GameObject effect;
    public GameObject explosion;
    //public float r;
    Transform explosion_target;
    player p;
    SpriteRenderer sprite;
    // Start is called before the first frame update
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        p = FindObjectOfType<player>();
        explosion_target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        op = Random.Range(0, 3);
        home = transform.position;//물체의 위치
        Physics2D.IgnoreLayerCollision(3, 11);//플레이어와의 충돌 무시
        //GameObject weapon = Instantiate(prefab_weapon);
        weapon_damage = p.player_grenade_dmg;
        Hp = 20;
        //effect = weapon.GetComponent<granade>().granade_effect;
    }

    // Update is called once per frame
    private void Update()
    {


        if (isFollow == false && isEnd == false && isDamage == false && isDie == false)//쫓아가고 있지 않을때만 이런 동작허용한다.
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
    }
    private void FixedUpdate()
    {
        if(isDie==true&&isexplosion==false)
        {
            DirectionEnemy(explosion_target.position.x, transform.position.x);
            transform.position=Vector3.MoveTowards(transform.position, explosion_target.position, Time.deltaTime*speed*2f);
        }

        if (isEnd == false && isDie == false)
        {

            RaycastHit2D raycast = Physics2D.Raycast(transform.position, transform.right * isLeft, distance, isLayer);//플레이어와만 충돌할수 있다
            Debug.DrawRay(transform.position, Vector2.right * isLeft * distance, new Color(0, 1, 0)); //듀레이션 없애야 계속 ray가 안 생긴다.
            if (raycast.collider != null)//플레이어와 충돌시에 
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
                            Attack();//몸통박치기
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
                animator.ResetTrigger("Run");//다시 리셋
                box.SetActive(false);
            }
        }
        else if(isDie==false)
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

        //Debug.Log(Hp);
        if (Hp <= 0)
        {

            StartCoroutine(Die());

        }
        // return h;
    }
    IEnumerator Die()
    {

        box.SetActive(false);
        isDie = true;
        animator.SetTrigger("Explosion");
        
        //isDamage = true;//적 못움직이게
        yield return new WaitForSeconds(3.5f);
        //폭발하도록하기
        isexplosion = true;
        Explosion();
        animator.SetTrigger("Die");
        //폭발알리기용
        yield return new WaitForSeconds(0.2f);
        explosion.SetActive(false);
        yield return new WaitForSeconds(4f);
        gameObject.SetActive(false);
    }


    public void Explosion()
    {
        explosion.SetActive(true);
        //if(Collision2D )
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(boxpos.position, 5f);
        //박스의 위치와 박스의 크기에 그리고 회전값을 넣는다
        foreach (Collider2D colider in collider2Ds)
        {
            // Debug.Log("충돌");
            if (colider.tag == "Player")//콜라이더의 테그를 비교해서 플레이어면은 넣어놓는다
            {
                
                damage_manager.Instance.damage_count(2); // 폭탄 벌레의 폭발
                //colider.GetComponent<Rigidbody2D>().AddForce(new Vector2(400f * isLeft, 500f));
            }
        }
    }
    /*animator.SetTrigger("Die");
        isDie = true;
        //isDamage = true;//적 못움직이게
        yield return new WaitForSeconds(3.5f);
    //폭발하도록하기

    Explosion();
    yield return new WaitForSeconds(0.1f);
    Destroy(gameObject);
    */
    IEnumerator Attacked_weapon(GameObject collision)
    {
        iseffect = true;
        Transform d = collision.transform;
        //Destroy(collision);
        //GameObject eff = Instantiate(effect, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.3f);
        
        yield return new WaitForSeconds(0.1f);
        iseffect = false;
    }
    void attacked()
    {
        sprite.color = new Color(1, 1, 1, 1);
        animator.SetBool("Attacked",false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="weapon"&&isDie==false)
        {
            if (collision.tag == "weapon")
                StartCoroutine(Attacked_weapon(collision.gameObject));
            
            TakeDamage(weapon_damage,Hp);
            game_manager.Instance.gm_ef_sound_mng("grenade_sound");
            isDamage = true;
            
            sprite.color = new Color(1, 1, 0, 1);
            animator.SetBool("Attacked", true);
            
            Invoke("attacked", 0.4f);
            Invoke("damage",0.4f);
          
        }
        else if(collision.tag=="effect"&&iseffect==false && isDie == false)
        {
            
            TakeDamage(weapon_damage, Hp);
            isDamage = true;
            
            animator.SetBool("Attacked", true);
            
            sprite.color = new Color(1, 1, 0, 1);
            Invoke("attacked", 0.4f);
            Invoke("damage", 0.4f);
            //Debug.Log(isDamage);
           
        }

        if (collision.tag == "Endpoint"&&isEnd==false && isDie == false)
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
            sprite.color = new Color(1, 1, 0, 1);
            animator.SetBool("Attacked", true);
            Invoke("attacked", 0.4f);
            Invoke("damage", 0.4f);
        }
    }
    public Vector2 boxSize;
    public Transform boxpos;
    public Transform direct;
    public GameObject box;
    public float p_left;
    public void Attack()
    {
        p_left = isLeft;
        box.SetActive(true);
        //Debug.Log("yes");
        if (isLeft == -1)
        {
            //direct.localScale = new Vector3(direct.localScale.x, direct.localScale.y, direct.localScale.z);
            if (boxpos.localPosition.x > 0)//부모와의 거리가 양수일때 음수가 정상 왼쪽
            {
                boxpos.localPosition = new Vector2(boxpos.localPosition.x*-1, boxpos.localPosition.y);//음수로 만든다
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
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(boxpos.position, boxSize, 0);
        //박스의 위치와 박스의 크기에 그리고 회전값을 넣는다
        foreach (Collider2D colider in collider2Ds)
        {
           // Debug.Log("충돌");
            if (colider.tag == "Player")//콜라이더의 테그를 비교해서 플레이어면은 넣어놓는다
            {
                //Debug.Log("player damage");
                damage_manager.Instance.damage_count(1); // 폭탄 벌레의 공격력 =1
               // colider.GetComponent<Rigidbody2D>().AddForce(new Vector2(200f*isLeft,10f));
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
