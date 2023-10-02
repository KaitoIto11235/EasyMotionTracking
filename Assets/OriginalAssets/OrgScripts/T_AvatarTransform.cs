using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;
using System;

public class T_AvatarTransform : MonoBehaviour
{
    [SerializeField] Transform _tAvatar, _nAvatarRightHand;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 sholderL = new Vector3(0.18f, -1.3f, 0.05f);

        Transform eye = CameraCache.Main.transform;
        _tAvatar.position = _nAvatarRightHand.position + sholderL;

        // 第一引数のベクトルを第二引数のベクトルまで回転させるためのQuaternionを作る
        Quaternion eyeRotate = Quaternion.FromToRotation(Vector3.forward, _nAvatarRightHand.position - eye.position);

        _tAvatar.rotation = Quaternion.Inverse(eyeRotate);
    }
}
