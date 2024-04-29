using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;
using System;

public class T_AvatarTransform : MonoBehaviour
{
    [SerializeField] Transform _nAvaWristR, _nAvaWristL;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 basePos = new Vector3(0f, -1.56f, -0.08f);
        Vector3 targetPos; // tAvaWristL�̈ʒu�x�N�g��
        Vector3 shiftDir; // tAva�̈ʒu�V�t�g�̕����x�N�g��

        targetPos = (_nAvaWristL.position - _nAvaWristR.position).normalized * 0.1f + _nAvaWristR.position; // tAva�̍�������̈ʒu�ɂ�����
        shiftDir = targetPos - _nAvaWristL.position;
        this.transform.position = basePos + shiftDir;
    }
}
