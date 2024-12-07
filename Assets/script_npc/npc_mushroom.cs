using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npc_mushroom : MonoBehaviour
{
    public GameObject press_f;
    dialogue_controller controller;
    private void Start()
    {
        controller = FindObjectOfType<dialogue_controller>();
    }
    private void Update()
    {
        if (controller.mushroom && Input.GetKeyDown(KeyCode.F))
        {
            controller.is_talk = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            press_f.SetActive(true);
            controller.mushroom = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            press_f.SetActive(false);
            controller.mushroom = false;
        }
    }
}

