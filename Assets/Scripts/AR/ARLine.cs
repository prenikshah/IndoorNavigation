using System.Collections;
using UnityEngine;
using TMPro;

public class ARLine : MonoBehaviour
{
    [SerializeField] GameObject arDestination;
    [SerializeField] LineRenderer arLine;
    [SerializeField] LineRenderer arLineWithTexture;
    [SerializeField] MapMaker mapMaker;

    [SerializeField] TextMeshProUGUI distancetext;
    [SerializeField] TextMeshProUGUI subdistancetext;
    [SerializeField] GameObject arrowDirectionUI;
    [SerializeField] GameObject arrowDirection;

    [SerializeField] GameObject arrowSprite;
    [SerializeField] GameObject destinationSprite;
    [SerializeField] GameObject arrived;

    bool isDestination;
    Vector3 offset = Vector3.zero;
    public GameObject loadingPanel;
    public void OnBack()
    {
        SceneChanger.instance.ChangeScene(DataManager.SelectMap, loadingPanel);
    }
    private IEnumerator Start()
    {
        arrived.SetActive(false);
        loadingPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        loadingPanel.SetActive(false);
        offset = mapMaker.player.transform.position;
        arLine.gameObject.SetActive(true);
        arLineWithTexture.gameObject.SetActive(true);
        isDestination = true;
    }
    private void Update()
    {
        if (isDestination && mapMaker.currentDestination != null)
        {
            GoToDestination(arLine);
            GoToDestination(arLineWithTexture);
            int posCount = PathLine._line.positionCount - 1;
            Vector3 linePos = new Vector3(PathLine._line.GetPosition(posCount).x - offset.x, -1.25f, PathLine._line.GetPosition(posCount).z - offset.z);
            arDestination.transform.position = linePos;
            arDestination.SetActive(true);
            if (mapMaker.player.GetComponent<Player>().isDestination)
                distancetext.text = "Total distance" + "0m";
            else
                distancetext.text = "Total distance" + mapMaker.distance + "0m";
        }
        else
        {
            arDestination.SetActive(false);
            arLine.positionCount = 0;
            arLineWithTexture.positionCount = 0;
        }
    }
    float angle;
    void GoToDestination(LineRenderer line)
    {
        line.positionCount = 2;
        if (PathLine._line.positionCount >= 2)
        {
            for (int i = 0; i < 2; i++)
            {
                Vector3 linePos = new Vector3(PathLine._line.GetPosition(i).x - offset.x, -2.25f, PathLine._line.GetPosition(i).z - offset.z);
                line.SetPosition(i, linePos);
            }
        }
        int posCount = line.positionCount;
        if (posCount < PathLine._line.positionCount)
        {
            Vector3 linePos = new Vector3(PathLine._line.GetPosition(posCount).x - offset.x, -1.25f, PathLine._line.GetPosition(posCount).z - offset.z);
            arrowDirection.transform.position = linePos;
            Vector3 dir = line.GetPosition(0) - linePos;
            angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
            Debug.Log(angle);
            arrowDirection.transform.eulerAngles = new Vector3(0, angle - 90, 0);
            arrowDirection.SetActive(true);
            if (arrowDirection.transform.position == arDestination.transform.position)
            {
                arrowDirection.SetActive(false);
            }
        }
        else
        {
            arrowDirection.SetActive(false);
        }
        UpdateUI();
    }
    void UpdateUI()
    {
        Vector3 dir = arLine.GetPosition(1) - arLine.GetPosition(0);
        float angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        float subDistance = (Vector3.Distance(arLine.GetPosition(0), arLine.GetPosition(1)));
        if (arrowDirection.activeInHierarchy)
        {
            ToggleSprite(true);
            if (subDistance < 2.0f)
            {

                if (angle < 0)
                {
                    arrowDirectionUI.transform.eulerAngles = new Vector3(0, 0, -90);
                    subdistancetext.text = "Turn right after " + subDistance.ToString() + "m";
                }
                else
                {
                    arrowDirectionUI.transform.eulerAngles = new Vector3(0, 0, 90);
                    subdistancetext.text = "Turn left after " + subDistance.ToString() + "m";
                }
            }
            else
            {
                arrowDirectionUI.transform.eulerAngles = new Vector3(0, 0, 0);
                subdistancetext.text = "Go straight ahead for " + subDistance.ToString() + "m";
            }
        }
        else
        {
            arrowDirectionUI.transform.eulerAngles = new Vector3(0, 0, 0);
            if (subDistance < 2.0f)
            {
                ToggleSprite(false);
                if (mapMaker.player.GetComponent<Player>().isDestination)
                {
                    arrived.SetActive(true);
                    subdistancetext.text = "Arriving at destination " + "0m";
                }
                else
                {
                    arrived.SetActive(false);
                    subdistancetext.text = "Arriving at destination " + subDistance.ToString() + "m";
                }
            }
            else
            {
                ToggleSprite(true);
                subdistancetext.text = "Go straight ahead for " + subDistance.ToString() + "m";
            }
        }
    }

    void ToggleSprite(bool param)
    {
        arrowSprite.SetActive(param);
        destinationSprite.SetActive(!param);
    }
}