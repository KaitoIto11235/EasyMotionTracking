using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;
using System;

public class F_AvatarTransform : MonoBehaviour
{
    [SerializeField] Transform _mAvatar;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 height = new Vector3(0f, -1.56f, 0f);

        Transform eye = CameraCache.Main.transform;
        _mAvatar.position = 5 * eye.forward + height;

        // 第一引数のベクトルを第二引数のベクトルまで回転させるためのQuaternionを作る
        Quaternion eyeRotate = Quaternion.FromToRotation(Vector3.forward, eye.forward);

        _mAvatar.rotation = Quaternion.Inverse(eyeRotate);
    }
}
