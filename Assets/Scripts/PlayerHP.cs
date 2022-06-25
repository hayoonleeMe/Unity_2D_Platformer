using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Environment;

public class PlayerHP : MonoBehaviour
{
    // PlayerHurt Animation clip
    [SerializeField]
    private AnimationClip playerHurt;

    // 다음 씬의 이름
    [SerializeField]
    private string nextSceneName;

    // scoreManager의 ManageScore 스크립트
    [SerializeField]
    private ManageScore manageScore;

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

    // damage 만큼 플레이어의 체력이 하락한다.
    public void TakeDamage(float damage)
    {
        // 현재 플레이어의 남은 체력보다 데미지가 더 크다면
        if (currentHP - damage < 0)
        {
            damage = currentHP;
        }

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
        #region For Test - 플레이어를 초기화시킨다.
        //Debug.Log("Player is Die");
        //playerController.InitializeControl();
        //InitializeHP();
        //manageHeart.InitializeHeart();
        #endregion

        // GameOver Scene에서 나타낼 Score를 저장한다.
        PlayerPrefs.SetInt("score", manageScore.getScore());

        // 플레이어 사망 연출을 실행하고 GameOver Scene으로 이동한다.
        StartCoroutine(DelayedDeadAnimationCoroutine());
    }

    // 시간을 조절하여 플레이어 사망 연출을 나타내는 코루틴
    private IEnumerator DelayedDeadAnimationCoroutine()
    {
        // 플레이어의 이동, 점프를 방지한다.
        playerController.RestrictMove();

        Time.timeScale = 0.01f;
        yield return new WaitForSeconds(0.005f);

        Time.timeScale = 0.2f;
        yield return new WaitForSeconds(playerHurt.length * 0.4f * 0.2f);

        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(playerHurt.length * 0.4f * 0.5f);

        Time.timeScale = 1.0f;

        // GameOver Scene으로 이동한다.
        SceneManager.LoadScene(nextSceneName);
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

    #region For Debugging - private void InitializeHP() 플레이어를 초기화시키는 함수
    //private void InitializeHP()
    //{
    //    currentHP = maxHP;
    //}
    #endregion
}
