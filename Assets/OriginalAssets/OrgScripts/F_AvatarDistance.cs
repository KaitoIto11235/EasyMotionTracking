using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class F_AvatarDistance : MonoBehaviour
{
    [SerializeField] Animator trail_animator;
    [SerializeField] float MaxDis = 1.0f; // スライダーの値の最大値となる

    public void T_DistanceChange(SliderEventData eventdata)
    {
        // 軌跡を作る透明のアバターがどれだけ先行するかを決定する
        trail_animator.SetFloat("D_trail", eventdata.NewValue * MaxDis);

    }
}
