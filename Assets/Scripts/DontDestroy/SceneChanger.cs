using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{
    public static SceneChanger instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    public void ChangeScene(string sceneName, GameObject loadingPanel)
    {
        loadingPanel.SetActive(true);
        SceneManager.LoadSceneAsync(sceneName);
    }
}
