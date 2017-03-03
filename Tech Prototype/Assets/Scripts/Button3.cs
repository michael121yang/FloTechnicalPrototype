using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Button3 : MonoBehaviour, IPointerDownHandler
{
    public Text tex;
    public bool clicked = false;
    public string next;
    Button b;

    // Use this for initialization
    void Start()
    {
        b = gameObject.GetComponent<Button>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        tex.text = ("Num Moves: " + Stats.CurMoves);
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        Stats.CurMoves = 0;
        SceneManager.LoadScene(next);
        gameObject.SetActive(false);
    }
}
