using System.Collections;
using UnityEngine;
using Environment;

public class Slime : MonoBehaviour
{
    private Movement2D movement2D;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody2D;
    private PolygonCollider2D polygonCollider2D;
    private Animator animator;

    // 슬라임이 죽었을 때의 스프라이트
    [SerializeField]
    private Sprite slimeDead;

    // 플레이어에게 입히는 슬라임 데미지
    [SerializeField]
    private float damage;

    // 슬라임이 플레이어를 밀어내는 힘
    [SerializeField]
    private float bouncePower;

    // 슬라임의 다음 방향
    private Vector2 nextDir = Vector2.zero;

    // SlimeMoveRoutine 코루틴의 반환값 IEnumerator
    private IEnumerator slimeMoveRoutine;

    // SizeDownEffect 를 진행할 시간
    private const float SIZE_DOWN_EFFECT_DURATION = 1.0f;

    // 슬라임이 이동 방향을 바꾸는 딜레이
    private const float DIR_CHANGE_DELAY = 2.0f;

    // 슬라임 사망 시 회전할 때의 속도
    private const float ROTATE_SPEED = 250.0f;

    // 슬라임이 밟혔을 때 진행하던 방향
    private bool deadFlipX;

    private void Awake()
    {
        movement2D = GetComponent<Movement2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        slimeMoveRoutine = SlimeMoveRoutine();

        StartCoroutine(slimeMoveRoutine);
    }

    private void FixedUpdate()
    {
        // 레이캐스트로 슬라임의 앞에 Ground 타일이 있는지 체크한다.
        Vector2 rayPos = rigidBody2D.position + nextDir;

        Debug.DrawRay(rayPos, Vector3.down, Color.red);
        RaycastHit2D rayHit = Physics2D.Raycast(rayPos, Vector3.down, 1.0f, LayerMask.GetMask("Ground"));

        // Ground 타일이 없으면 슬라임의 이동방향을 반대로 바꾼다.
        if (rayHit.collider == null)
        {
            nextDir *= -1;
            movement2D.MoveTo(nextDir);
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
    }

    // 슬라임을 랜덤으로 왼쪽으로 이동, 오른쪽으로 이동, 제자리에 그대로 중 하나의 이동을 수행하도록 하는 코루틴
    private IEnumerator SlimeMoveRoutine()
    {
        while (true)
        {
            ChangeNextDir();

            yield return new WaitForSeconds(DIR_CHANGE_DELAY);
        }
    }

    // 슬라임의 다음 이동 방향을 정한다.
    private void ChangeNextDir()
    {
        nextDir.x = Random.Range(-1, 2);

        movement2D.MoveTo(nextDir);

        if (nextDir.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (nextDir.x < 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 슬라임의 옆에 플레이어가 부딪히면 플레이어에게 데미지를 입힌다.
        if (collision.gameObject.CompareTag("Player") && collision.collider.friction == 0)
        {
            if (collision.gameObject.GetComponent<PlayerHP>().IsHit == false)
            {
                collision.gameObject.GetComponent<PlayerHP>().TakeDamage(damage);
                collision.gameObject.GetComponent<PlayerController>().Bounce(bouncePower, BounceMode.Damage);
            }
        }
        // 슬라임의 위를 플레이어가 밟으면 슬라임은 죽는다.
        else if (collision.gameObject.CompareTag("Player") && collision.collider.friction != 0)
        {
            if (collision.gameObject.GetComponent<PlayerHP>().IsHit == false)
            {
                // 플레이어를 튀어오르게 한다.
                collision.gameObject.GetComponent<PlayerController>().Bounce(15.0f);
                OnDie();
            }
        }
    }

    // 슬라임이 죽을 때 호출된다.
    private void OnDie()
    {
        StopCoroutine(slimeMoveRoutine);
        deadFlipX = spriteRenderer.flipX;

        BounceOff();

        StartCoroutine(SizeDownAndRotateEffectRoutine());
    }

    // 슬라임 스프라이트의 사이즈를 SIZE_DOWN_EFFECT_DURATION 동안 0으로 줄이고 진행방향으로 회전시키는 코루틴
    private IEnumerator SizeDownAndRotateEffectRoutine()
    {
        polygonCollider2D.enabled = false;
        animator.enabled = false;

        spriteRenderer.sprite = slimeDead;
        spriteRenderer.flipX = deadFlipX;

        float elapsedTime = 0.0f;
        Vector2 originScale = transform.localScale;

        float direction = deadFlipX ? -1.0f : 1.0f;

        while (elapsedTime < SIZE_DOWN_EFFECT_DURATION)
        {
            elapsedTime += Time.deltaTime;
            
            transform.localScale = Vector2.Lerp(originScale, Vector2.zero, elapsedTime / SIZE_DOWN_EFFECT_DURATION);

            transform.Rotate(0.0f, 0.0f, ROTATE_SPEED * Time.deltaTime * direction, Space.Self);

            yield return null;
        }

        Destroy(gameObject);
    }

    // 슬라임 오브젝트를 튕겨오르게 한다.
    private void BounceOff()
    {
        movement2D.MoveTo(Vector2.zero);
        rigidBody2D.velocity = Vector2.zero;
        rigidBody2D.AddForce(Vector2.up * 5.0f, ForceMode2D.Impulse);

        
    }
}
