using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorCameraTransform : MonoBehaviour
{
    [SerializeField] Transform _nAvaWrist;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = -0.2f * _nAvaWrist.position;  // ���肪�ڂ̑O�ɗ������ɂ��f�邽�߂̑΍�
        Vector3 direction = _nAvaWrist.position - this.transform.position;

        this.transform.rotation = Quaternion.LookRotation(direction);
    }
}
