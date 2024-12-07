using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bg_sound_start : MonoBehaviour
{
    Slider slider;
    background_sound bg_sound;
    void Start()
    {
        slider = GetComponent<Slider>();
        bg_sound = FindObjectOfType<background_sound>();
        slider.onValueChanged.AddListener(delegate { bg_slider_cng(); });
    }
    private void bg_slider_cng()
    {
        bg_sound.audio.volume = slider.value;
    }
}
