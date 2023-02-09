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
        Destroy(gameObject);
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
        //�ڽ��� ��ġ�� �ڽ��� ũ�⿡ �׸��� ȸ������ �ִ´�
        foreach (Collider2D colider in collider2Ds)
        {
            // Debug.Log("�浹");
            if (colider.tag == "Player")//�ݶ��̴��� �ױ׸� ���ؼ� �÷��̾���� �־���´�
            {
                Debug.Log("player damage");
                damage_manager.Instance.damage_count(1/10);
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
