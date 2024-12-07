using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obj_manager : MonoBehaviour
{
    public GameObject hit_ef_prefab;

    //수류탄
    public GameObject exclusion_prefab;
    public GameObject grenade_prefab;
    public GameObject grenade_effect_prefab;
    public GameObject grenade_partical_prefab;
    public GameObject grenade_bottle_prefab;

    //몬스터
    
    public GameObject popcornbug_bullet_prefab;
    public GameObject boss_bullet_prefab;


    public GameObject player;

    GameObject[] hit_ef;

    GameObject[] exclusion;
    GameObject[] grenades;
    GameObject[] grenades_ef;
    GameObject[] grenades_partical;
    GameObject[] grenades_bottle;

    
    GameObject[] popcornbug_bullet;
    GameObject[] boss_bullet;


    GameObject[] target_pool;

    private void Start()
    {
        hit_ef = new GameObject[20];

        exclusion = new GameObject[20];
        grenades = new GameObject[20];
        grenades_ef = new GameObject[20];
        grenades_partical = new GameObject[20];
        grenades_bottle = new GameObject[20];

        
        popcornbug_bullet = new GameObject[50];

        boss_bullet = new GameObject[30];

        generate();
    }

    public void generate()
    {
        for (int i = 0; i < hit_ef.Length; i++)
        {
            hit_ef[i] = Instantiate(hit_ef_prefab);
            hit_ef[i].SetActive(false);
        }

        for (int i = 0; i < exclusion.Length; i++)
        {
            exclusion[i] = Instantiate(exclusion_prefab);
            exclusion[i].SetActive(false);
        }
        for (int i = 0; i < grenades.Length; i++)
        {
            grenades[i] = Instantiate(grenade_prefab);
            grenades[i].SetActive(false);
        }
        for (int i = 0; i < grenades_ef.Length; i++)
        {
            grenades_ef[i] = Instantiate(grenade_effect_prefab);
            grenades_ef[i].SetActive(false);
        }
        for (int i = 0; i < grenades_partical.Length; i++)
        {
            grenades_partical[i] = Instantiate(grenade_partical_prefab);
            grenades_partical[i].SetActive(false);
        }
        for (int i = 0; i < grenades_bottle.Length; i++)
        {
            grenades_bottle[i] = Instantiate(grenade_bottle_prefab);
            grenades_bottle[i].SetActive(false);
        }

        
        for (int i = 0; i < popcornbug_bullet.Length; i++)
        {
            popcornbug_bullet[i] = Instantiate(popcornbug_bullet_prefab);
            popcornbug_bullet[i].SetActive(false);
        }
        for (int i = 0; i < boss_bullet.Length; i++)
        {
            boss_bullet[i] = Instantiate(boss_bullet_prefab);
            boss_bullet[i].SetActive(false);
        }
    }

    public GameObject make_obj(string type)
    {
        switch (type)
        {
            case "hit_ef":
                target_pool = hit_ef;
                break;

            case "exclusion":
                target_pool = exclusion;
                break;
            case "grenades":
                target_pool = grenades;
                break;
            case "grenades_ef":
                target_pool = grenades_ef;
                break;
            case "grenades_partical":
                target_pool = grenades_partical;
                break;
            case "grenades_bottle":
                target_pool = grenades_bottle;
                break;

            
            case "popcornbug_bullet":
                target_pool = popcornbug_bullet;
                break;
            case "boss_bullet":
                target_pool = boss_bullet;
                break;
        }

        for (int i = 0; i < target_pool.Length; i++)
        {
            if (!target_pool[i].activeSelf)
            {
                target_pool[i].SetActive(true);
                return target_pool[i];
            }
        }
        return null;
    }

    public GameObject[] get_pool(string type)
    {
        switch (type)
        {
            case "hit_ef":
                target_pool = hit_ef;
                break;

            case "exclusion":
                target_pool = exclusion;
                break;
            case "grenades":
                target_pool = grenades;
                break;
            case "grenades_ef":
                target_pool = grenades_ef;
                break;
            case "grenades_partical":
                target_pool = grenades_partical;
                break;
            case "grenades_bottle":
                target_pool = grenades_bottle;
                break;

            
            case "popcornbug_bullet":
                target_pool = popcornbug_bullet;
                break;
            case "boss_bullet":
                target_pool = boss_bullet;
                break;
        }
        return target_pool;
    }
}
