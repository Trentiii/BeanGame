using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transitioner : MonoBehaviour
{
    public GameObject TransitionHolder;
    public GameObject backupCamera;

    private Scene currentScene;

    GameObject mainCamera;
    GameObject globalLight;

    private void Start()
    {
        mainCamera = Camera.main.gameObject;
        globalLight = GameObject.Find("Global Light 2D");
    }

    public void startloadScene(int index)
    {
        StartCoroutine(loadScene(index));
    }

    IEnumerator loadScene(int index)
    {
        Destroy(globalLight);
        mainCamera.transform.tag = "Untagged";
        Destroy(mainCamera.GetComponent<AudioListener>());

        currentScene = SceneManager.GetActiveScene();

        AsyncOperation scene = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        Scene sceneToLoad = SceneManager.GetSceneByBuildIndex(index);

        backupCamera.SetActive(true);

        //Wait until done loading the scene
        while (scene.progress < 1f)
        {
            yield return null;
        }

        if (sceneToLoad.IsValid())
        {
            TransitionHolder.name = "transitionEnder";
            SceneManager.MoveGameObjectToScene(TransitionHolder, sceneToLoad);
        }

        AsyncOperation unload = SceneManager.UnloadSceneAsync(currentScene);
    }

    void setupScene(int index)
    {
        Scene sceneToLoad = SceneManager.GetSceneByBuildIndex(index);
        if (sceneToLoad.IsValid())
        {
            TransitionHolder.name = "transitionEnder";
            SceneManager.MoveGameObjectToScene(TransitionHolder, sceneToLoad);          
            SceneManager.SetActiveScene(sceneToLoad);
        }
    }
}
