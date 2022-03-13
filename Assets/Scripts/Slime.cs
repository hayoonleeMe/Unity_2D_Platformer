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

    // BounceMode.Damage 타입의 Bounce 메소드에 적용되는 슬라임이 플레이어를 밀어내는 힘
    [SerializeField]
    private float damageBouncePower;

    // BounceMode.Normal 타입의 Bounce 메소드에 적용되는 슬라임이 플레이어를 밀어내는 힘
    [SerializeField]
    private float normalBouncePower;

    private GameObject playerObject = null;

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

    // 슬라임이 타격 당하는 지점의 y좌표를 반환하는 프로퍼티
    public float HitSpotY => transform.position.y + polygonCollider2D.points[1].y * transform.localScale.y;

    // 슬라임이 타격 당하는 지점의 최소 x좌표를 반환하는 프로퍼티
    public float HitSpotMinX => transform.position.x + polygonCollider2D.points[0].x * transform.localScale.x;
    
    // 슬라임이 타격 당하는 지점의 최대 x좌표를 반환하는 프로퍼티
    public float HitSpotMaxX => transform.position.x + polygonCollider2D.points[4].x * transform.localScale.x;

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

        Debug.DrawRay(rigidBody2D.position + nextDir, Vector3.down, Color.red);
        RaycastHit2D rayHit = Physics2D.Raycast(rigidBody2D.position + nextDir, Vector3.down, 1.0f, LayerMask.GetMask("Ground"));

        // Ground 타일이 없으면 슬라임의 이동방향을 반대로 바꾼다.
        if (rayHit.collider == null)
        {
            nextDir *= -1;
            movement2D.MoveTo(nextDir);
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            //spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        // 슬라임과의 충돌에 대한 처리를 한다.
        if (playerObject != null)
        {
            // 슬라임의 옆에 플레이어가 부딪히면 플레이어에게 데미지를 입힌다.
            if (!CanPlayerAttackSlime() && playerObject.GetComponent<PlayerHP>().IsHit == false)
            {
                // 플레이어게 데미지를 입힌다.
                playerObject.GetComponent<PlayerHP>().TakeDamage(damage);
                // 플레이어를 튀어오르게 한다.
                playerObject.GetComponent<PlayerController>().Bounce(damageBouncePower, BounceMode.Damage);
            }
            // 슬라임의 위를 플레이어가 밟으면 슬라임은 죽는다.
            else if (CanPlayerAttackSlime())
            {
                // 플레이어를 튀어오르게 한다.
                playerObject.GetComponent<PlayerController>().Bounce(normalBouncePower);

                // 슬라임은 소멸한다.
                OnDie();
            }
        }
    }

    // 플레이어가 슬라임을 공격할 수 있는지를 반환한다.
    private bool CanPlayerAttackSlime()
    {
        // 플레이어의 타격 지점이 슬라임의 피격 지점보다 위에 있고,
        // pivot 이 정중앙인 플레이어의 position 이 슬라임의 피격 지점 내부에 있다면 플레이어는 슬라임을 공격할 수 있다.
        if (playerObject.GetComponent<PlayerController>().AttackSpotY >= HitSpotY &&
           ((playerObject.GetComponent<PlayerController>().AttackSpotMinX >= HitSpotMinX && playerObject.GetComponent<PlayerController>().AttackSpotMinX <= HitSpotMaxX) ||
           (playerObject.GetComponent<PlayerController>().AttackSpotMaxX >= HitSpotMinX && playerObject.GetComponent<PlayerController>().AttackSpotMaxX <= HitSpotMaxX)))
        {
            return true;
        }

        return false;
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

        // 반대 방향으로 갈 때
        if (nextDir.x * transform.localScale.x > 0.0f)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerObject = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerObject = null;
        }
    }

    // 슬라임이 죽을 때 호출된다.
    private void OnDie()
    {
        StopCoroutine(slimeMoveRoutine);
        //deadFlipX = spriteRenderer.flipX;

        BounceOff();

        StartCoroutine(SizeDownAndRotateEffectRoutine());
    }

    // 슬라임 스프라이트의 사이즈를 SIZE_DOWN_EFFECT_DURATION 동안 0으로 줄이고 진행방향으로 회전시키는 코루틴
    private IEnumerator SizeDownAndRotateEffectRoutine()
    {
        polygonCollider2D.enabled = false;
        animator.enabled = false;

        spriteRenderer.sprite = slimeDead;

        float elapsedTime = 0.0f;
        Vector2 originScale = transform.localScale;

        // 슬라임 스프라이트가 바라보고 있는 방향의 반대로 회전한다.
        float direction;
        if (transform.localScale.x > 0.0f)
        {
            direction = -1.0f;
        }
        else
        {
            direction = 1.0f;
        }

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
