using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using UnityEngine.Video;

[Serializable]
public class Playables
{
    public VideoClip videoClip;
    [HideInInspector]
    public GameObject surface;
    [HideInInspector]
    public Guid imageID;
}

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

    public Playables[] playables;

    void OnEnable()
    {
        imageLibrary = imageManager.referenceLibrary;

        if (playables.Length != imageLibrary.count)
        {
            Debug.LogError("Length of image library and playables must match");
            return;
        }

        for (int i = 0; i < imageLibrary.count; i++)
        {
            playables[i].imageID = imageLibrary[i].guid;
        }

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
            foreach (var playable in playables)
            {
                if (playable.imageID == image.referenceImage.guid)
                {
                    playable.surface = PrepareSurface(videoSurfacePrefab, image, playable.videoClip);
                }
            }
        }
        
        // updated, set prefab position and rotation
        foreach(ARTrackedImage image in images.updated)
        {
            // image is tracking or tracking with limited state, show visuals and update it's position and rotation
            if (image.trackingState == TrackingState.Tracking)
            {
                foreach (var playable in playables)
                {
                    if (playable.imageID == image.referenceImage.guid)
                    {
                        UpdateSurface(playable.surface, image);
                    }
                }
            }
            // image is no longer tracking, disable visuals TrackingState.Limited TrackingState.None
            else
            {
                foreach (var playable in playables)
                {
                    if (playable.imageID == image.referenceImage.guid)
                    {
                        playable.surface.SetActive(false);
                    }
                }
            }
        }
        
        // removed, destroy spawned instance
        foreach(ARTrackedImage image in images.removed)
        {
            foreach (var playable in playables)
            {
                if (playable.imageID == image.referenceImage.guid)
                {
                    Destroy(playable.surface);
                }
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
