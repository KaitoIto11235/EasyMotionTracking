using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;

public class VFOV_Dir : MonoBehaviour
{
    Vector3 direction;
    [SerializeField] Transform centralBall;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ���I�u�W�F�N�g���烁�C���J�����̕����̃x�N�g�����擾����
        direction = centralBall.position - CameraCache.Main.transform.position;

        // �I�u�W�F�N�g���x�N�g�������ɏ]���ĉ�]������
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
