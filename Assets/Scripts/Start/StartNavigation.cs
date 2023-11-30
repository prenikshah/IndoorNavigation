using UnityEngine;

public class StartNavigation : MonoBehaviour
{
    [SerializeField] GameObject loadingPanel;

    private void Start()
    {
        loadingPanel.SetActive(false);
    }
    public void OnNavigation()
    {
        SceneChanger.instance.ChangeScene("SelectMap", loadingPanel);
    }
}
