using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class start_battle : MonoBehaviour
{
    public GameObject potal1;
    public GameObject potal2;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            game_manager.Instance.gm_bg_sound_mng("none_sound");
            potal1.SetActive(false);
            potal2.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
