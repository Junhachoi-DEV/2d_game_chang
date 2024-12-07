using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class game_manager : MonoBehaviour
{
    #region ΩÃ±€≈Ê
    private static game_manager instance = null;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static game_manager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    #endregion

    background_sound bg_sound;
    effects_sound ef_sound;
    private void Start()
    {
        bg_sound = FindObjectOfType<background_sound>();
        ef_sound = FindObjectOfType<effects_sound>();
    }

    public void gm_bg_sound_mng(string bgm_name)
    {
        bg_sound.play_sounds(bgm_name);
    }
    public void gm_ef_sound_mng(string efm_name)
    {
        ef_sound.play_sounds(efm_name);
    }

    public void scene_load(string name)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(name);
    }
}
