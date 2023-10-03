using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;

public class MirrorTransform : MonoBehaviour
{
    [SerializeField] Transform _nAvaWristR;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Transform eye = CameraCache.Main.transform;
        this.transform.position = 2f * (_nAvaWristR.position - eye.position);
        Vector3 direction = eye.position - this.transform.position;
        this.transform.rotation = Quaternion.LookRotation(-1.0f * direction);
    }
}
