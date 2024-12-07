using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class g_bottle : MonoBehaviour
{
    int num;
    void Update()
    {
        if (num <= 0)
        {
            Invoke("destroy_this", 0.18f);
            num++;
        }
    }
    void destroy_this()
    {
        gameObject.SetActive(false);
        num = 0;
    }
}
