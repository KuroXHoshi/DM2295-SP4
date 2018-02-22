using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashFade : MonoBehaviour {

    public Image splashImage;
    public string loadlevel;

     IEnumerator Start()
    {
        splashImage.canvasRenderer.SetAlpha(0.0f);
        FadeIN();
        yield return new WaitForSeconds(2.5f);
        FadeOUT();
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(loadlevel);
    }


    void FadeIN()
    {
        splashImage.CrossFadeAlpha(1.0f, 1.5f, false);
    }

    void FadeOUT()
    {
        splashImage.CrossFadeAlpha(0.0f, 2.5f, false);
    }
}
