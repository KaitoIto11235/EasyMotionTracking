using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class F_AvatarSpeed : MonoBehaviour
{
    public Animator trail_animator;

    public void T_SpeedChange(SliderEventData eventData)
    {
        trail_animator.SetFloat("S_keisuu", eventData.NewValue);
    }
}
