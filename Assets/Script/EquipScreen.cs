using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EquipScreen : MonoBehaviour
{
    public Transform EQ;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (EQ.gameObject.activeInHierarchy == false)
            {
                EQ.gameObject.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                EQ.gameObject.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }
    public void EQscreenup()
    {

        if (EQ.gameObject.activeInHierarchy == false)
        {
            EQ.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void EQscreenclose()
    {
        if (EQ.gameObject.activeInHierarchy == true)
        {
            EQ.gameObject.SetActive(true);
            Time.timeScale = 1;
        }
    }
}
