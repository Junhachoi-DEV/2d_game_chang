using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_fairy : MonoBehaviour 
{
    int weapon_damage;

    int sound_cnt; //배경음 한번만 실행

    //GameObject effect;
    player p;
    SpriteRenderer sprite;
    obj_manager obj_m;
    float uptime = 0.5f;
    float attack_cool = 4f;
    float readytime = 5f;
    float groundtime = 3.5f;
    //GameObject bullet;
    public GameObject center;//이 물체를 기준으로 좌우로 움직인다(처음에만
    public float distance;
    public LayerMask isLayer;
    bool isDamage = false;
    bool isDie = false;
    bool isEnd = false;
    bool iscool = false;
    bool isNotmove = false;
    bool isAttack = false;
    bool isground=false;
    bool isDown = false;
    bool isback= false;
    bool isone = false;
    bool isupdown = false;
    bool isAttacking = false;
    public bool is_bettle_start;//보스전 시작
    public GameObject potal1;
    public GameObject potal2;


    public int Hp;
    float isLeft;
    public float speed;
    public GameObject prefab_weapon;
    public GameObject bullet_box;
    protected bool iseffect = false;
    Rigidbody2D rb;
    Transform target;
    Transform Player;
    float bullet_respown = 0.2f;
    int op,s_op;




    public float min_P;//움직일수 있는 최하좌표
    public float max_P;//움직일수 있는 최대좌표
    float fly_limit_time= 3.5f;

    Animator anime;

    private void Awake()
    {
        p = FindObjectOfType<player>();
        sprite = GetComponent<SpriteRenderer>();
        obj_m = FindObjectOfType<obj_manager>();
        Player= GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
        op = Random.Range(0, 3);
        s_op=Random.Range(0, 2);
        target = center.transform;
        min_P = target.position.x - 8f;
        max_P = target.position.x + 8f;
        //GameObject weapon = Instantiate(prefab_weapon);
        weapon_damage = p.player_grenade_dmg;
        //effect = weapon.GetComponent<granade>().granade_effect;
        // op = 2;

    }

    // Update is called once per frame
    void Update()
    {
        
        if (!is_bettle_start)
        {
            return;
        }
        else if (sound_cnt <= 0)
        {
            game_manager.Instance.gm_bg_sound_mng("boss_bgm");
            potal1.SetActive(false);
            potal2.SetActive(false);
            anime.SetTrigger("do_battle");
            sound_cnt++;
        }
        
       /* Debug.Log("op");
        Debug.Log(op);
        Debug.Log("iscool");
        Debug.Log(iscool);
        Debug.Log("back");
        Debug.Log(isback);
        Debug.Log("attack");
        Debug.Log(isAttack);
        Debug.Log("notmove");
        Debug.Log(isNotmove);
        Debug.Log("isdie");
        Debug.Log(isDie);*/
        //Debug.Log(s_op);
        //op = 2;
        // s_op = 1;
        // Debug.Log(op); 
        if (isDie == false)
        {
            if (iscool == false)
            {
                if (op == 0)
                {
                    //Debug.Log("yes");
                    //rb.velocity = Vector3.zero;
                    // if(isupdown==false)
                    // {
                    //     updown();
                    //}
                    if (iscool == false)
                    {
                        StartCoroutine(op0());
                    }
                    // StartCoroutine(cooltime());
                    if (isEnd == true && isDown == true && iscool == false)
                    {
                        if (isNotmove == false)
                            this.transform.Translate(Vector2.down * 6 * Time.deltaTime);
                        else if (isone == false)
                            StartCoroutine(cooltime());
                    }
                }
                else if (op == 1)
                {
                    if (fly_limit_time > 0)
                    {
                        //탄환발사
                        if (s_op == 0)//왼쪽
                        {
                            if (this.transform.position.x <= min_P + 1f)
                            {
                                isAttack = true;
                            }

                            if (isAttack == true)
                            {
                                if (bullet_respown > 0)
                                    bullet_respown -= Time.deltaTime;
                                if (bullet_respown <= 0)
                                {
                                    GameObject bullet = obj_m.make_obj("boss_bullet"); //생성
                                    bullet.transform.position = bullet_box.transform.position;
                                    //Instantiate(bullet, bullet_box.transform.position, bullet_box.transform.rotation);
                                    bullet_respown = 0.2f;
                                }
                                fly_limit_time -= Time.deltaTime;

                                // rb.AddForce(Vector2.right*60*Time.deltaTime);
                                transform.Translate(Vector2.right * 12 * Time.deltaTime);
                            }
                        }
                        else if (s_op == 1)//오른쪽
                        {
                            if (this.transform.position.x >= max_P - 1f)
                            {
                                isAttack = true;

                            }

                            if (isAttack == true)
                            {
                                if (bullet_respown > 0)
                                    bullet_respown -= Time.deltaTime;
                                if (bullet_respown <= 0)
                                {
                                    GameObject bullet = obj_m.make_obj("boss_bullet"); //생성
                                    bullet.transform.position = bullet_box.transform.position;
                                    bullet_respown = 0.2f;
                                }
                                fly_limit_time -= Time.deltaTime;

                                //rb.AddForce(Vector2.left * 60 * Time.deltaTime);
                                transform.Translate(Vector2.left * 12 * Time.deltaTime);
                            }
                        }
                        // StartCoroutine(cooltime());
                    }
                    else if (fly_limit_time <= 0)
                    {
                        if (isDown == false)
                        {
                            isDown = true;
                            this.transform.position = new Vector2(Player.position.x, Player.position.y + 12f);
                            temp_player = Player;//플레이어 위치
                            rb.velocity = Vector2.zero;
                            //  Debug.Log("yese");
                        }
                        else if (isNotmove == false)
                        {
                            this.transform.Translate(Vector2.down * 6 * Time.deltaTime);
                            isEnd = true;
                        }
                        if (isNotmove == true && isone == false)
                        {
                            StartCoroutine(cooltime());
                        }
                    }
                }

                else if (op == 2)
                {
                    if (s_op == 0)
                    {
                        if (this.transform.position.x <= min_P + 1f)
                        {
                            isAttack = true;
                        }
                    }
                    else if (s_op == 1)
                    {
                        if (this.transform.position.x >= max_P - 1f)
                        {
                            isAttack = true;
                        }
                    }

                    //근처에가서 공격하기
                    //Invoke("cooltime_C", 1f);
                    if (isAttack == true && attack_cool > 0)
                    {
                        anime.SetBool("do_atk", true);
                        if (Vector2.Distance(this.transform.position, Player.position) > 1.5f&&isAttacking==false)
                            this.transform.position = Vector2.MoveTowards(this.transform.position, Player.position, Time.deltaTime * speed);
                        if (Vector2.Distance(this.transform.position, Player.position) <= 1.5f)
                        {
                            DirectionEnemy(Player.position.x, this.transform.position.x);
                            //isfast = true;
                            isAttacking = true;
                            sprite.color = Color.red;
                            
                            Invoke("Attack", 0.45f);

                        }

                        if (attack_cool > 0)
                        {
                            attack_cool -= Time.deltaTime;
                        }
                    }
                    if (attack_cool <= 0)
                    {
                        if (this.transform.position.x == min_P)
                        {
                            isback = true;
                            anime.SetBool("do_atk", false);
                            if (isone == false)
                                StartCoroutine(cooltime());
                        }
                        else if (isback == false)
                        {
                            //isback = true;
                            anime.SetBool("do_atk", false);
                            gofirst();
                        }
                    }
                }
            }
            else if (iscool == true)
            {
                if (readytime > 0)
                {
                    readytime -= Time.deltaTime;

                    if (isback == true)
                    {
                        gofirst();
                        if (this.transform.position.x == min_P)
                        {

                            // Debug.Log("minp");
                            //Debug.Log(min_P);
                            //Debug.Log("back");
                            isback = false;
                        }
                    }
                }
                if (readytime <= 0)
                {
                    //op = 0;
                    //s_op = 1;
                    anime.SetBool("do_atk", false);
                    isAttack = false;
                    isNotmove = false;
                    isback = false;
                    isupdown = false;
                    attack_cool = 4f;
                    readytime = 5f;
                }
            }
        }
    }
    
    public GameObject box;
    public Transform boxpos;
    public Vector2 boxSize;
    public void updown()
    {
        if (uptime > 0)
        {
            rb.AddForce(Vector2.up* 1.41f );
            //rb.AddForce(boxSize);
            uptime-=Time.deltaTime;
        }
        else 
        {   
            if(isupdown == false)
                rb.AddForce(Vector2.down*1.41f);
            if (this.transform.position.y <= (center.transform.position.y + 6f))
            {
                isupdown = true;
                rb.velocity = Vector2.zero;
                
            }
        }
    }
    public void Attack()
    {
        //p_left = isLeft;
        box.SetActive(true);
        anime.SetBool("do_atk", false);
        anime.SetTrigger("atk");
        //Debug.Log("yes");
        if (isLeft == -1)
        {
            //direct.localScale = new Vector3(direct.localScale.x, direct.localScale.y, direct.localScale.z);
            if (boxpos.localPosition.x > 0)//부모와의 거리가 양수일때 음수가 정상 왼쪽
            {
                sprite.flipX = true;
                boxpos.localPosition = new Vector2(boxpos.localPosition.x * -1, boxpos.localPosition.y);//음수로 만든다
                //axepos.localPosition = new Vector2(axepos.localPosition.x * -1, axepos.localPosition.y);
            }
        }
        else
        {
            // direct.localScale = new Vector3(direct.localScale.x*-1, direct.localScale.y, direct.localScale.z);

            if (boxpos.localPosition.x < 0)//부모와의 거리가 음수일때 양수가 정상 오른쪽
            {
                sprite.flipX = false;
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
                damage_manager.Instance.damage_count(2); 
            }
        }
        StartCoroutine(attack());
        //StartCoroutine(cooltime());
    }

    IEnumerator attack()
    {
        yield return new WaitForSeconds(0.6f);
        //isAttack = false;
        sprite.color = Color.white;
        box.SetActive(false);
        isAttacking = false;
        yield return null;
        //melee.SetActive(false);
        //animator.SetBool("Attack", false);
    }


    public void DirectionEnemy(float target, float basobj)
    {
        if (target < basobj)
        {
           // animator.SetFloat("Isleft", -1);//방향의 설정
            isLeft = -1;
        }
        else
        {
          //  animator.SetFloat("Isleft", 1);
            isLeft = 1;
        }
    }
    IEnumerator cooltime()
    {
        
        // yield return new WaitForSeconds(2.5f);
        isone = true;
        yield return new WaitForSeconds(0.5f);
        anime.SetBool("is_air", true);
        // isup = true;
        yield return new WaitForSeconds(5f);
        anime.SetBool("is_air", false);
        groundtime = 3.5f;
        fly_limit_time = 3.5f;
        attack_cool = 4f;
        isground = false;
        isEnd = false;
       
        isAttack = false;
        //yield return new WaitForSeconds(6f);
        isDown = false;
        op = Random.Range(0, 3);
        s_op = Random.Range(0, 1);
        // yield return new WaitForSeconds(readytime);
        iscool = true;
        isback = true;
        
    }
    
    private void FixedUpdate()
    {
        if (!is_bettle_start)
        {
            return;
        }
        if (isAttack == false&&isback==false&&isNotmove==false&&isDie==false&&isupdown==false)//위에 공격시에 안움직임
        { 
            FollowTarget();
            isone = false;
            iscool = false;
        }
        
    }
    void gofirst()
    {   
        t=0;
        currentLerpTime = 0;
        this.transform.position = Vector2.MoveTowards(this.transform.position, new Vector2(min_P,center.transform.position.y+6f), speed * Time.deltaTime);    
    }

    void FollowTarget()
    {
        currentLerpTime+= Time.deltaTime;
            StartCoroutine(Move());
        
    }
    float currentLerpTime = 0f;
    public float lerpTime = 5f;

    void attacked()
    {
        sprite.color = new Color(1, 1, 1, 1);
     //   animator.SetBool("Attacked", false);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "bottom"&&isAttack==true&&isEnd==false&&op!=2&&isDie==false)
        {
           // rb.gravityScale = 0;
            rb.velocity = new Vector3(0, 0, 0);
           //왼쪽으로 이동
            //rb.AddForce(new Vector3(1,0,0));
            isground = true;
        }

        if(collision.tag == "bottom" && isEnd == true&&isNotmove==false&&isDie==false)
        {
           // Debug.Log("yaps");
            isNotmove = true;
            target = Player.transform;
            min_P = target.position.x - 8f;
            max_P = target.position.x + 8f;
        }

        if (collision.tag == "weapon" && isDie == false)
        {
            if (collision.tag == "weapon")
                StartCoroutine(Attacked_weapon(collision.gameObject));
           // Debug.Log("weapondamage");
            TakeDamage(weapon_damage, Hp);
            isDamage = true;
            game_manager.Instance.gm_ef_sound_mng("grenade_sound");
           // Debug.Log("isnot move");
           // animator.SetBool("Attacked", true);
            //Debug.Log(isDamage);
            Invoke("attacked", 0.15f);
         //   Invoke("damage", 0.15f);
            sprite.color = new Color(1, 0, 0, 1);
        }
        else if (collision.tag == "effect" && iseffect == false && isDie == false)
        {
            //Debug.Log("effectdamage");
            TakeDamage(weapon_damage, Hp);
            isDamage = true;
          //  Debug.Log("isnot move");
           // animator.SetBool("Attacked", true);
            //Debug.Log(isDamage);
            Invoke("attacked", 0.15f);
          //  Invoke("damage", 0.15f);
            //Debug.Log(isDamage);
            sprite.color = new Color(1, 0, 0, 1);
        }

       
        if (collision.gameObject.tag == "p_melee" && !isDie)
        {
            TakeDamage(p.player_dmg, Hp);
            isDamage = true;
            
            //animator.SetBool("Attacked", true);
            Invoke("attacked", 0.15f);
            sprite.color = new Color(1, 0, 0, 1);
            //  Invoke("damage", 0.15f);
        }

    }
    IEnumerator Die()
    {
        //animator.SetTrigger("Die");
        isDie = true;

      //  Debug.Log("isroll");
        //Debug.Log(roll);
        //isDamage = true;//적 못움직이게
        yield return new WaitForSeconds(0.3f);
        rb.AddForce(new Vector3(0, 0, 0));
        anime.SetTrigger("is_die");
        yield return new WaitForSeconds(0.5f);
        //폭발하도록하기

        //Explosion();
        //yield return new WaitForSeconds(0.1f);
        potal1.SetActive(true);
        potal2.SetActive(true);
        game_manager.Instance.gm_bg_sound_mng("ingame_bgm");
        gameObject.SetActive(false);
    }
    public void TakeDamage(int damage, int h)
    {

        Hp = h - damage;
        //Debug.Log(Hp);
        if (Hp <= 0)
        {
            isDie = true;
            StartCoroutine(Die());
        }
        // return h;

    }
    Transform temp_player;
    IEnumerator op0()
    {
        if(iscool==true)
            yield return null;
         yield return new WaitForSeconds(0.5f);
        if (isground == false)
        {
            if (s_op == 0)
            {
                if (this.transform.position.x <= min_P + 1f)
                {
                    if (isupdown == false)
                        {
                        updown();
                        }
                        isAttack = true;
                    //Debug.Log("yes");
                    if (isupdown == true)
                    {
                        box.SetActive(true);
                        rb.AddForce(new Vector2(1.41f * speed, -1.41f * speed));
                    }
                }
            }

            if (s_op == 1)
            {
                if (this.transform.position.x >= max_P - 1f)
                {
                    if (isupdown == false)
                    {
                        updown();
                    }
                    isAttack = true;
                    if (isupdown == true)
                    {
                        box.SetActive(true);
                        rb.AddForce(new Vector2(-1.41f * speed, -1.41f * speed));
                    }
                }
            }
        }
        
        
        if (isground == true&&isEnd==false)
        {
            
            if(s_op==0)
            {
                rb.velocity = new Vector3(10, 0, 0);
            }
            
            if(s_op==1)
            {
                rb.velocity = new Vector3(-10, 0, 0);
            }
        }
        if (isground == true&&groundtime>0f)
            groundtime -= Time.deltaTime;

        if (groundtime <= 0)
            isEnd = true;

        if (isground == true && isEnd==true&&isDown==false)
        {
            isDown = true;//한번만
            box.SetActive(false);
            this.transform.position = new Vector2(Player.position.x, Player.position.y + 12f);
           temp_player =Player;//플레이어 위치
            rb.velocity = Vector2.zero;
            // isground = false;
            // yield return new WaitForSeconds(0.5f);
        }
       
    }
    float t=0;
    IEnumerator Move()
    {
        if (currentLerpTime >= 2*lerpTime)//왼쪽으로 찍었을때 
        {
            currentLerpTime = 0;   
        }
        t= currentLerpTime / lerpTime;
        t = Mathf.Sin(t * Mathf.PI * 0.5f);
        
        //if(this.transform.position.x==min_P)
        this.transform.position = Vector3.Lerp(new Vector2(min_P,center.transform.position.y+6f),new Vector2(max_P, center.transform.position.y + 6f), t);//좌우로 반복으로 움직이게 한다
        yield return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(boxpos.position, new Vector2(3f, 3f));
    }
}
