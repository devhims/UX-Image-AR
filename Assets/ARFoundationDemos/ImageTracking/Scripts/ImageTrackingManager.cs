using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using UnityEngine.Video;

public class ImageTrackingManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Image manager on the AR Session Origin")]
    ARTrackedImageManager imageManager;

    [Tooltip("Reference Image Library")]
    IReferenceImageLibrary imageLibrary;

    [SerializeField]
    [Tooltip("Prefab for tracked 1 image")]
    GameObject videoSurfacePrefab;

    public List<VideoClip> videoClips = new List<VideoClip>();

    GameObject firstSurface;
    GameObject secondSurface;
    GameObject thirdSurface;

    static Guid firstImageGUID;
    static Guid secondImageGUID;
    static Guid thirdImageGUID;

    void OnEnable()
    {
        imageLibrary = imageManager.referenceLibrary;

        firstImageGUID = imageLibrary[0].guid;
        secondImageGUID = imageLibrary[1].guid;
        thirdImageGUID = imageLibrary[2].guid;

        imageManager.trackedImagesChanged += ImageManagerOnTrackedImagesChanged;
    }

    void OnDisable()
    {
        imageManager.trackedImagesChanged -= ImageManagerOnTrackedImagesChanged;
    }

    void ImageManagerOnTrackedImagesChanged(ARTrackedImagesChangedEventArgs images)
    {
        // added, spawn prefab
        foreach(ARTrackedImage image in images.added)
        {
            if (image.referenceImage.guid == firstImageGUID)
            {
                firstSurface = PrepareSurface(videoSurfacePrefab, image, videoClips[0]);
            }
            else if (image.referenceImage.guid == secondImageGUID)
            {
                secondSurface = PrepareSurface(videoSurfacePrefab, image, videoClips[1]);
            }
            else if (image.referenceImage.guid == thirdImageGUID)
            {
                thirdSurface = PrepareSurface(videoSurfacePrefab, image, videoClips[2]);
            }
        }
        
        // updated, set prefab position and rotation
        foreach(ARTrackedImage image in images.updated)
        {
            // image is tracking or tracking with limited state, show visuals and update it's position and rotation
            if (image.trackingState == TrackingState.Tracking)
            {
                if (image.referenceImage.guid == firstImageGUID)
                {
                    UpdateSurface(firstSurface, image);
                }
                else if (image.referenceImage.guid == secondImageGUID)
                {
                    UpdateSurface(secondSurface, image);
                }
                else if (image.referenceImage.guid == thirdImageGUID)
                {
                    UpdateSurface(thirdSurface, image);
                }
            }
            // image is no longer tracking, disable visuals TrackingState.Limited TrackingState.None
            else
            {
                if (image.referenceImage.guid == firstImageGUID)
                {
                    firstSurface.SetActive(false);
                }
                else if (image.referenceImage.guid == secondImageGUID)
                {
                    secondSurface.SetActive(false);
                }
                else if (image.referenceImage.guid == thirdImageGUID)
                {
                    thirdSurface.SetActive(false);
                }
            }
        }
        
        // removed, destroy spawned instance
        foreach(ARTrackedImage image in images.removed)
        {
            if (image.referenceImage.guid == firstImageGUID)
            {
                Destroy(firstSurface);
            }
            else if (image.referenceImage.guid == firstImageGUID)
            {
                Destroy(secondSurface);
            }
            else if (image.referenceImage.guid == thirdImageGUID)
            {
                Destroy(thirdSurface);
            }
        }
    }

    GameObject PrepareSurface(GameObject prefab, ARTrackedImage image, VideoClip clip)
    {
        GameObject go;
        go = Instantiate(prefab, image.transform.position, image.transform.rotation);
        Vector2 imageSize = image.size;
        imageSize = new Vector2(imageSize.x + imageSize.x * 0.025f, imageSize.y + imageSize.y * 0.025f);
        go.transform.localScale = new Vector3(imageSize.x, 0f, imageSize.y);
        go.GetComponentInChildren<VideoPlayer>().clip = clip;
        go.SetActive(true);
        return go;
    }
      
    void UpdateSurface(GameObject go, ARTrackedImage image)
    {
        go.SetActive(true);
        go.transform.SetPositionAndRotation(image.transform.position, image.transform.rotation);
    }

    public int NumberOfTrackedImages()
    {
        int m_NumberOfTrackedImages = 0;

        foreach (ARTrackedImage image in imageManager.trackables)
        {
            if (image.trackingState == TrackingState.Tracking)
            {
                m_NumberOfTrackedImages++;
            }
        }
        return m_NumberOfTrackedImages;
    }
}
