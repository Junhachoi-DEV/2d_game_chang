using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class popcorn_bullet : MonoBehaviour
{
    Rigidbody2D rb;
    bool isground;
    Animator animator;
    public Vector2 boxSize;
    public Transform boxpos;
    public GameObject box;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    private void Update()
    {
        if(isground == true)
        {
            animator.SetBool("Grow", true);
        }
       
    }
    public void boom()
    {
        animator.SetBool("Ready", true);

      
    }
    public void Destroy()
    {
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="bottom")
        {
            rb.gravityScale = 0;
            rb.velocity = new Vector3(0, 0, 0);
            isground = true;
        }

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(boxpos.position, boxSize, 0);
        //박스의 위치와 박스의 크기에 그리고 회전값을 넣는다
        foreach (Collider2D colider in collider2Ds)
        {
            // Debug.Log("충돌");
            if (colider.tag == "Player")//콜라이더의 테그를 비교해서 플레이어면은 넣어놓는다
            {
                //Debug.Log("player damage");
                damage_manager.Instance.damage_count(1);
                StartCoroutine(attack());
            }
        }
        
    }

    


    IEnumerator attack()
    {
    yield return new WaitForSeconds(0.3f);

        box.SetActive(false);
    }
}
