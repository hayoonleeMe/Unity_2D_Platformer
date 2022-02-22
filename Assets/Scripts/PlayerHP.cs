using System.Collections;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    // �÷��̾� ������Ʈ�� ��������Ʈ ������
    private SpriteRenderer spriteRenderer;

    // ĵ����
    [SerializeField]
    private ManageHeart manageHeart;

    // �÷��̾��� �ִ� ü�°� ������Ƽ
    [SerializeField]
    private float maxHP;
    public float MaxHP => maxHP;

    // �÷��̾��� ���� ü�°� ������Ƽ
    private float currentHP;
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

    // �����̴� �ִϸ��̼��� ������ ������
    private float blinkDelay = 0.1f;

    // HitColorAnimation �� IEnumerator
    IEnumerator blinkEffectRoutine;

    private void Awake()
    { 
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHP = maxHP;
    }

    private void Start()
    {
        blinkEffectRoutine = BlinkEffectRoutine();
    }

    // damage ��ŭ �÷��̾��� ü���� �϶��Ѵ�.
    public void TakeDamage(float damage)
    {
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

    // �÷��̾ ���� �� ȣ��ȴ�.
    private void OnDie()
    {
        Debug.Log("Player is Die");
    }

    // ������ �ִϸ��̼��� �����ϴ� �ڷ�ƾ
    private IEnumerator BlinkEffectRoutine()
    {
        Color color = spriteRenderer.color;

        while (true)
        {
            color.a = 0.3f;
            spriteRenderer.color = color;

            yield return new WaitForSeconds(blinkDelay);

            color.a = 1.0f;
            spriteRenderer.color = color; 

            yield return new WaitForSeconds(blinkDelay);
        }
    }

    // �÷��̾ �ǰݴ����� �� ����Ǵ� �ڷ�ƾ
    private IEnumerator HitRoutine()
    {
        isHit = true;
        StartCoroutine(blinkEffectRoutine);

        yield return new WaitForSeconds(hitDelay);

        isHit = false;
        StopCoroutine(blinkEffectRoutine);
    }
}
