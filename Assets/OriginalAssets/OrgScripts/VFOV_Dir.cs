using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;

public class VFOV_Dir : MonoBehaviour
{
    void Update()
    {
        // �I�u�W�F�N�g���x�N�g�������ɏ]���ĉ�]������
        transform.rotation = Quaternion.LookRotation(CameraCache.Main.transform.forward);
    }
}
