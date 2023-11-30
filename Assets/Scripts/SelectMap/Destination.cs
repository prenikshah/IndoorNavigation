using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Destination : MonoBehaviour
{
    public TextMeshProUGUI destinationText;
    public RawImage destinationImage;

    public void OnDestinationClick()
    {
        UIManagerX.instance.TogglePanel(true);
    }
}
