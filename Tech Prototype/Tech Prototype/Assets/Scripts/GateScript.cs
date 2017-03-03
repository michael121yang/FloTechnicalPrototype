using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour {

    SpriteRenderer renderer;

	// Use this for initialization
	void Start () {
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
                gameObject.SetActive(false);
            }
            else if (renderer.color.r > 0)
            {
                if (coll.gameObject.GetComponent<SpriteRenderer>().color.r > 0 && coll.gameObject.GetComponent<SpriteRenderer>().color.g == 0)
                {
                    gameObject.SetActive(false);
                }
            }
            else if (renderer.color.b > 0)
            {
                if (coll.gameObject.GetComponent<SpriteRenderer>().color.b > 0 && coll.gameObject.GetComponent<SpriteRenderer>().color.g == 0)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
