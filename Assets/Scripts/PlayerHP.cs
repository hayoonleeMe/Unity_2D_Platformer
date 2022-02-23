using System.Collections;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    // 플레이어 오브젝트의 스프라이트 렌더러
    private SpriteRenderer spriteRenderer;
    private PlayerController playerController;

    // 캔버스
    [SerializeField]
    private ManageHeart manageHeart;

    // 플레이어의 최대 체력과 프로퍼티
    [SerializeField]
    private float maxHP;
    public float MaxHP => maxHP;

    // 플레이어의 현재 체력과 프로퍼티
    private float currentHP;
    public float CurrentHP => currentHP;

    // 플레이어가 데미지를 입은 상태인지를 나타내는 상태변수와 프로퍼티
    private bool isHit = false;
    public bool IsHit => isHit;

    // 피격 후 다음 피격이 가능할 때까지 걸리는 시간
    [SerializeField]
    private float hitDelay;

    // 깜빡이는 애니메이션의 깜빡임 딜레이
    private float blinkDelay = 0.1f;

    // HitColorAnimation 의 IEnumerator
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

    // 플레이어를 초기화 시킨다.
    private void InitializeHP()
    {
        currentHP = maxHP;
    }

    // damage 만큼 플레이어의 체력이 하락한다.
    public void TakeDamage(float damage)
    {
        manageHeart.ApplyDamageToHeart(damage);
        currentHP -= damage;

        if (currentHP <= 0)
        {
            OnDie();
        }

        // 해당 메소드를 호출하려면 isHit 이 false 라는 조건이 필요하도록
        // 코딩했기 때문에 HitRoutine() 코루틴은 중복 실행되지 않는다.
        StartCoroutine(HitRoutine());
    }

    // 플레이어가 죽을 때 호출된다.
    private void OnDie()
    {
        Debug.Log("Player is Die");

        // 플레이어를 초기화시킨다.
        playerController.InitializeControl();
        InitializeHP();
        manageHeart.InitializeHeart();
    }

    // 깜빡임 애니메이션을 구현하는 코루틴
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

    // 플레이어가 피격당했을 때 실행되는 코루틴
    private IEnumerator HitRoutine()
    {
        isHit = true;
        StartCoroutine(blinkEffectRoutine);

        yield return new WaitForSeconds(hitDelay);

        isHit = false;
        StopCoroutine(blinkEffectRoutine);
    }
}
