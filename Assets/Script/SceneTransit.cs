using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransit : MonoBehaviour {

    [SerializeField]
    Canvas mainMenu;

    [SerializeField]
     Canvas optionsCanvas;

    Canvas currentCanvas;

    [SerializeField]
    Canvas FriendlistCanvas;

    Canvas TimerCanvas;

    public Text timertext;
    public float startertime;
    protected void Start()
    {
       // startertime = Time.time;
        optionsCanvas.enabled = false;
    }
    public void Game()
    {
        //startertime = Time.time;
        //TimerCanvas.enabled = true;
        //float timer = 0;
        //timer = Time.deltaTime * 10; // go second by seconds
    }
    void Update()
    {
        //float timer = Time.time - startertime;

        //string Minutes = ((int)timer / 60).ToString();
        //string Seconds = (timer % 60).ToString();

    //    timertext.text = Minutes + ":" + Seconds;
        //if (timer > 5)
        //{
        //   // TimerCanvas.enabled = false;
        //    SceneManager.LoadScene("InGame"); //load title of scene
        //    timer = 0;
        //}
        //Debug.Log("timer running" + timer);
}

public void Options()
    {
        mainMenu.enabled = false;
        optionsCanvas.enabled = true;

        currentCanvas = optionsCanvas;
    }

    public void HighScore()
    {
        mainMenu.enabled = false;
        //highscoreCanvas.enabled = true;

        //currentCanvas = highscoreCanvas;
    }
    public void Back() // options back btn, highscore back btn
    {
        mainMenu.enabled = true;

        //if option, options canvas disable, 
        //if highscore, highscore canvas disable

        currentCanvas.enabled = false;
    }
    public void Exit()
    {
        Application.Quit();
    }

}
