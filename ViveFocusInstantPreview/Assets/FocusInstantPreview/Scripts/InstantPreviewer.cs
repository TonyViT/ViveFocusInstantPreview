//MIT License

//Copyright(c) 2018 Antony Vitillo(a.k.a. "Skarredghost")

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System.Collections;
using UnityEngine;
using WaveVR_Log;
using wvr;

/// <summary>
/// Manages the preview app, getting roto-translation pose of the device and streaming it to the PC
/// and showing the preview screen sent from the PC on the screeen of the focus
/// </summary>
public class InstantPreviewer : MonoBehaviour
{
    /// <summary>
    /// Singleton instance
    /// </summary>
    public static InstantPreviewer instance = null;

    /// <summary>
    /// Cameras that render the left and right previews to be shown on the screen of the device
    /// </summary>
    [SerializeField]
    private Camera[] m_renderingCameras;

    /// <summary>
    /// These quads will show half of the Game preview sent by Unity,
    /// will be framed by the cameras and this will make the preview to be shown
    /// </summary>
    [SerializeField]
    private Renderer[] m_renderingQuads;
    
    /// <summary>
    /// Render textures: the two cameras will render here the scene framed by them
    /// </summary>
    private RenderTexture[] currentRt;

    /// <summary>
    /// Gets the rendering quads
    /// </summary>
    public Renderer[] RenderingQuads
    {
        get
        {
            return m_renderingQuads;
        }
    }

    private void Awake()
    {
        //singleton implementation

        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;

            //init the VR app

#if !UNITY_EDITOR
            //Init the HMD. Without this, the WaveVR system doesn't get data from its sensors. 
            WaveVR_Utils.IssueEngineEvent(WaveVR_Utils.EngineEventID.HMD_INITIAILZED);
#endif

            //init the app
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Application.targetFrameRate = 90; //Focus is actually 75, but with 90 things seem a little better to me :)
        }
        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a InstantPreviewer.
            Destroy(gameObject);

        
    }
    // Use this for initialization
    private void Start()
    {
        //Initialize render textures. We hardcode data for the Vive Focus
        currentRt = new RenderTexture[2];

        for (int i = 0; i < 2; i++)
        {
            currentRt[i] = new RenderTexture(1440, 1600, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
            currentRt[i].useMipMap = false;
            currentRt[i].wrapMode = TextureWrapMode.Clamp;
            currentRt[i].filterMode = FilterMode.Bilinear;
            currentRt[i].anisoLevel = 1;
            currentRt[i].antiAliasing = 1;
            currentRt[i].Create();
            m_renderingCameras[i].targetTexture = currentRt[i];
        }        
    }

    private void OnApplicationQuit()
    {
        WaveVR_Utils.IssueEngineEvent(WaveVR_Utils.EngineEventID.UNITY_APPLICATION_QUIT);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            WaveVR_Utils.IssueEngineEvent(WaveVR_Utils.EngineEventID.UNITY_APPLICATION_PAUSE);
            StopCoroutine("MainLoop");
        }
        else
        {
            WaveVR_Utils.IssueEngineEvent(WaveVR_Utils.EngineEventID.UNITY_APPLICATION_RESUME);
            StartCoroutine("MainLoop");
        }
           
    }

    private void OnEnable()
    {
#if !UNITY_EDITOR
        WaveVR_Utils.IssueEngineEvent(WaveVR_Utils.EngineEventID.UNITY_ENABLE);
#endif
        StartCoroutine("MainLoop");
    }

    private void OnDisable()
    {
#if !UNITY_EDITOR
        WaveVR_Utils.IssueEngineEvent(WaveVR_Utils.EngineEventID.UNITY_DISABLE);
#endif
        StopCoroutine("MainLoop");
    }

    /// <summary>
    /// Main Loop: here we get the rototranslational data of the device (that will be streamed to Unity)
    /// And will show the preview on the device screen (streamed from Unity)
    /// </summary>
    /// <returns></returns>
    private IEnumerator MainLoop()
    {
#if !UNITY_EDITOR

        yield return 1;

        //we're about to render the first frame. This is necessary to make the app wake-up correctly if you remove the headset and put it on again
        WaveVR_Utils.IssueEngineEvent(WaveVR_Utils.EngineEventID.FIRST_FRAME);

        //loop forever
        while (true)
        {
            //Update the position of the device
            WaveVR.Instance.UpdatePoses(WVR_PoseOriginModel.WVR_PoseOriginModel_OriginOnGround);

            //wait the end of frame, so we can play a bit with textures
            yield return new WaitForEndOfFrame();

            //for each eye (0 = left, 1 = right)
            for (int i = 0; i < 2; i++)
            {
                //notify WaveVR that we want to show the content of the render texture associated with one of the two cameras of the scene.
                //Each camera in the scene has in front of it a big quad, big as its near plane, with half of the texture of the Game Area sent by Unity.
                //This means that the left camera will frame the left part of the screen sent by Unity, and the right camera the right part.
                //Every camera will render this onto a RenderTexture that we'll now send to the ViveWave system, that will draw them onto the screen.
                //Basically we're taking the screen sent by Unity, we're splitting it into half and we're rendering it onto the screen of the Vive Focus device
                WaveVR_Utils.SetRenderTexture(currentRt[i].GetNativeTexturePtr());
                WaveVR_Utils.SendRenderEventNative(i == 0 ? WaveVR_Utils.k_nRenderEventID_SubmitL : WaveVR_Utils.k_nRenderEventID_SubmitR);
                WaveVR_Utils.SendRenderEventNative(i == 0 ? WaveVR_Utils.k_nRenderEventID_RenderEyeEndL : WaveVR_Utils.k_nRenderEventID_RenderEyeEndR);
            }
        }
#else
        yield break;
#endif
    }
   
}
