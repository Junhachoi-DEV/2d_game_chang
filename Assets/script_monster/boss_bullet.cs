using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss_bullet : MonoBehaviour
{
    // Start is called before the first frame update
    private void FixedUpdate()
    {
        transform.Translate(Vector2.down*Time.deltaTime*4);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="boss_bullet_bottom")
        {
            gameObject.SetActive(false);
        }
        if(collision.tag=="Player")
        {
            damage_manager.Instance.damage_count(2);
            gameObject.SetActive(false);
        }
    }
}
