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
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//This files hold modifications that have been necessary for the Unity Remote 4 source files.
//I can't publish the whole modified files because original ones are under Unity license

/// <summary>
/// Modifications needed to the DataSender.cs file. You have only to change method SendGyroscopeInput
/// </summary>
public class DataSenderModification
{

    PacketWriter writer;
    Stream stream;

    public void SendGyroscopeInput()
    {       
        Gyroscope gyro = Input.gyro;
        writer.BeginMessage(RemoteMessage.GyroInput);
        writer.Write(gyro.rotationRate.x);
        writer.Write(gyro.rotationRate.y);
        writer.Write(gyro.rotationRate.z);
        writer.Write(gyro.rotationRateUnbiased.x);
        writer.Write(gyro.rotationRateUnbiased.y);
        writer.Write(gyro.rotationRateUnbiased.z);
        writer.Write(gyro.gravity.x);
        writer.Write(gyro.gravity.y);
        writer.Write(gyro.gravity.z);

        //substitute these useless data with the ones obtained by the WaveVR system
        //writer.Write(gyro.userAcceleration.x);
        //writer.Write(gyro.userAcceleration.y);
        //writer.Write(gyro.userAcceleration.z);
        //writer.Write(gyro.attitude.x);
        //writer.Write(gyro.attitude.y);
        //writer.Write(gyro.attitude.z);
        //writer.Write(gyro.attitude.w);
        writer.Write(InstantPreviewer.instance.transform.localPosition.x);
        writer.Write(InstantPreviewer.instance.transform.localPosition.y);
        writer.Write(InstantPreviewer.instance.transform.localPosition.z);
        writer.Write(InstantPreviewer.instance.transform.localRotation.x);
        writer.Write(InstantPreviewer.instance.transform.localRotation.y);
        writer.Write(InstantPreviewer.instance.transform.localRotation.z);
        writer.Write(InstantPreviewer.instance.transform.localRotation.w);
        writer.EndMessage(stream);
    }
}

/// <summary>
/// Modifications needed to the ScreenStream.cs file. You have only to change method OnGUI;
/// </summary>
public class ScreenStreamModification: MonoBehaviour
{
    bool synced;
    Texture2D screen;

    void OnGUI()
    {
        if (!synced)
        {
            if (SystemInfo.supportsGyroscope)
                Input.gyro.enabled = false;
        }

        if (synced && (screen != null))
        {
            //new data has arrived from Unity, replace the content of the preview quads in the scene
            InstantPreviewer.instance.RenderingQuads[0].sharedMaterial.mainTexture = screen;
            InstantPreviewer.instance.RenderingQuads[1].sharedMaterial.mainTexture = screen;
        }
    }
}
