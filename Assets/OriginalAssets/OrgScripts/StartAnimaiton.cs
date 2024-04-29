using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimaiton : MonoBehaviour
{
    [SerializeField] Animator n_animator, _animator;
    float advTime = 0f; // N_AvatarよりadvTimeだけ早く動く
    float time = 3f; // ボタンを押してからtime+advTime秒後にN_Avatarが動き始める

    /// <summary>
    /// Start!ボタンが押されると呼び出される
    /// </summary>
    public void StartAni()
    {
        StartCoroutine(DelayCoroutine());
    }

    private IEnumerator DelayCoroutine()
    {
        
        yield return new WaitForSeconds(time);
        _animator.SetTrigger("StartTri"); // T_Avatarが動き出す
        yield return new WaitForSeconds(advTime);
        n_animator.SetTrigger("StartTri"); // N_Avatarが動き出す
        Debug.Log("StartAnimation");
    }
}
