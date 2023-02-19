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
        // ����� ���� �ҷ���
    }
    public void option()
    {
        // ���� �Ŵ����� �ִ� �ɼ��� �ҷ��ð���.
    }
    public void quit_game()
    {
        Application.Quit();
    }
}
