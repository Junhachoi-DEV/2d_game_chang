using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class granade_destroy : MonoBehaviour
{
    
    obj_manager obj_m;
    float ro_speed =350;
    
    void Update()
    {
        transform.Rotate(0, 0, Time.deltaTime * ro_speed, Space.Self);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "bottom" || collision.gameObject.tag == "Monster")
        {
            //Instantiate(boom_position, transform.position, boom_position.transform.rotation);
            obj_m = FindObjectOfType<obj_manager>();

            //GameObject excolusion = obj_m.make_obj("exclusion");
            GameObject granade_effect = obj_m.make_obj("grenades_ef");
            GameObject granade_partical_ef = obj_m.make_obj("grenades_partical");
            GameObject granade_bottle_effect = obj_m.make_obj("grenades_bottle");
            game_manager.Instance.gm_ef_sound_mng("grenade_sound");

            //excolusion.transform.position = gameObject.transform.position;
            granade_effect.transform.position = gameObject.transform.position;
            granade_partical_ef.transform.position = gameObject.transform.position;
            granade_bottle_effect.transform.position = gameObject.transform.position;

            gameObject.SetActive(false);
        }
    }
}
