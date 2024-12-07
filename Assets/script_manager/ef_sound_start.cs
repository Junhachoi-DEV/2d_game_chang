using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ef_sound_start : MonoBehaviour
{
    Slider slider;
    effects_sound ef_sound;
    void Start()
    {
        slider = GetComponent<Slider>();
        ef_sound = FindObjectOfType<effects_sound>();
        slider.onValueChanged.AddListener(delegate { ef_sound_cng(); });
    }

    public void ef_sound_cng()
    {
        ef_sound.audio.volume = slider.value;
    } 
}
