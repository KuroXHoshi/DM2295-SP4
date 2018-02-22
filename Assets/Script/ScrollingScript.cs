using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingScript : MonoBehaviour {

    // Use this for initialization
    public float speed;
	// Update is called once per frame
	void Update () {
      Vector2 offSet = new Vector2(0, Time.time * speed)  ;
        GetComponent<Renderer>().material.mainTextureOffset = offSet;
	}
}
