using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceBehind : MonoBehaviour {

    ParticleSystem particleSystem;


    // Use this for initialization
    void Start () {
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.GetComponent<Renderer>().sortingLayerName = "FX";
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
