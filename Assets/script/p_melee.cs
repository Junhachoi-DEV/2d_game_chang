using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class p_melee : MonoBehaviour
{
    player p;
    obj_manager obj_m;
    private void Start()
    {
        p = FindObjectOfType<player>();
        obj_m = FindObjectOfType<obj_manager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Monster")
        {
            GameObject hit_effect = obj_m.make_obj("hit_ef");
            hit_effect.transform.position = collision.transform.position;
            if (p.atk_num == 1)
            {
                game_manager.Instance.gm_ef_sound_mng("atk1_hit_sound");
            }
            else
            {
                game_manager.Instance.gm_ef_sound_mng("atk2_hit_sound");
            }
            
        }
    }
    
}
