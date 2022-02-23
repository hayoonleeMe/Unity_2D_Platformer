using System.Collections;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    // �÷��̾� ������Ʈ�� ��������Ʈ ������
    private SpriteRenderer spriteRenderer;
    private PlayerController playerController;

    // ĵ����
    [SerializeField]
    private ManageHeart manageHeart;

    // �÷��̾��� �ִ� ü�°� ������Ƽ
    [SerializeField]
    private float maxHP;
    public float MaxHP => maxHP;

    // �÷��̾��� ���� ü�°� ������Ƽ
    private float currentHP;
    public float CurrentHP => currentHP;

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
        playerController = GetComponent<PlayerController>();

        currentHP = maxHP;
    }

    private void Start()
    {
        blinkEffectRoutine = BlinkEffectRoutine();
    }

    // �÷��̾ �ʱ�ȭ ��Ų��.
    private void InitializeHP()
    {
        currentHP = maxHP;
    }

    // damage ��ŭ �÷��̾��� ü���� �϶��Ѵ�.
    public void TakeDamage(float damage)
    {
        manageHeart.ApplyDamageToHeart(damage);
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

        // �÷��̾ �ʱ�ȭ��Ų��.
        playerController.InitializeControl();
        InitializeHP();
        manageHeart.InitializeHeart();
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
