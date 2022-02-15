using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    // 플레이어 오브젝트의 스프라이트 렌더러
    private SpriteRenderer spriteRenderer;

    // 캔버스
    [SerializeField]
    private ManageHeart manageHeart;

    // 플레이어의 최대 체력
    [SerializeField]
    private float maxHP = 3.0f;

    // maxHP 의 프로퍼티 (only get)
    public float MaxHP => maxHP;

    // 플레이어의 현재 체력
    private float currentHP;

    // currentHP 의 프로퍼티 (set, get)
    public float CurrentHP
    {
        set => currentHP = Mathf.Clamp(value, 0, maxHP);
        get => currentHP;
    }

    // 플레이어가 데미지를 입은 상태인지를 나타내는 상태변수와 프로퍼티
    private bool isHit = false;
    public bool IsHit => isHit;

    // 피격 후 다음 피격이 가능할 때까지 걸리는 시간
    [SerializeField]
    private float hitDelay;

    private float BlinkDelay = 0.1f;

    IEnumerator hitColorAnimation;

    private void Awake()
    { 
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHP = maxHP;
    }

    private void Start()
    {
        hitColorAnimation = HitColorAnimation();
    }

    // damage 만큼 플레이어의 체력이 하락한다.
    public void TakeDamage(float damage)
    {
        //StopCoroutine(HitColorAnimation());
       //StartCoroutine(HitColorAnimation());

        manageHeart.ApplyHeart(damage);
        currentHP -= damage;

        if (currentHP <= 0)
        {
            OnDie();
        }

        // 해당 메소드를 호출하려면 isHit 이 false 라는 조건이 필요하도록
        // 코딩했기 때문에 HitRoutine() 코루틴은 중복 실행되지 않는다.
        StartCoroutine(HitRoutine());
    }

    private void OnDie()
    {
        Debug.Log("Player is Die");
    }

    private IEnumerator HitColorAnimation()
    {
        Color color = spriteRenderer.color;

        while (true)
        {
            color.a = 0.3f;
            spriteRenderer.color = color;

            yield return new WaitForSeconds(BlinkDelay);

            color.a = 1.0f;
            spriteRenderer.color = color; 

            yield return new WaitForSeconds(BlinkDelay);
        }
    }

    private IEnumerator HitRoutine()
    {
        isHit = true;
        StartCoroutine(hitColorAnimation);

        yield return new WaitForSeconds(hitDelay);

        isHit = false;
        StopCoroutine(hitColorAnimation);
    }

}
