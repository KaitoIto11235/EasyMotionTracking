using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimaiton : MonoBehaviour
{
    [SerializeField] Animator n_animator, t_animator;

    public void StartAni()
    {
        StartCoroutine(DelayCoroutine());
    }

    private IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(3);
        Debug.Log("StartAnimation");
        n_animator.SetTrigger("StartTri");
        t_animator.SetTrigger("StartTri");
    }
}
