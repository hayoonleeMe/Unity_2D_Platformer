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

    // �������� �÷��̾ �о�� ��
    [SerializeField]
    private float bouncePower;

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
        Vector2 rayPos = rigidBody2D.position + nextDir;

        Debug.DrawRay(rayPos, Vector3.down, Color.red);
        RaycastHit2D rayHit = Physics2D.Raycast(rayPos, Vector3.down, 1.0f, LayerMask.GetMask("Ground"));

        // Ground Ÿ���� ������ �������� �̵������� �ݴ�� �ٲ۴�.
        if (rayHit.collider == null)
        {
            nextDir *= -1;
            movement2D.MoveTo(nextDir);
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }
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
        // �������� ���� �÷��̾ �ε����� �÷��̾�� �������� ������.
        if (collision.gameObject.CompareTag("Player") && collision.collider.friction == 0)
        {
            if (collision.gameObject.GetComponent<PlayerHP>().IsHit == false)
            {
                collision.gameObject.GetComponent<PlayerHP>().TakeDamage(damage);
                collision.gameObject.GetComponent<PlayerController>().Bounce(bouncePower, BounceMode.Damage);
            }
        }
        // �������� ���� �÷��̾ ������ �������� �״´�.
        else if (collision.gameObject.CompareTag("Player") && collision.collider.friction != 0)
        {
            if (collision.gameObject.GetComponent<PlayerHP>().IsHit == false)
            {
                // �÷��̾ Ƣ������� �Ѵ�.
                collision.gameObject.GetComponent<PlayerController>().Bounce(15.0f);
                OnDie();
            }
        }
    }

    // �������� ���� �� ȣ��ȴ�.
    private void OnDie()
    {
        StopCoroutine(slimeMoveRoutine);
        deadFlipX = spriteRenderer.flipX;

        BounceOff();

        StartCoroutine(SizeDownAndRotateEffectRoutine());
    }

    // ������ ��������Ʈ�� ����� SIZE_DOWN_EFFECT_DURATION ���� 0���� ���̰� ����������� ȸ����Ű�� �ڷ�ƾ
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

    // ������ ������Ʈ�� ƨ�ܿ����� �Ѵ�.
    private void BounceOff()
    {
        movement2D.MoveTo(Vector2.zero);
        rigidBody2D.velocity = Vector2.zero;
        rigidBody2D.AddForce(Vector2.up * 5.0f, ForceMode2D.Impulse);

        
    }
}
