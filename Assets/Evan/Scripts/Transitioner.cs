using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transitioner : MonoBehaviour
{
    private GameObject obj;

    private Scene currentScene;

    GameObject mainCamera;
    GameObject globalLight;

    private void Start()
    {
        mainCamera= Camera.main.gameObject;
        globalLight = GameObject.Find("Global Light 2D");

        obj = GameObject.Find("TransitionHolder");
    }

    public void startloadScene(int index)
    {
        StartCoroutine(loadScene(index));
    }

    IEnumerator loadScene(int index)
    {
        Destroy(globalLight);
        Destroy(mainCamera);

        currentScene = SceneManager.GetActiveScene();

        AsyncOperation scene = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);

        //Wait until we are done loading the scene
        while (scene.progress < 1f)
        {
            yield return null;
        }

        setupScene(index);

        AsyncOperation unload = SceneManager.UnloadSceneAsync(currentScene);
    }

    void setupScene(int index)
    {
        Scene sceneToLoad = SceneManager.GetSceneByBuildIndex(index);
        if (sceneToLoad.IsValid())
        {
            obj.name = "transitionEnder";
            SceneManager.MoveGameObjectToScene(obj, sceneToLoad);          
            SceneManager.SetActiveScene(sceneToLoad);
        }
    }
}
