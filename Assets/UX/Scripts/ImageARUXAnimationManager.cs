using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;

public class ImageARUXAnimationManager : MonoBehaviour
{
    [SerializeField]
    TMP_Text instructionText;

    [SerializeField]
    VideoClip findImageClip;

    [SerializeField]
    VideoPlayer videoPlayer;

    [SerializeField]
    RawImage rawImage;

    [SerializeField]
    float fadeOnDuration = 0.5f;
    [SerializeField]
    float fadeOffDuration = 0.5f;

    Color whiteAlpha0 = new Color(1, 1, 1, 0);
    Color whiteAlpha1 = new Color(1, 1, 1, 1);
    Color targetColor;

    const string k_FindClipText = "Find an Image to Track";

    [HideInInspector]
    public bool fadeOffComplete;

    [SerializeField]
    Texture transparentTexture;
    RenderTexture m_RenderTexture;


    public bool fadeOnStarted;
    public bool fadeOffStarted;

    void OnEnable()
    {
        m_RenderTexture = videoPlayer.targetTexture;
        m_RenderTexture.DiscardContents();
        m_RenderTexture.Release();
        Graphics.Blit(transparentTexture, m_RenderTexture);
    }

    public void ShowFindImage()
    {
        videoPlayer.clip = findImageClip;
        videoPlayer.Play();
        instructionText.text = k_FindClipText;
        FadeOnUI();
    }

    public void FadeOffUI()
    {
        if (videoPlayer.clip != null)
        {
            fadeOffStarted = true;
            targetColor = whiteAlpha0;
            rawImage.DOColor(targetColor, fadeOffDuration).OnComplete(() => fadeOffComplete = true);
            instructionText.DOColor(targetColor, fadeOffDuration);
        }
    }

    void FadeOnUI()
    {
        if (videoPlayer.clip != null)
        {
            fadeOnStarted = true;
            targetColor = whiteAlpha1;
            rawImage.DOColor(targetColor, fadeOnDuration);
            instructionText.DOColor(targetColor, fadeOnDuration);
        }
    }
}