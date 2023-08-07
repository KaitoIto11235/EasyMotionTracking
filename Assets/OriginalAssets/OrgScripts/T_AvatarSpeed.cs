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

        // �A�j���[�V�������x�ɂ��O�Ղ̎������Ԃ̕ύX
        timeOnSpeed = script.timeOnDis / trail_animator.GetFloat("S_keisuu");

        script.handR.time = timeOnSpeed;
    }
}
