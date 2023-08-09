using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimaiton : MonoBehaviour
{
    [SerializeField] Animator n_animator, _animator;
    [SerializeField] float advTime = 0.3f; // N_Avatar‚æ‚èadvTime‚¾‚¯‘‚­“®‚­
    float time = 3f; // ƒ{ƒ^ƒ“‚ğ‰Ÿ‚µ‚Ä‚©‚çtime•bŒã‚ÉN_Avatar‚ª“®‚«n‚ß‚é

    public void StartAni()
    {
        StartCoroutine(DelayCoroutine());
    }

    private IEnumerator DelayCoroutine()
    {
        time -= advTime;
        yield return new WaitForSeconds(time);
        _animator.SetTrigger("StartTri");


        yield return new WaitForSeconds(advTime);
        Debug.Log("StartAnimation");
        n_animator.SetTrigger("StartTri");
    }
}
