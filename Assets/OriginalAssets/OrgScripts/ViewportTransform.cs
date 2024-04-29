using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;
using System;

public class ViewportTransform : MonoBehaviour
{
    [SerializeField] Transform _rightUp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float depth = 1.0f;
        _rightUp.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, depth));
    }
}
