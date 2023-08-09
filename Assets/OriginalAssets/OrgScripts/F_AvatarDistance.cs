using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class F_AvatarDistance : MonoBehaviour
{
    [SerializeField] Animator trail_animator;
    [SerializeField] float MaxDis = 1.0f; // �X���C�_�[�̒l�̍ő�l�ƂȂ�

    public void T_DistanceChange(SliderEventData eventdata)
    {
        // �O�Ղ���铧���̃A�o�^�[���ǂꂾ����s���邩�����肷��
        trail_animator.SetFloat("D_trail", eventdata.NewValue * MaxDis);

    }
}
