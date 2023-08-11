using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleCheck : MonoBehaviour
{
    public FaceHandPosSaver script;

    void OnBecameVisible()
    {
        script.visible = true;
    }

    void OnBecameInvisible()
    {
        script.visible = false;
    }
}
