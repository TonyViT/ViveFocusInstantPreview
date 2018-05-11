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

using UnityEngine;

/// <summary>
/// Attach this object to a WaveVR prefab to have instant preview on your Unity scene.
/// DON'T FORGET TO ENABLE UNITY REMOTE IN EDITOR SETTINGS AND TO LAUNCH THE PREVIEW APP ON THE FOCUS FOR THIS TO WORK
/// </summary>
public class InstantPreviewWaveVR : MonoBehaviour
{
    /// <summary>
    /// Data from gyroscope... actually used to get the data from the Wave app running on the device
    /// </summary>
    private Gyroscope gyro;

    private void Start()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
        }
    }

    private void LateUpdate()
    {        
        //get pose of the actual device and apply it to the associated WaveVR prefab
        if (gyro != null)
        {
            transform.localRotation = gyro.attitude;
            transform.localPosition = gyro.userAcceleration;
        }
    }

}