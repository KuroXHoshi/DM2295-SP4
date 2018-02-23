using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashFade : MonoBehaviour {

    public Image splashImage;
    public Text text;
    public Text text2;
    public string loadlevel;

    public Text displayLabel;

     IEnumerator Start()
    {
        //Flashlabel();
        splashImage.canvasRenderer.SetAlpha(0.0f);
        text.canvasRenderer.SetAlpha(0.0f);
        //text2.canvasRenderer.SetAlpha(0.0f);

        for (int i = 0; i < 4; ++i)
        {
            FadeIN();
            //TranslateRIGHT();
            yield return new WaitForSeconds(2.5f);
            FadeOUT();
            yield return new WaitForSeconds(2.5f);
        }
        SceneManager.LoadScene(loadlevel);
    }


    void FadeIN()
    {
        splashImage.CrossFadeAlpha(1.0f, 1.5f, false);
        text.CrossFadeAlpha(1.0f, 1.5f, false);
    }

    void FadeOUT()
    {
        splashImage.CrossFadeAlpha(0.0f, 2.5f, false);
        text.CrossFadeAlpha(0.0f, 2.5f, false);
    }

    void Flashlabel()
    {
        displayLabel.canvasRenderer.SetAlpha(0.0f);
       
            displayLabel.CrossFadeAlpha(1.0f, 1.2f, false);
            displayLabel.CrossFadeAlpha(0.0f, 1.2f, false);
        
       
    }
}
