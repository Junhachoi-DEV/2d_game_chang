using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class load_scene_mng : MonoBehaviour
{
    public Slider progress_bar;
    public Text loading_text;

    private void Start()
    {
        StartCoroutine(load_scene_co_ro());
    }
    IEnumerator load_scene_co_ro()
    {
        yield return null;
        //asy~ �� �Ӽ��� ����Ұ��� _ �ε�Ť�async �� ���� �Ѿ�� �߿� ������ �ʰ� ������ �����Ҽ� ����
        AsyncOperation operation = SceneManager.LoadSceneAsync(2);
        operation.allowSceneActivation = false; // ������(90%)�� ����


        while (!operation.isDone)//���� �ҷ������� ���� = ���� �ҷ����� ������
        {
            yield return null;
            if (progress_bar.value < 0.9f)
            {
                progress_bar.value = Mathf.MoveTowards(progress_bar.value, 0.9f, operation.progress);
            }
            else if (operation.progress >= 0.9f)
            {
                progress_bar.value = Mathf.MoveTowards(progress_bar.value, 1f, operation.progress);
            }


            if (progress_bar.value >= 1f)
            {
                loading_text.text = "Press attack Botton";
            }

            if (Input.GetMouseButtonDown(0) && progress_bar.value >= 1f && operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }
        }
    }
}