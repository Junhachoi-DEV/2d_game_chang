using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class main_menu_manager : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1f;
    }
    public void new_game()
    {
        game_manager.Instance.scene_load("story1");
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
