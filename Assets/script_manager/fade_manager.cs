using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fade_manager : MonoBehaviour
{
    [Range(0.01f, 10f)] public float fade_time;
    public Image fade_img;

    //public bool bool_fade;
    /*
    private void Update()
    {
        fade_in_out();
    }

    void fade_in_out()
    {
        Color color = fade_img.color;

        if (color.a > 0 && bool_fade)
        {
            color.a -= Time.deltaTime;
        }
        else if(color.a < 1 && !bool_fade)
        {
            color.a += Time.deltaTime;
        }
        fade_img.color = color;
    }
    *///¾È¾²´Â°Å

    public void fade_in() //¹à¾ÆÁü
    {
        StartCoroutine(fade_in_out_co_ro(1, 0));
    }
    public void fade_out() //¾îµÎ¿öÁü
    {
        StartCoroutine(fade_in_out_co_ro(0, 1));
    }
    IEnumerator fade_in_out_co_ro(float start, float end)
    {
        float cur_time = 0f;
        float percent = 0f;

        while(percent < 1)
        {
            cur_time += Time.deltaTime;
            percent = cur_time / fade_time;

            Color color = fade_img.color;
            color.a = Mathf.Lerp(start, end, percent);
            fade_img.color = color;

            yield return null;
        }
    }
}
