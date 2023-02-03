using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    [SerializeField] private GameObject loaderCanvas = null;


    public void LoadWellcome(){
        StartCoroutine(LoadAsync("Wellcome"));
    }
    
    public void LoadAuth(){
        StartCoroutine(LoadAsync("Auth"));
    }

    public void LoadHub(){
        StartCoroutine(LoadAsync("Hub"));
    }

    public void LoadTraining(){
        StartCoroutine(LoadAsync("Training"));
    }

    public void LoadCredits(){
        StartCoroutine(LoadAsync("Credits"));
    }
    
    public void LoadGame(){
        StartCoroutine(LoadAsync("Game"));
    }
    
    public void LoadStatus(){
        StartCoroutine(LoadAsync("Status"));
    }

    public void LoadLimbo(){
        StartCoroutine(LoadAsync("Limbo"));
    }


    private void ShowLoader(){
        loaderCanvas.SetActive(true);
    }

    private IEnumerator LoadAsync(string sceneName)
    {   
        ShowLoader();
        yield return new WaitForSeconds(2f);

        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
