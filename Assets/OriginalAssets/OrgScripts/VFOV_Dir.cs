using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;

public class VFOV_Dir : MonoBehaviour
{
    void Update()
    {
        // オブジェクトをベクトル方向に従って回転させる
        transform.rotation = Quaternion.LookRotation(CameraCache.Main.transform.forward);
    }
}
