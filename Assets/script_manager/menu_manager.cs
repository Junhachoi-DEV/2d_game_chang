using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class menu_manager : MonoBehaviour
{
    public GameObject ingame_menu;
    public GameObject gameover_menu;
    public GameObject[] in_button_menu;

    public bool is_menu_show;


    void Update()
    {
        press_esc();
        if (is_menu_show)
        {
            ingame_menu.SetActive(true);
            Time.timeScale = 0f; //Ω√∞£ ∏ÿ√„
        }
        else
        {
            ingame_menu.SetActive(false);
            Time.timeScale = 1f;
        }
    }
    void press_esc()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            is_menu_show_onoff();
        }
    }
    public void is_menu_show_onoff()
    {
        is_menu_show = !is_menu_show;
    }
    public void new_game()
    {
        Time.timeScale = 1f;
        game_manager.Instance.scene_load("loading_scene");
        game_manager.Instance.gm_bg_sound_mng("ingame_bgm");
    }
    public void ingame_quit()
    {
        Time.timeScale = 1f;
        game_manager.Instance.scene_load("main_menu");
        game_manager.Instance.gm_bg_sound_mng("main_menu_bgm");
    }

    public void info()
    {
        check_ingame_menu(0);
    }
    public void inventory()
    {
        check_ingame_menu(1);
    }
    public void setting()
    {
        check_ingame_menu(2);
    }
    public void save_and_quit()
    {
        check_ingame_menu(3);
    }


    void check_ingame_menu(int num)
    {
        for (int i = 0; i < in_button_menu.Length; i++)
        {
            in_button_menu[i].SetActive(false);
            in_button_menu[num].SetActive(true);
        }
    }
}
