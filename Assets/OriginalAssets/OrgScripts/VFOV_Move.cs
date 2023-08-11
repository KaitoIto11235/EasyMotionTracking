using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFOV_Move : MonoBehaviour
{
    [SerializeField] Transform n_AvatarHand, f_AvatarHand;
    Vector3 arrow;

    // Update is called once per frame
    void Update()
    {
        arrow = 30 * (f_AvatarHand.position - n_AvatarHand.position);
        this.transform.localPosition = arrow;
    }
}
