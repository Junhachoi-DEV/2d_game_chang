using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effects_sound : MonoBehaviour
{
    //이펙트 사운드
    public AudioClip atk1_sound;
    public AudioClip atk2_sound;
    public AudioClip atk1_hit_sound;
    public AudioClip atk2_hit_sound;
    public AudioClip jump_step_sound;
    public AudioClip land_sound;
    public AudioClip wall_jump_step_sound;
    public AudioClip grenade_sound;
    public AudioClip dash_sound;

    //public AudioClip enemy2_sound;
    //public AudioClip enemy3_sound;
    //public AudioClip boss_sound;


    //ui사운드
    public AudioClip click_sound;


    public AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }


    public void play_sounds(string action)
    {
        switch (action)
        {
            case "atk1_sound":
                audio.clip = atk1_sound;
                break;
            case "atk1_hit_sound":
                audio.clip = atk1_hit_sound;
                break;
            case "atk2_sound":
                audio.clip = atk2_sound;
                break;
            case "atk2_hit_sound":
                audio.clip = atk2_hit_sound;
                break;
            case "jump_step_sound":
                audio.clip = jump_step_sound;
                break;
            case "land_sound":
                audio.clip = land_sound;
                break;
            case "wall_jump_step_sound":
                audio.clip = wall_jump_step_sound;
                break;
            case "grenade_sound":
                audio.clip = grenade_sound;
                break;
            case "dash_sound":
                audio.clip = dash_sound;
                break;
            case "click_sound":
                audio.clip = click_sound;
                break;
        }
        audio.Play();
    }
}
