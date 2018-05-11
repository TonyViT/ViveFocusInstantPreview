# ViveFocusInstantPreview
An experiment to create an instant preview feature for the Vive Focus device.

With this solution you will be able to hit Play on your Unity scene on your PC, while wearing the Vive Focus, to directly preview the content of the scene on your device, without deploying anything.

The solution is experimental, not finished and misses a lot of features, but it is a good start if you plan implementing it by yourself.

## Getting Started

The solution is comprised of two parts: a standalone application that runs on the Vive Focus and lets you see the preview and a script to be attached to the WaveVR prefab of your Unity scene. 
The two elements will communicate so that the rototranslational data of the Focus will be sent to Unity, while the scene rendered by Unity will be sent to the device.

### Use the preview app
If you don't plan to modify the standalone app, using it is pretty simple:
* Get the APK from the Bin folder of this project
* Connect your Vive Focus to the USB port of your PC
* Install the app on your Vive Focus device
* Launch it from your Vive Focus. You should now see a scene with an arrow... this means that the app is ready and it is waiting for you to stream something for Unity
* Now on your PC Open your Vive Wave Unity project that you want to preview
* Go to Edit -> Project Settings -> Editor
* Change the Unity Remote settings: Device -> Any Android Device; Compression -> PNG; Resolution: Downsize
* Open the Unity scene that you want to preview
* Copy the InstantPreviewWaveVR.cs script that you can find in the Assets\FocusInstantPreview\Scripts folder of this repository into your project
* Attach the InstantPreviewWaveVR to the WaveVR prefab of the scene
* Change the size of the Game preview window in Unity to one compatible to the Vive Focus resolution: 2880x1600 or 1440x800. You do this by using the menu on the upper-left corner of the Game window in Unity
* Hit Play: you should see the content being previewed in the Focus!

### Build the preview app
If you want to build the preview app:
* Download the repo and open the project with Unity
* Import the Unity Remote 4 project from the Asset Store: [link](https://assetstore.unity.com/packages/templates/unity-remote-4-18106)
* Import the WaveVR Unity Plugin: you can download WaveVR SDK from this [link](https://hub.vive.com/en-US/profile/material-download)
* Open the file Assets\FocusInstantPreview\Scripts\UnityRemoteFilesModifications and apply the modifications specified there to the files of Unity Remote 4 project. I can't push the modified files
  in this repo because they're under Unity Asset Store license. You have just to substitute the code of two methods
* Hit Build and Run to build the app and execute it into your Focus

### Understanding the code
The project is split in various folders whose names are self-explanatory. Its code is widely commented.

The main scenes are: InstantPreviewApp, that is the scene of the preview app for the device and SampleUnityScene, that is a sample scene that you can test on Unity hitting Play to see the content
being streamed to the device.

The app running on the device is the complicated part of this project. Inside the scene you can see that there are 5 objects:
* UnityRemote4Code: the code coming from Unity Remote 4, that lets the program connect to Unity to exchange data with the game engine
* InstantPreview: features the behaviours necessary to get the pose of the device from Vive Wave and to show on the screen the content sent by Unity
* [WaveVR]: useful to init the WaveVR platform
* Cameras: contains two cameras. One represents the left eye and the other one the right eye. Every camera will render the content shown in PreviewTextureQuads on a render texture and then these render textures
  will be shown on the screen of the device, one next to the other to fill the whole screen
* PreviewTexturesQuads: quads for the left and right eye: every quad will show the part of the preview sent by Unity relative to one of the two eyes. This content will be rendered by the cameras and then shown
  on the screen. Every quad will cover completely the view area of the related camera.

## Known issues
This is really a pre-alpha project and has lots of problems:
* It doesn't work over Wi-fi, but only over USB
* The framerate is far from optimal... this app is a puking machine
* The images shown in the device are quite distorted
* The controller is currently not emulated (even if adding it is quite trivial, copying the code of the headset)
* The app requires you to resize the Game window and is tailored on the values of the Vive Focus device

## Authors

* **Antony Vitillo (Skarredghost)** - [Blog](http://skarredghost.com)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Acknowledgments

I hope you'll enjoy it :)