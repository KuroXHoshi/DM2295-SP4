using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Gameover : MonoBehaviour {
    

    [SerializeField]
    Canvas gameover;
    private float delay = 2.0f;
    public AudioScript go;
    // Use this for initialization
    void Start () {
        gameover.enabled = true;
        go.GO();
	}
	
	// Update is called once per frame
	void Update () {
        delay -= Time.deltaTime;
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Escape)) && (delay<=0.0f))
        {
            gameover.enabled = false;
            SceneManager.LoadScene(0);
        }
	}
}
