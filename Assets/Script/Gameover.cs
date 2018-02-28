using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Gameover : MonoBehaviour {
    

    [SerializeField]
    Canvas gameover;
    private float delay = 2.0f;
 
    // Use this for initialization
    void Start () {
        gameover.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
        delay -= Time.deltaTime;
        if ((Input.GetMouseButtonDown(0))&&(delay<=0.0f))
        {
            gameover.enabled = false;
            SceneManager.LoadScene("MainMenu");
        }
	}
}
