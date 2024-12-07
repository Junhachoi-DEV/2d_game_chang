using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class background_sound : MonoBehaviour
{
    public AudioClip main_menu_bgm;
    public AudioClip story1_bgm;
    public AudioClip ingame_bgm;
    public AudioClip boss_bgm;
    public AudioClip none_sound;

    public AudioSource audio;
 

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    
    public void play_sounds(string action)
    {
        switch (action)
        {
            case "main_menu_bgm":
                audio.clip = main_menu_bgm;
                break;
            case "story1_bgm":
                audio.clip = story1_bgm;
                break;
            case "ingame_bgm":
                audio.clip = ingame_bgm;
                break;
            case "boss_bgm":
                audio.clip = boss_bgm;
                break;
            case "none_sound":
                audio.clip = none_sound;
                break;
        }
        audio.Play();
    }
}
