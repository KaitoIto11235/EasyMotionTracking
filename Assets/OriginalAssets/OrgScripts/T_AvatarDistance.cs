using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.UI;

public class T_AvatarDistance : MonoBehaviour
{
    [SerializeField] Animator trail_animator;
    [SerializeField] float MaxDis = 1.0f; // �X���C�_�[�̒l�̍ő�l�ƂȂ�
    public TrailRenderer handR;
    public float timeOnDis;

    public void T_DistanceChange(SliderEventData eventdata)
    {
        // �O�Ղ���铧���̃A�o�^�[���ǂꂾ����s���邩�����肷��
        trail_animator.SetFloat("D_trail", eventdata.NewValue * MaxDis);

        // �����̕ω��ɂ��O�Ղ̒����̕ύX
        // ���A�j���[�V������2�b�ň�����邽�߁A2��������
        timeOnDis = eventdata.NewValue * MaxDis * 2f / trail_animator.GetFloat("S_keisuu");

        // BhandR.time�͐g�̕���B-hand_R�̋O�Ղ̎�������
        handR.time = timeOnDis;
    }
}
