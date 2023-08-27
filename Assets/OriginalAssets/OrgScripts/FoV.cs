using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;


public class FoV : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Transform face = CameraCache.Main.transform; // ÉJÉÅÉâÇÃTransform
        this.transform.position = face.forward + face.position;
        this.transform.rotation = face.rotation;
    }
}
