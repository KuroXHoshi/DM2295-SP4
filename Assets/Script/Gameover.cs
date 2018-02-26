using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Gameover : MonoBehaviour {
    

    [SerializeField]
    Canvas gameover;

 
    // Use this for initialization
    void Start () {
        gameover.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            gameover.enabled = false;
            SceneManager.LoadScene("MainMenu");
        }
	}
}
