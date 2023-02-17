using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class screen_option : MonoBehaviour
{
    FullScreenMode screenMode;
    public Dropdown resolutions_dropdown;
    public Toggle full_screen_b;
    List<Resolution> resolutions = new List<Resolution>();

    public int resoultion_num;

    
    private void Start()
    {
        ui_form();
    }
    void ui_form()
    {
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRate == 60) //60�츣���� ����
            {
                resolutions.Add(Screen.resolutions[i]);
            }
        }
        //resolutions.AddRange(Screen.resolutions);
        resolutions_dropdown.options.Clear(); //���� �ִ� ����ٿ��� ����Ʈ(�ɼ�)���� ����

        int option_num = 0;

        foreach(Resolution item in resolutions)
        {
            Dropdown.OptionData option = new Dropdown.OptionData(); // ��ü�� ����
            option.text = item.width + "x" + item.height + " " + item.refreshRate + "hz";
            resolutions_dropdown.options.Add(option);

            if(item.width == Screen.width && item.height == Screen.height)
            {
                resolutions_dropdown.value = option_num;
            }
            option_num++;
        }
        resolutions_dropdown.RefreshShownValue(); //���� ���ΰ�ħ �Լ�.

        full_screen_b.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;

    }

    public void dropboxoption_change(int x)
    {
        resoultion_num = x;
    }

    public void full_screen_btn(bool is_full)
    {
        screenMode = is_full ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed; 
    }

    public void screen_ok_btn()
    {
        //screenMode = is_full ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        // ����, ����, ��üȭ��, �츣��
        Screen.SetResolution(resolutions[resoultion_num].width, resolutions[resoultion_num].height, screenMode);

    }

}
