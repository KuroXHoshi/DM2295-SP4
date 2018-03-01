using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShieldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Player player;

    public bool buttonDown { get; protected set; }
    public bool buttonEnter { get; protected set; }

    private void Awake()
    {
        buttonDown = false;
        buttonEnter = false;
    }

    // Use this for initialization
    void Start ()
    {
        if ((player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>()) == null) Debug.Log(this.GetType() + ".cs : Player not loaded!");
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (player == null)
            return;

        player.ShieldButtonDown = (buttonDown && buttonEnter);
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonDown = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonEnter = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonEnter = false;
    }
}
