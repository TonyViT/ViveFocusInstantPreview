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

//Code from https://answers.unity.com/questions/314049/how-to-make-a-plane-fill-the-field-of-view.html

/// <summary>
/// Adapts the quad dimension so that it fills completely the Camera field of view
/// </summary>
public class AdaptQuadToCamera : MonoBehaviour
{
    /// <summary>
    /// The camera this quad has to adapt to
    /// </summary>    
    public Camera m_camera;

    private void Update()
    {
        //we don't put this code in start because at the beginning the cameras may have a different format from the final one
        float pos = (m_camera.nearClipPlane + 0.01f);
        transform.position = m_camera.transform.position + m_camera.transform.forward * pos;
        float h = Mathf.Tan(m_camera.fieldOfView  * Mathf.Deg2Rad * 0.5f) * pos * 2f;
        transform.localScale = new Vector3(h * m_camera.aspect, h, 0f);
    }
}