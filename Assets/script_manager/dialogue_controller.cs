using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable] //�������� Ŭ�󽺿� �����Ҽ��ֵ��� ���ش�.
public class _dialogue
{
    [TextArea]//�ν�����â�� ���� �̻� �����ְ� �������
    public string dialogue;  //��
    public Image cgs; // ��ü�̹���.(ǥ����ȭ ���)

}
public class dialogue_controller : MonoBehaviour
{
    public Image character_img;
    public Text dialog_talk;
    public GameObject t_panel;
    public float dialog_speed;

    public int count = 0; //���° �������
    public bool is_talk =true;
    public bool is_typing;
    bool text_finish;

    //��ȭ ���
    public bool story1;
    public bool mushroom;
    public bool npc_spider_store;

    public _dialogue[] dialogues; //�迭

    

    private void Update()
    {
        is_talking_start();
    }
    
    void is_talking_start()
    {
        if (is_talk)
        {
            t_panel.SetActive(true);
            if (!is_typing && Input.GetMouseButtonDown(0))
            {
                if (count < dialogues.Length) //��??���Ƣ� ����������???
                {
                    text_finish = false;
                    next_dialogue();
                }
                else
                {
                    is_talk = false;
                    //fade.fade_out();
                    if (story1)
                    {
                        Invoke("scene_load_deley", 0.1f);
                    }
                }
            }
            else if(is_typing && Input.GetMouseButtonDown(0))
            {
                text_finish = true;
                dialog_talk.text = dialogues[count-1].dialogue;
                is_typing = false;
            }
        }
        else
        {
            //is_typing = false;
            //text_finish = true;
            dialog_talk.text = "";
            t_panel.SetActive(false);
            count = 0;
        }
    }
    public void is_t_start_botton()
    {
        is_talk = true;
    }
    public void next_dialogue()
    {

        dialog_talk.text = "";
        string sample_text = dialogues[count].dialogue;

        if (count <= dialogues.Length)
        {
            dialogues[count].cgs.gameObject.SetActive(true);
            if (count > 0 && dialogues[count].cgs.name != character_img.name)
            {
                dialogues[count - 1].cgs.gameObject.SetActive(false);
            }
        }

        character_img = dialogues[count].cgs;

        //�ε��� ���� ����
        #region �ʿ������(�׳� ��ο� ������ ���� ��)
        /*
        if (count != 0)
        {
            dialogues[count - 1].cgs.color = new Color(0.3f, 0.3f, 0.3f, 1); //��ξ���
            if(character_img.name == character_img.name) //���� �����̶��
            {
                character_img.color = new Color(1, 1, 1, 1); //�����
            }
        }
        */
        #endregion

        count++;//����
        StartCoroutine(typing(sample_text));
    }
    void scene_load_deley()
    {
        is_talk = true;
        game_manager.Instance.scene_load("loading_scene");
        game_manager.Instance.gm_bg_sound_mng("ingame_bgm");
    }

    IEnumerator typing(string text)
    {
        foreach (char letter in text.ToCharArray())//���ڿ��� �� ���ھ� �ɰ��� �̸� charŸ���� �迭�� ����־���
        {
            if (text_finish)
            {
                break;
            }
            is_typing = true;
            
            dialog_talk.text += letter;
            yield return new WaitForSeconds(dialog_speed);
        }
        is_typing = false;
        //yield return new WaitForSeconds(0.05f);

    }
}
