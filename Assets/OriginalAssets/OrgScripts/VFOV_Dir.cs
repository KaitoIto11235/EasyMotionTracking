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
        // 現オブジェクトからメインカメラの方向のベクトルを取得する
        direction = centralBall.position - CameraCache.Main.transform.position;

        // オブジェクトをベクトル方向に従って回転させる
        transform.rotation = Quaternion.LookRotation(direction);
    }
}
