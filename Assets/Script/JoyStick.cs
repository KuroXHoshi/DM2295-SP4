using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

 
public class JoyStick : MonoBehaviour {

    public Text printout;
    private Image ImgFG;
    private Image ImgBG;

    // Use this for initialization
    void Start () {
        ImgBG = GetComponent<Image>();
        ImgFG = transform.GetChild(0).GetComponent<Image>();
	}
	
    public void Drag()
    {
        Vector3 newPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1);
        ImgFG.rectTransform.position = newPos;
       // printout.text = "x: " + Input.mousePosition.x + "y: " + Input.mousePosition.y;
    }

    public void ReturnOriginal()
    {
        ImgFG.rectTransform.anchoredPosition = new Vector3(0, 0, 1);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
