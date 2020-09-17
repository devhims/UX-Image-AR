using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageUIManager : MonoBehaviour
{
    [SerializeField] ARTrackedImageManager imageManager;
    [SerializeField] ImageARUXAnimationManager animationManager;

    bool prepared;

    void OnEnable()
    {
        ARSession.stateChanged += ShowFindImageUI;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        imageManager.trackedImagesChanged += ImageManagerTrackedImagesChanged;
    }

    void ImageManagerTrackedImagesChanged(ARTrackedImagesChangedEventArgs images)
    {
        if (images.updated.Count > 0)
        {
            int count = 0;

            foreach (var image in images.updated)
            {
                if (image.trackingState != TrackingState.Tracking)
                {
                    count++;
                }
            }

            if (count >= imageManager.trackables.count)
            {
                animationManager.ShowFindImage();
            }
            else
            {
                animationManager.FadeOffUI();
            }
        }
    }

    void ShowFindImageUI(ARSessionStateChangedEventArgs args)
    {
        if (args.state == ARSessionState.SessionTracking && !ImageFound())
        {
            // Show scan UI
            animationManager.ShowFindImage();
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Debug.Log("called" + Screen.sleepTimeout);
        }
    }

    void Update()
    {
        // Proceed only when the session is in tracking state
        if (ARSession.state != ARSessionState.SessionTracking)
        {
            return;
        }

        if (ImageFound() && !prepared)
        {
            // Hide scan UI
            animationManager.FadeOffUI();
            prepared = true;
        }
    }


    bool ImageFound()
    {
        return imageManager?.trackables.count > 0;
    }
    
}
