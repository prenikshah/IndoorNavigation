﻿using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.SpatialTracking;
/// <summary>
/// Controls the IndoorLocalisation example.
/// </summary>
public class ARPoseTracker : MonoBehaviour
{

    /// <summary>
    /// The first-person camera being used to render the passthrough camera image (i.e. AR
    /// background).
    /// </summary> 
    [SerializeField] TrackedPoseDriver mDriver;


    /// <summary>
    /// Remember previous position in order to calculate rotations and translations
    /// </summary>
    private Vector3 PrevARPosePosition;

    /// <summary>
    /// Saying AR is tracking ot not
    /// </summary>
    private bool Tracking = false;

    /// <summary>
    /// The Unity Start() method.
    /// </summary>
    public void Start()
    {
        //set initial position
        PrevARPosePosition = Vector3.zero;
    }

    /// <summary>
    /// The Unity Update() method.
    /// </summary>
    public void Update()
    {
        UpdateApplicationLifecycle();

        //move the person indicator according to positionn
        //Vector3 currentARPosition = Frame.Pose.position;
        PoseDataSource.GetDataFromSource(mDriver.poseSource, out Pose resultPose);
        Vector3 currentARPosition = resultPose.position;
        if (!Tracking)
        {
            Tracking = true;
            //PrevARPosePosition = Frame.Pose.position;
            //PoseDataSource.GetDataFromSource(mDriver.poseSource, out resultPose);
            currentARPosition = resultPose.position;
            //PrevARPosePosition = mDriver.originPose.position;
        }
        //Remember the previous position so we can apply deltas
        Vector3 deltaPosition = currentARPosition - PrevARPosePosition;
        PrevARPosePosition = currentARPosition;

        // The initial forward vector of the sphere must be aligned with the initial camera direction in the XZ plane.
        // We apply translation only in the XZ plane.
        this.transform.Translate(deltaPosition.x, 0.0f, deltaPosition.z);
        // Set the pose rotation to be used in the CameraFollow script
        //Follower.targetRot = Frame.Pose.rotation;

        //mMapController.targetRot = resultPose.rotation;
    }

    /// <summary>
    /// Check and update the application lifecycle.
    /// </summary>
    private void UpdateApplicationLifecycle()
    {
        // Exit the app when the 'back' button is pressed.
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Only allow the screen to sleep when not tracking. 
        if (ARSession.state != ARSessionState.SessionTracking)
        {
            Tracking = false;
            const int lostTrackingSleepTimeout = 15;
            Screen.sleepTimeout = lostTrackingSleepTimeout;
            return;
        }
        else
        {
            //if (ErrorText.text.Equals("Lost tracking, wait ..."))
            //{
            //    ErrorText.text = "";
            //}
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        //if (IsQuitting)
        //{
        //    return;
        //}

        /*
        // Quit if ARCore was unable to connect and give Unity some time for the toast to
        // appear.
        if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
        {
            ShowAndroidToastMessage("Camera permission is needed to run this application.");
            IsQuitting = true;
            Invoke("DoQuit", 0.5f);
        }
        else if (Session.Status.IsError())
        {
            ShowAndroidToastMessage(
                "ARCore encountered a problem connecting.  Please start the app again.");
            IsQuitting = true;
            Invoke("DoQuit", 0.5f);
        }
        */
    }

    /// <summary>
    /// Actually quit the application.
    /// </summary>
    private void DoQuit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Show an Android toast message.
    /// </summary>
    /// <param name="message">Message string to show in the toast.</param>
    private void ShowAndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity =
            unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject =
                    toastClass.CallStatic<AndroidJavaObject>(
                        "makeText", unityActivity, message, 0);
                toastObject.Call("show");
            }));
        }
    }
}
