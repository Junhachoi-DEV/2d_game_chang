using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    //플레이어 정보
    public int player_hp;
    //속도
    public float apply_speed; // 현재 스피드
    public float crouch_speed = 0f;
    public float run_speed = 6f;
    public float walk_speed= 3f;
    public float dash_speed= 8f;

    //레이 길이
    public float ray_dis;
    public float ray_wall_dis;

    //사용횟수 인트
    int stop_cnt; //벽점프 멈추게하는것 한번만 실행

    //타임
    float dash_timer;
    [Range(0.1f, 3)] public float dash_time;

    //점프
    public float jump_force = 10f; //점프 힘 값
    public float hook_jump_force = 8f; //후크점프 힘 값
    public float wall_jump_force = 5f; //벽점프 힘 값

    // 가로,세로 이동 값
    float x;
    float y;

    //기타 오브젝트
    public GameObject granade;
    public GameObject hiar;
    public GameObject p_melee;

    //총알 및 폭탄 힘, 기타 값
    public float g_force;

    //상태
    bool is_trun; //앞 ,뒤 전환 상태
    bool is_ground; //땅 상태
    bool is_air; //공중 상태
    bool is_wall_jump_ready; //벽 점프 준비 상태
    bool is_dash; //대쉬 상태
    bool ray_wall; //벽 상태
    bool is_hitted; //피격 상태
    bool is_attacking; //공격 중
    public bool is_hook_range_max; // 갈고리 길이 최대 상태

    // 컴포넌트
    Rigidbody2D rigid;
    grapping grap;
    Animator anime;
    SpriteRenderer sprite;
    void Start()
    {
        anime = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>(); // 선언
        sprite = GetComponent<SpriteRenderer>();
        apply_speed = run_speed; // 기존 스피드는 run속도
        grap = GetComponent<grapping>();
        is_trun = true;
    }

    void Update()
    {
        check_wall_and_bottom();
        player_jump();
        player_wall_jump();
        player_use_granade();
        player_dash();
        player_attack();
    }

    // 이동은 효율을 위해 여기에 넣는다.
    void FixedUpdate()
    {
        if (is_hitted)
        {
            return;
        }
        player_move();
    }

    void player_move()
    {
        if (is_wall_jump_ready || is_attacking)
        {
            return; //위에 조건이면 함수를 끝냄.
        }
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        if (Input.GetButton("Horizontal"))
        {
            anime.SetBool("is_run", true);
            hiar.transform.localPosition = new Vector3(0.1f, 0.15f, 0);
            if (x != 0)
            {
                transform.localScale = new Vector3(x, 1, 1); //캐릭터 뒤집기
            }
            if (grap.is_attach) // 갈고리에 붙을시
            {
                rigid.AddForce(new Vector2(x * apply_speed * Time.deltaTime, 0),ForceMode2D.Impulse); //좌,우 이동
            }
            else
            {
                transform.Translate(new Vector3(x * apply_speed * Time.deltaTime, 0, 0)); // 기본 이동
            }
        }
        else
        {
            anime.SetBool("is_run", false);
        }
        if (Input.GetButton("Vertical") && !is_hook_range_max)
        {
            if (grap.is_attach) // 갈고리에 붙을시
            {
                transform.Translate(new Vector3(0, y * apply_speed * Time.deltaTime, 0)); //위,아래 이동
            }
        }
    }

    //벽 && 바닥 체크.
    void check_wall_and_bottom()
    {
        if (x != 0)
        {
            if (x == 1)
            {
                is_trun = true;
            }
            else
            {
                is_trun = false;
            }
        }
        //땅체크
        Debug.DrawRay(rigid.position, Vector2.down * ray_dis, new Color(0, 1, 0));
        is_ground = Physics2D.Raycast(rigid.position, Vector2.down * ray_dis, ray_dis, LayerMask.GetMask("bottom"));
        
        //벽체크
        Debug.DrawRay(rigid.position, Vector2.right * (is_trun ? ray_wall_dis : ray_wall_dis * -1), new Color(0, 1, 0));
        ray_wall = Physics2D.Raycast(rigid.position, Vector2.right * (is_trun ? ray_wall_dis : ray_wall_dis * -1), ray_wall_dis, LayerMask.GetMask("bottom"));
    }

    void player_jump()
    {
        if (is_wall_jump_ready||is_attacking)
        {
            anime.SetBool("do_jump", false);
            return; //위에 조건이면 함수를 끝냄.
        }
        if (!is_ground)
        {
            anime.SetBool("do_jump", true);
        }
        else
        {
            anime.SetBool("do_jump", false);
        }
        if (Input.GetButtonDown("Jump") && is_ground)
        {
            is_air = false;
            Invoke("jump_ani_deley", 0.5f);
            rigid.velocity = new Vector2(rigid.velocity.x, jump_force);
            anime.SetBool("do_jump", true);
        }
        else if (Input.GetButtonDown("Jump") && grap.is_attach) // 갈고리에 붙을시
        {
            is_air = false;
            Invoke("jump_ani_deley", 0.5f);
            rigid.velocity = Vector2.zero;
            rigid.AddForce(new Vector2(rigid.velocity.x, hook_jump_force),ForceMode2D.Impulse);
            anime.SetBool("do_jump", true);
        }
        if(is_air && is_ground)
        {
            anime.SetBool("do_jump", false);
        }
    }
    void jump_ani_deley()
    {
        is_air = true;
    }
    void player_wall_jump()
    {
        if (is_attacking)
        {
            return;
        }
        if (is_hitted)
        {
            anime.SetBool("is_wall", false);
            is_wall_jump_ready = false;
            rigid.gravityScale = 2.5f;
            stop_cnt = 0;
  
            return;
        }
        if(ray_wall && !is_ground) //벽에 붙었고 땅에 없을시.
        {
            anime.SetBool("is_wall", true);
            #region 벽매달리기 취소
            //왼쪽 벽에서 오른쪽으로 가면 벽매달리기 취소

            if (!is_trun && Input.GetKey(KeyCode.D))
            {
                is_wall_jump_ready = false;
                anime.SetBool("is_wall", false);
                anime.SetBool("do_jump", true);
                stop_cnt = 0;
                return;
            }
            //오른쪽 벽에서 왼쪽으로 가면 벽매달리기 취소
            if (is_trun && Input.GetKey(KeyCode.A))
            {
                is_wall_jump_ready = false;
                anime.SetBool("is_wall", false);
                anime.SetBool("do_jump", true);
                stop_cnt = 0;
                return;
            }
            #endregion
            
            is_wall_jump_ready = true; //벽점프 준비 완료.
            
            if (stop_cnt <= 0)
            {
                
                rigid.velocity = Vector2.zero; // 멈춤.
                stop_cnt++;
            }
            
            rigid.gravityScale = 0;

            
            if (Input.GetButtonDown("Jump")) //벽에서 점프를 눌렀을시.
            {
                // is_trun이 트루면 왼쪽으로 펄스면 오른쪽으로 튕김 (즉 왼쪽벽에서 점프를 누르면 오른쪽으로 튕김)
                rigid.velocity = new Vector2(wall_jump_force * (is_trun ? -1 : 1), wall_jump_force * 1.5f);
                anime.SetBool("is_wall", false);
                anime.SetBool("do_jump", true);
                Invoke("wall_jump_deley", 0.15f); // 튕기고 딜레이
            }
        }
        else if (ray_wall && is_ground)
        {
            anime.SetBool("is_wall", false);
            rigid.gravityScale = 2.5f;
            is_wall_jump_ready = false;
            stop_cnt = 0;
        }
        else
        {
            anime.SetBool("is_wall", false);
            rigid.gravityScale = 2.5f;
        }
    }
    void wall_jump_deley()
    {
        is_wall_jump_ready = false;
        stop_cnt = 0;
        rigid.velocity = new Vector2(0, rigid.velocity.y);
    }

    void player_use_granade()
    {
        if (is_attacking)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // 참고로 중력값은 2이다.
            GameObject ins_granade = Instantiate(granade, transform.position, transform.rotation); // 캐릭터 위치에서 생성, 나중에 오브젝트 풀링 해줄거임.
            Rigidbody2D rigid_granade = ins_granade.GetComponent<Rigidbody2D>(); // 물리 선언
            
            //캐릭터위치에서 (위* 힘* 조절) + (오른쪽 * 힘* 캐릭터 바라보는 방향) = 대각선으로 포물선을 그린다.
            rigid_granade.velocity = (transform.up * g_force *0.7f ) + (transform.right * g_force * (is_trun ? 1 : -1));
           
        }
    }
    
    void player_dash()
    {
        if (is_attacking)
        {
            return;
        }
        if(x != 0) //서있지 않을 때
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                is_dash = true;

                if (is_dash)
                {
                    dash_timer += Time.deltaTime;
                    if (dash_timer >= dash_time)
                    {
                        apply_speed = run_speed;
                    }
                    else
                    {
                        apply_speed = dash_speed;
                    }
                }
            }
            
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            dash_timer = 0;
            is_dash = false;
            apply_speed = run_speed;
        }
    }

    void player_attack()
    {
        if (is_attacking || is_hitted || is_wall_jump_ready)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            anime.SetTrigger("is_atk");
        }
        else if(!is_ground && Input.GetMouseButtonDown(0))
        {
            anime.SetTrigger("is_atk");
        }
    }
    #region 공격 로직 //유니티 애니메이션으로 조절한다.
    public void atk_true()
    {
        is_attacking = true;
    }
    public void atk_false()
    {
        is_attacking = false;
    }
    public void atk_collider_on()
    {
        p_melee.SetActive(true);
    }
    public void atk_collider_off()
    {
        p_melee.SetActive(false);
    }
    #endregion  
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "monster_melee")
        {
            is_hitted = true;
            anime.SetTrigger("is_hitted");
            gameObject.layer = 14; // 무적
            sprite.color = new Color(1, 1, 1, 0.5f); //투명해짐
            player_hp -= damage_manager.Instance.en_dmg;
            Invoke("hitted_deley", 0.3f);
            Invoke("hitted_back", 2f);
        }
    }
    void hitted_back()
    {
        gameObject.layer = 3; // 무적 다시 돌아옴
        sprite.color = new Color(1, 1, 1, 1);
    }
    void hitted_deley()
    {
        is_hitted = false;
    }
}
