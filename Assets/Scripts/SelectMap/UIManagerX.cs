using UnityEngine;
using TMPro;
public class UIManagerX : MonoBehaviour
{
    [Header("Object Panel")]
    [SerializeField] GameObject destinationPanel;
    [SerializeField] GameObject scanPanel;
    [SerializeField] GameObject loadingPanel;

    [Header("Destination panel")]
    [SerializeField] TextMeshProUGUI placeFoundText;
    [SerializeField] GameObject destinationPrefab;
    [SerializeField] Transform destinationContent;

    [Header("Editor API Data")]
    [SerializeField] Texture images;

    public bool isDestinationSelected;
    public static UIManagerX instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        TogglePanel(false);
        loadingPanel.SetActive(false);
        LoadDestination();
    }

    void LoadDestination()
    {
        placeFoundText.text = DataManager.destinationName.Length.ToString() + " places found";
        foreach (var destination in DataManager.destinationName)
        {
            var des = Instantiate(destinationPrefab, destinationContent);
            des.GetComponent<Destination>().destinationText.text = destination;
            des.GetComponent<Destination>().destinationImage.texture = images;
        }
    }

    public void OnBack()
    {
        if (destinationPanel.activeInHierarchy)
        {
            SceneChanger.instance.ChangeScene(DataManager.Start, loadingPanel);
        }
        else
        {
            TogglePanel(false);
        }
    }


    public void TogglePanel(bool param)
    {
        isDestinationSelected = param;
        destinationPanel.SetActive(!param);
        scanPanel.SetActive(param);
    }

}
