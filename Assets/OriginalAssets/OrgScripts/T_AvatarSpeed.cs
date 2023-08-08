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

        // �����p�B SpeedSlider�̒l��1�ɂȂ������A�A�j���[�V�����������o��
        if (eventData.NewValue == 1)
        {
            trail_animator.SetBool("StartAni", true);
        }
        else
        {
            trail_animator.SetBool("StartAni", false);
        }

        // �A�j���[�V�������x�ɂ��O�Ղ̎������Ԃ̕ύX
        timeOnSpeed = script.timeOnDis / trail_animator.GetFloat("S_keisuu");

        script.handR.time = timeOnSpeed;
    }
}
