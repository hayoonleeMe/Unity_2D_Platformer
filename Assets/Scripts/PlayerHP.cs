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

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHP = maxHP;
    }

    // damage 만큼 플레이어의 체력이 하락한다.
    public void TakeDamage(float damage)
    {
        StopCoroutine(HitColorAnimation());
        StartCoroutine(HitColorAnimation());

        manageHeart.ApplyHeart(damage);
        currentHP -= damage;

        if (currentHP <= 0)
        {
            OnDie();
        }
    }

    private void OnDie()
    {
        Debug.Log("Player is Die");
    }

    private IEnumerator HitColorAnimation()
    {
        Color temp = spriteRenderer.color;
        temp.a = 0.3f;
        spriteRenderer.color = temp;

        yield return new WaitForSeconds(0.1f);

        temp.a = 1.0f;
        spriteRenderer.color = temp;
    }

}
