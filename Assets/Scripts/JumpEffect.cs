using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpEffect : MonoBehaviour
{
    private Animator animator;

    // 실행할 클립
    [SerializeField]
    private AnimationClip clip;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // 점프 이펙트 애니메이션 클립을 실행한다.
    public void PlayEffect()
    {
        animator.Play(clip.name);
    }

    // 애니메이션 클립이 종료되면 Event로 실행되어 추후에도 사용하기 위해 점프 이펙트를 비활성화한다.
    private void OnAnimationEnd()
    {
        gameObject.SetActive(false);
    }
}
