using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ImageARUXAnimationManager))]
public class ImageUIManager : MonoBehaviour
{ 
    ImageARUXAnimationManager animationManager;
    ARTrackedImageManager imageManager;

    bool _sessionTracking;
    int totalTargetsFound = 0;
    bool showScanUI = true;

    void Awake()
    {
        imageManager = FindObjectOfType<ARTrackedImageManager>();
        animationManager = GetComponent<ImageARUXAnimationManager>();
    }

    void OnEnable()
    {
        ARSession.stateChanged += OnSessionStateChange;
        imageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnSessionStateChange(ARSessionStateChangedEventArgs args)
    {
        _sessionTracking = args.state == ARSessionState.SessionTracking ? true : false;

        if (_sessionTracking && imageManager.referenceLibrary.count > 0)
        {
            animationManager.ShowFindImage();
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
        else
        {
            animationManager.FadeOffUI();
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
        }
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        //Debug.Log("updated images:" + args.updated.Count);
        //Debug.Log("trackeables: " + imageManager.trackables.count);

        totalTargetsFound = args.updated.Count;

        if (totalTargetsFound > 0 && _sessionTracking)
        {
            int currentlyTrackedTargets = 0;

            foreach (var image in args.updated)
            {
                if (image.trackingState == TrackingState.Tracking)
                {
                    currentlyTrackedTargets++;
                }
            }

            ShowHideScanAnim(currentlyTrackedTargets);
        }
    }

    void ShowHideScanAnim(int currentTargets)
    {
        if (currentTargets > 0)
        {
            if (showScanUI)
            {
                animationManager.FadeOffUI();
                showScanUI = false;
            }
        }
        else
        {
            if (!showScanUI)
            {
                animationManager.ShowFindImage();
                showScanUI = true;
            }
        }
    }
    
}
