using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class SceneChangeManager : Singleton<SceneChangeManager>
{
    string LoadSceneTemp; 

    public void ChangeSceneTo(string toScene)
    {
        LoadSceneTemp= toScene;
        StartCoroutine(SceneTimeout()); 
    }

    IEnumerator SceneTimeout()
    {
        GetComponent<Animator>().SetTrigger("ExitScene"); 
        yield return new WaitForSeconds(1.25f);
        SceneManager.LoadScene(LoadSceneTemp);
    }
}
