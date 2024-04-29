using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TensionRodTransform : MonoBehaviour
{
    [SerializeField] Transform _nAvaR, _nAvaL;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = _nAvaR.position - this.transform.position;
        Vector3 r = _nAvaR.position;
        Vector3 l = _nAvaL.position;
        float distance = Vector3.Distance(r, l);
        Vector3 scale;
        float thickness = 0.01f * Mathf.Exp(-distance);    

        scale = new Vector3(thickness, thickness, distance);
        this.transform.position = (_nAvaR.position + _nAvaL.position) / 2f;
        this.transform.rotation = Quaternion.LookRotation(direction);
        this.transform.localScale = scale;
    }
}
