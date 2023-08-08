using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class N_AvatarSpeed : MonoBehaviour
{
    public Animator _animator;

    public void SpeedChange(SliderEventData eventData)
    {
        _animator.SetFloat("S_keisuu", eventData.NewValue);

        // 実験用。 SpeedSliderの値が1になった時、アニメーションが動き出す
        if (eventData.NewValue == 1)
        {
            _animator.SetBool("StartAni", true);
        }
        else
        {
            _animator.SetBool("StartAni", false);
        }
    }
}
