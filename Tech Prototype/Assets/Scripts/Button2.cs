using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Button2 : MonoBehaviour, IPointerDownHandler
{

    public bool clicked = false;
    public Text t;
    Button b;

    // Use this for initialization
    void Start()
    {
        b = gameObject.GetComponent<Button>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        EventSystem.current.IsPointerOverGameObject();
        Input.GetMouseButtonDown(0);
        if (clicked && Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            t.text = "Reset";
            b.GetComponent<Image>().color = Color.white;
            clicked = false;
        }

    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (clicked)
        {
            Stats.Moves = Stats.Moves - Stats.CurMoves;
            Stats.CurMoves = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            t.text = "Are you sure?";
            b.GetComponent<Image>().color = Color.red;
            clicked = true;
        }
    }
}
