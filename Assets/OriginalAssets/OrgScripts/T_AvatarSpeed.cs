using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class T_AvatarSpeed : MonoBehaviour
{
    public Animator trail_animator;
    public T_AvatarDistance script;
    float timeOnSpeed;

    public void T_SpeedChange(SliderEventData eventData)
    {
        trail_animator.SetFloat("S_keisuu", eventData.NewValue);

        // 実験用。 SpeedSliderの値が1になった時、アニメーションが動き出す
        if (eventData.NewValue == 1)
        {
            trail_animator.SetBool("StartAni", true);
        }
        else
        {
            trail_animator.SetBool("StartAni", false);
        }

        // アニメーション速度による軌跡の持続時間の変更
        timeOnSpeed = script.timeOnDis / trail_animator.GetFloat("S_keisuu");

        script.handR.time = timeOnSpeed;
    }
}
