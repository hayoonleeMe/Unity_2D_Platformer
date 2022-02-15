using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    // �÷��̾� ������Ʈ�� ��������Ʈ ������
    private SpriteRenderer spriteRenderer;

    // ĵ����
    [SerializeField]
    private ManageHeart manageHeart;

    // �÷��̾��� �ִ� ü��
    [SerializeField]
    private float maxHP = 3.0f;

    // maxHP �� ������Ƽ (only get)
    public float MaxHP => maxHP;

    // �÷��̾��� ���� ü��
    private float currentHP;

    // currentHP �� ������Ƽ (set, get)
    public float CurrentHP
    {
        set => currentHP = Mathf.Clamp(value, 0, maxHP);
        get => currentHP;
    }

    // �÷��̾ �������� ���� ���������� ��Ÿ���� ���º����� ������Ƽ
    private bool isHit = false;
    public bool IsHit => isHit;

    // �ǰ� �� ���� �ǰ��� ������ ������ �ɸ��� �ð�
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

    // damage ��ŭ �÷��̾��� ü���� �϶��Ѵ�.
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

        // �ش� �޼ҵ带 ȣ���Ϸ��� isHit �� false ��� ������ �ʿ��ϵ���
        // �ڵ��߱� ������ HitRoutine() �ڷ�ƾ�� �ߺ� ������� �ʴ´�.
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
