using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;

public class Scanner : MonoBehaviour
{
    [SerializeField] ARTrackedImageManager m_TrackedImageManager;

    [SerializeField] GameObject loadingPanel;

    [SerializeField]
    UnityEvent OnImageTrackedEvent = new UnityEvent();

    bool markerTracked = false;


    bool editor;
    private void Start()
    {
        loadingPanel.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSuccess();
        }
        if (UIManagerX.instance.isDestinationSelected && editor == false)
        {
            StartCoroutine(WaitSec());
            editor = true;
        }
        else
        {
            editor = false;
        }
    }
    IEnumerator WaitSec()
    {
        yield return new WaitForSeconds(2f);
        OnSuccess();
    }

    void OnEnable()
    {

        m_TrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }
    void OnDisable()
    {
        m_TrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }
    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        if (UIManagerX.instance.isDestinationSelected)
        {
            foreach (var trackedImage in eventArgs.added)
            {
                // Give the initial image a reasonable default scale
                trackedImage.transform.localScale = new Vector3(0.01f, 1f, 0.01f);
                if (markerTracked) return;
                OnSuccess();
            }

            foreach (var trackedImage in eventArgs.updated)
            {
                if (markerTracked) return;
                OnSuccess();
            }
        }
    }
    void OnSuccess()
    {
        markerTracked = true;
        m_TrackedImageManager.enabled = false;
        OnImageTrackedEvent.Invoke();
        SceneChanger.instance.ChangeScene(DataManager.ARMap, loadingPanel);
    }

}
