using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour {

    public float changeTime = 2;
    float start;
    SpriteRenderer r;

	// Use this for initialization
	void Start () {
        r = GetComponent<SpriteRenderer>();
        start = Time.time;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	    if (Time.time - 2 >= start)
        {
            if (r.color.r == 1)
            {
                r.color = new Color(0, 0, 1);
            } else
            {
                r.color = new Color(1, 0, 0);
            }
            start = Time.time;
        }
	}
}
