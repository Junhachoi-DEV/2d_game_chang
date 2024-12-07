using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class g_p : MonoBehaviour
{
    int num;
    void Update()
    {
        if (num <= 0)
        {
            Invoke("destroy_this", 1.34f);
            num++;
        }
    }
    void destroy_this()
    {
        gameObject.SetActive(false);
        num = 0;
    }
}
