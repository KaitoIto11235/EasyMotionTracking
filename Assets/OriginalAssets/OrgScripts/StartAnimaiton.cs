using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimaiton : MonoBehaviour
{
    [SerializeField] Animator n_animator, _animator;
    float advTime = 0f; // N_Avatar���advTime������������
    float time = 3f; // �{�^���������Ă���time+advTime�b���N_Avatar�������n�߂�

    /// <summary>
    /// Start!�{�^�����������ƌĂяo�����
    /// </summary>
    public void StartAni()
    {
        StartCoroutine(DelayCoroutine());
    }

    private IEnumerator DelayCoroutine()
    {
        
        yield return new WaitForSeconds(time);
        _animator.SetTrigger("StartTri"); // T_Avatar�������o��
        yield return new WaitForSeconds(advTime);
        n_animator.SetTrigger("StartTri"); // N_Avatar�������o��
        Debug.Log("StartAnimation");
    }
}
