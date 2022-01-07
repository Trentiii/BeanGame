using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transitioner : MonoBehaviour
{
    public GameObject obj;

    GameObject mainCamera;
    GameObject globalLight;

    private void Start()
    {
        mainCamera= Camera.main.gameObject;
        globalLight = GameObject.Find("Global Light 2D");
    }

    public void startloadScene(int index)
    {
        StartCoroutine(loadScene(index));
    }

    IEnumerator loadScene(int index)
    {
        Destroy(globalLight);
        Destroy(mainCamera);

        AsyncOperation scene = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);

        //Wait until we are done loading the scene
        while (scene.progress < 1f)
        {
            yield return null;
        }

        setupScene(2);

        AsyncOperation unload = SceneManager.UnloadSceneAsync(0);
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
