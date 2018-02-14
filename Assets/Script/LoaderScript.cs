using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoaderScript : MonoBehaviour {
    //[SerializeField]
    //Canvas mainMenu;
    public GameObject LoadScreen;
    public Slider slider;
    public Text progresstext;

    public void Loadlevel(int SceneIndex)
    {
       // mainMenu.enabled = false;
        StartCoroutine(LoadAsynchronously(SceneIndex));
    }
    IEnumerator LoadAsynchronously(int SceneIndex)
    {
        //mainMenu.enabled = false;
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneIndex);
        LoadScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            slider.value = progress;
            progresstext.text = progress * 100f + "%";
            yield return null;
        }
    }
}
    

