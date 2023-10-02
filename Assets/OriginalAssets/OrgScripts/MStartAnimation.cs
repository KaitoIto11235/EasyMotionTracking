using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MStartAnimation : MonoBehaviour
{
    [SerializeField] Animator n_animator;

    /// <summary>
    /// Start!�{�^�����������ƌĂяo�����
    /// </summary>
    public void StartAni()
    {
        StartCoroutine(DelayCoroutine());
    }

    private IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(3);
        n_animator.SetTrigger("StartTri"); // N_Avatar�������o��
        Debug.Log("StartAnimation");
    }
}
