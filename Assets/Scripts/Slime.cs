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

    // �������� �׾��� ���� ��������Ʈ
    [SerializeField]
    private Sprite slimeDead;

    // �÷��̾�� ������ ������ ������
    [SerializeField]
    private float damage;

    // BounceMode.Damage Ÿ���� Bounce �޼ҵ忡 ����Ǵ� �������� �÷��̾ �о�� ��
    [SerializeField]
    private float damageBouncePower;

    // BounceMode.Normal Ÿ���� Bounce �޼ҵ忡 ����Ǵ� �������� �÷��̾ �о�� ��
    [SerializeField]
    private float normalBouncePower;

    private GameObject playerObject = null;

    // �������� ���� ����
    private Vector2 nextDir = Vector2.zero;

    // SlimeMoveRoutine �ڷ�ƾ�� ��ȯ�� IEnumerator
    private IEnumerator slimeMoveRoutine;

    // SizeDownEffect �� ������ �ð�
    private const float SIZE_DOWN_EFFECT_DURATION = 1.0f;

    // �������� �̵� ������ �ٲٴ� ������
    private const float DIR_CHANGE_DELAY = 2.0f;

    // ������ ��� �� ȸ���� ���� �ӵ�
    private const float ROTATE_SPEED = 250.0f;

    // �������� ������ �� �����ϴ� ����
    private bool deadFlipX;

    // �������� Ÿ�� ���ϴ� ������ y��ǥ�� ��ȯ�ϴ� ������Ƽ
    public float HitSpotY => transform.position.y + polygonCollider2D.points[1].y * transform.localScale.y;

    // �������� Ÿ�� ���ϴ� ������ �ּ� x��ǥ�� ��ȯ�ϴ� ������Ƽ
    public float HitSpotMinX => transform.position.x + polygonCollider2D.points[0].x * transform.localScale.x;
    
    // �������� Ÿ�� ���ϴ� ������ �ִ� x��ǥ�� ��ȯ�ϴ� ������Ƽ
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
        // ����ĳ��Ʈ�� �������� �տ� Ground Ÿ���� �ִ��� üũ�Ѵ�.

        Debug.DrawRay(rigidBody2D.position + nextDir, Vector3.down, Color.red);
        RaycastHit2D rayHit = Physics2D.Raycast(rigidBody2D.position + nextDir, Vector3.down, 1.0f, LayerMask.GetMask("Ground"));

        // Ground Ÿ���� ������ �������� �̵������� �ݴ�� �ٲ۴�.
        if (rayHit.collider == null)
        {
            nextDir *= -1;
            movement2D.MoveTo(nextDir);
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            //spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        // �����Ӱ��� �浹�� ���� ó���� �Ѵ�.
        if (playerObject != null)
        {
            // �������� ���� �÷��̾ �ε����� �÷��̾�� �������� ������.
            if (!CanPlayerAttackSlime() && playerObject.GetComponent<PlayerHP>().IsHit == false)
            {
                // �÷��̾�� �������� ������.
                playerObject.GetComponent<PlayerHP>().TakeDamage(damage);
                // �÷��̾ Ƣ������� �Ѵ�.
                playerObject.GetComponent<PlayerController>().Bounce(damageBouncePower, BounceMode.Damage);
            }
            // �������� ���� �÷��̾ ������ �������� �״´�.
            else if (CanPlayerAttackSlime())
            {
                // �÷��̾ Ƣ������� �Ѵ�.
                playerObject.GetComponent<PlayerController>().Bounce(normalBouncePower);

                // �������� �Ҹ��Ѵ�.
                OnDie();
            }
        }
    }

    // �÷��̾ �������� ������ �� �ִ����� ��ȯ�Ѵ�.
    private bool CanPlayerAttackSlime()
    {
        // �÷��̾��� Ÿ�� ������ �������� �ǰ� �������� ���� �ְ�,
        // pivot �� ���߾��� �÷��̾��� position �� �������� �ǰ� ���� ���ο� �ִٸ� �÷��̾�� �������� ������ �� �ִ�.
        if (playerObject.GetComponent<PlayerController>().AttackSpotY >= HitSpotY &&
           ((playerObject.GetComponent<PlayerController>().AttackSpotMinX >= HitSpotMinX && playerObject.GetComponent<PlayerController>().AttackSpotMinX <= HitSpotMaxX) ||
           (playerObject.GetComponent<PlayerController>().AttackSpotMaxX >= HitSpotMinX && playerObject.GetComponent<PlayerController>().AttackSpotMaxX <= HitSpotMaxX)))
        {
            return true;
        }

        return false;
    }

    // �������� �������� �������� �̵�, ���������� �̵�, ���ڸ��� �״�� �� �ϳ��� �̵��� �����ϵ��� �ϴ� �ڷ�ƾ
    private IEnumerator SlimeMoveRoutine()
    {
        while (true)
        {
            ChangeNextDir();

            yield return new WaitForSeconds(DIR_CHANGE_DELAY);
        }
    }

    // �������� ���� �̵� ������ ���Ѵ�.
    private void ChangeNextDir()
    {
        nextDir.x = Random.Range(-1, 2);

        movement2D.MoveTo(nextDir);

        // �ݴ� �������� �� ��
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

    // �������� ���� �� ȣ��ȴ�.
    private void OnDie()
    {
        StopCoroutine(slimeMoveRoutine);
        //deadFlipX = spriteRenderer.flipX;

        BounceOff();

        StartCoroutine(SizeDownAndRotateEffectRoutine());
    }

    // ������ ��������Ʈ�� ����� SIZE_DOWN_EFFECT_DURATION ���� 0���� ���̰� ����������� ȸ����Ű�� �ڷ�ƾ
    private IEnumerator SizeDownAndRotateEffectRoutine()
    {
        polygonCollider2D.enabled = false;
        animator.enabled = false;

        spriteRenderer.sprite = slimeDead;

        float elapsedTime = 0.0f;
        Vector2 originScale = transform.localScale;

        // ������ ��������Ʈ�� �ٶ󺸰� �ִ� ������ �ݴ�� ȸ���Ѵ�.
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

    // ������ ������Ʈ�� ƨ�ܿ����� �Ѵ�.
    private void BounceOff()
    {
        movement2D.MoveTo(Vector2.zero);
        rigidBody2D.velocity = Vector2.zero;
        rigidBody2D.AddForce(Vector2.up * 5.0f, ForceMode2D.Impulse);
    }
}
