using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour {
    public GameObject b;

    SpriteRenderer renderer;

	// Use this for initialization
	void Start () {
        b.SetActive(false);
        renderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            Debug.Log("GATE");
            if (renderer.color.g > 0)
            {
                b.SetActive(true);
            }
            else if (renderer.color.r > 0)
            {
                if (coll.gameObject.GetComponent<SpriteRenderer>().color.r > 0 && coll.gameObject.GetComponent<SpriteRenderer>().color.g == 0)
                {
                    b.SetActive(true);
                }
            }
            else if (renderer.color.b > 0)
            {
                if (coll.gameObject.GetComponent<SpriteRenderer>().color.b > 0 && coll.gameObject.GetComponent<SpriteRenderer>().color.g == 0)
                {
                    b.SetActive(true);
                }
            }
        }
    }
}
