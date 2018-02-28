using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class JoyStick : MonoBehaviour, IDragHandler,IPointerUpHandler,IPointerDownHandler {

    public Text printout;
    private Image ImgFG;
    private Image ImgBG;
    private Vector3 inputVector
    { set; get; }

    // Use this for initialization
    private void Start () {
        ImgBG = GetComponent<Image>();
        //ImgFG = transform.GetChild(0).GetComponent<Image>();
        ImgFG = GetComponentsInChildren<Image>()[1];
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Vector2 position;
        Vector2 position = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(ImgBG.rectTransform, eventData.position, eventData.pressEventCamera, out position))
        {
            position.x = (position.x / ImgBG.rectTransform.sizeDelta.x);
            position.y = (position.y / ImgBG.rectTransform.sizeDelta.y);

            float x = (ImgBG.rectTransform.pivot.x == 1) ? position.x * 2 : position.x * 2 - 1;
            float y = (ImgBG.rectTransform.pivot.y == 1) ? position.y * 2 : position.y * 2 - 1;

            inputVector = new Vector3(x, 0, y);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

          

            //Move Joystick image
            ImgFG.rectTransform.anchoredPosition = new Vector3(inputVector.x * (ImgBG.rectTransform.sizeDelta.x/3),inputVector.z *(ImgBG.rectTransform.sizeDelta.y/3));
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector3.zero;
        ImgFG.rectTransform.anchoredPosition = Vector3.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }
    public float Horizontal()
    {
        if (inputVector.x != 0)
            return inputVector.x;
        else
            return Input.GetAxis("Horizontal");
    }
    public float Vertical()
    {
        if (inputVector.z != 0)
            return inputVector.z;
        else
            return Input.GetAxis("Vertical");
    }
}
