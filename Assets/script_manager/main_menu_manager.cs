using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class main_menu_manager : MonoBehaviour
{
    background_sound bg_sound_mng;
    effects_sound ef_sound_mng;
    private void Start()
    {
        bg_sound_mng = FindObjectOfType<background_sound>();
        ef_sound_mng = FindObjectOfType<effects_sound>();
        Time.timeScale = 1f;
    }
    public void new_game()
    {
        game_manager.Instance.scene_load("story1");
        bg_sound_mng.play_sounds("story1_bgm");
    }
    public void load_game()
    {
        // 저장된 씬을 불러옴
    }
    public void option()
    {
        // 게임 매니져에 있는 옵션을 불러올것임.
    }
    public void quit_game()
    {
        Application.Quit();
    }
}
