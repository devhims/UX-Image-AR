# UX-Image-AR
UX implementation for image tracking based AR experiences (Unity AR Foundation 2019.4+). Based on [Unity's AR Foundation Demos - UX Onboarding asset](https://assetstore.unity.com/packages/templates/ar-foundation-demos-onboarding-ux-164766).

## Demo
YouTube [Link](https://youtu.be/lKs1xv_yfDQ)

## How to build?
1. Make sure you're using **Unity 2019.4** and above.
2. Clone/Download and open the project in the specified Unity version.
3. Change the build platform to Android/iOS
4. Settings for Android are already in place, just do a build and run. Check the AR Foundation specific platform settings for iOS.

**Note:** Go to Assets/Scenes/UXImages for the scene setup. 

## Features:
1. UI Feedback on AR session states implemented. 
2. Muliple simultaneous image target tracking enabled.
3. Size of the video surface game object can be adjusted at runtime. 
4. Scan image UI activated and deactivated depending on the session and image tracking mode at any given instant.
5. `ImageTrackingManager.cs` can be extended to support even more than 3 image targets.  

## Other Assets Used:
[DOTween (HOTween v2)](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676)
