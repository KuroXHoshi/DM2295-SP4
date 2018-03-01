using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransit : MonoBehaviour
{

    [SerializeField]
    Canvas mainMenu, optionsCanvas, instruct;

    Canvas currentCanvas, TimerCanvas;

    protected void Start()
    {
        // startertime = Time.time;
        optionsCanvas.enabled = false;
        instruct.enabled = false;
        currentCanvas = mainMenu;
    }

    public void Game()
    {
        
    }

    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentCanvas.GetHashCode() == mainMenu.GetHashCode())
                Application.Quit();
            else
                Back();
        }
    }

    public void Options()
    {
        mainMenu.enabled = false;
        optionsCanvas.enabled = true;

        currentCanvas = optionsCanvas;
    }

    public void instructions()
    {
        mainMenu.enabled = false;
        instruct.enabled = true;

        currentCanvas = instruct;
    }

    public void Back() // options back btn, highscore back btn
    {
        mainMenu.enabled = true;
        currentCanvas.enabled = false;
        currentCanvas = mainMenu;
    }

    public void Exit()
    {
        Application.Quit();
    }

}
