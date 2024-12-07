using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class start_battle2 : MonoBehaviour
{
    Boss_fairy boss;
    void Start()
    {
        boss = FindObjectOfType<Boss_fairy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            boss.is_bettle_start = true;
            gameObject.SetActive(false);
        }
    }
}
