using System.Collections;
using UnityEngine;
using Environment;

public class PlayerController : MonoBehaviour
{
    private Movement2D movement2D;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody2D;

    // ���������� ������ �����ϴ� StageData
    [SerializeField]
    private StageData stageData;

    // �÷��̾��� �ʱ� ��ġ
    private Vector2 initialPos = new Vector2(0.0f, -0.3f);

    // ������ �������� ���
    private float jumpAmount = 15.0f;

    // �����̽��ٸ� Ű�ٿ��Ҷ� �� �����Ӹ��� jumpAmount�� �������� offset 
    private float jumpOffset = 0.05f;

    // �ִ� ���� ���
    private const float MAX_JUMP_AMOUNT = 22.0f;

    // �ּ� ���� ���
    private const float MIN_JUMP_AMOUNT = 15.0f;

    // �÷��̾��� ���¸� üũ�ϴ� �ð�
    private const float CHECK_SECONDS = 0.07f;

    // ������Ʈ���� �����ϴ� ������Ʈ�� �ö󰥶��� �߷� ��
    private const float RISING_GRAVITY_SCALE = 7.0f;

    // ������Ʈ���� �����ϴ� ������Ʈ�� ���������� �߷� ��
    private const float FALLING_GRAVITY_SCALE = 17.0f;

    // �����̽��ٸ� ������ �ִ����� ��Ÿ���� ���º���
    private bool isSpaceDown = false;

    // �÷��̾ ���� �������� ��Ÿ���� ���º���
    private bool isJump = false;
    
    // �÷��̾ �ǰݵǾ������� ��Ÿ���� ���º����� ������Ƽ
    private bool isHurt = false;
    public bool IsHurt => isHurt;

    // �÷��̾ ���� ��Ƽ� ������ ���� y��ǥ�� ������Ƽ
    public float AttackSpotY
    {
        get
        {
            return transform.position.y - spriteRenderer.bounds.size.y / 2;
        }
    }

    // �ǰ� �ִϸ��̼� ����ð�
    private const float HURT_ANIMATION_DURATION = 0.3f;

    /// <summary>
    /// ��ü�� �̸� ����� �δ� �� �����ս��� �����ִ��� Ȯ�� �ʿ�
    /// </summary>
    // �ǰ� �ִϸ��̼� ����ð��� ��Ÿ���� WaitForSeconds ��ü
    private WaitForSeconds hurtAnimationDuration;

    private void Awake()
    {
        movement2D = GetComponent<Movement2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody2D = GetComponent<Rigidbody2D>();

        hurtAnimationDuration = new WaitForSeconds(HURT_ANIMATION_DURATION);
    }

    private void Start()
    {
        // �÷��̾ ���߿� �ִ��� üũ�ϴ� �ڷ�ƾ�� �����Ѵ�.
        StartCoroutine(CheckPlayerIsAir());
    }

    private void Update()
    {
        // Ű���� �Է��� �޾Ƽ� �÷��̾��� �̵������� ���Ѵ�.
        float x = Input.GetAxisRaw("Horizontal");

        // �̵����⿡ ���� �÷��̾��� ��������Ʈ�� ȸ����Ų��.
        if (x < 0) spriteRenderer.flipX = true;
        else if (x > 0) spriteRenderer.flipX = false;

        // �÷��̾ �̵���Ų��.
        movement2D.MoveTo(new Vector2(x, 0.0f));

        // �ȱ� �ִϸ��̼��� �����Ѵ�.
        // ���� �߿��� ���� �ִϸ��̼Ǹ� �����Ѵ�.
        bool walk = (x != 0) ? true : false;
        if (!isJump)
        {
            animator.SetBool("onWalk", walk);
        }

        // �����̽��ٸ� Ű�ٿ����� �ƴҶ��� �����̽��ٸ� �������� üũ�Ѵ�.
        if (!isSpaceDown)
        {
            isSpaceDown = Input.GetKeyDown(KeyCode.Space);
        }

        // �������� �ƴϸ鼭 �����̽��ٸ� Ű�ٿ� ���̸� ������ �غ��Ѵ�.
        // �����̽��ٸ� Ű�ٿ��Ҽ��� �÷��̾ �����ϴ� ���̰� Ŀ����.
        if (isSpaceDown && !isJump)
        {
            PrepareJump();
        }

        // �����̽��ٸ� ���� ������ ���� �����ŭ �÷��̾ �����Ѵ�.
        bool spaceUp = Input.GetKeyUp(KeyCode.Space);
        if (spaceUp && !isJump)
        {
            isSpaceDown = false;
            Jump();
            jumpAmount = MIN_JUMP_AMOUNT;
        }

        // �÷��̾� ������Ʈ�� ������ �� �� �߷� ���� �����Ѵ�.
        if (isJump)
        {
            // ���� �ö� ��
            if (rigidBody2D.velocity.y > 0)
            {
                rigidBody2D.gravityScale = RISING_GRAVITY_SCALE;
            }
            // �Ʒ��� ������ ��
            else
            {
                rigidBody2D.gravityScale = FALLING_GRAVITY_SCALE;
            }
        }
    }

    private void LateUpdate()
    {
        // �÷��̾��� �̵��ݰ��� �����Ѵ�.
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, stageData.LimitMin.x + spriteRenderer.bounds.size.x / 2, stageData.LimitMax.x - spriteRenderer.bounds.size.x / 2),
                                         Mathf.Clamp(transform.position.y, stageData.LimitMin.y + spriteRenderer.bounds.size.y / 2, stageData.LimitMax.y - spriteRenderer.bounds.size.y / 2));
    }

    // �÷��̾ �ʱ�ȭ ��Ų��.
    public void InitializeControl()
    {
        transform.position = initialPos;
    }

    // �����̽��ٸ� Ű�ٿ��� �� ȣ��Ǵ� �Լ�
    // ������ �������� ����� ���� �þ��.
    private void PrepareJump()
    {
        // �� �����Ӹ��� ���� ����� �þ��.
        jumpAmount += jumpOffset;
        // �ִ� ���� ����� ���� �ʴ´�.
        jumpAmount = Mathf.Clamp(jumpAmount, MIN_JUMP_AMOUNT, MAX_JUMP_AMOUNT);
    }

    // �÷��̾� ������Ʈ�� ������Ű�� �޼ҵ�
    private void Jump()
    {
        // ����� ���� ������Ų��.
        rigidBody2D.AddForce(Vector2.up * jumpAmount, ForceMode2D.Impulse);
    }

    // �÷��̾ ���߿� �ִ��� CHECK_SECONDS �ʸ��� üũ�ϴ� �ڷ�ƾ
    private IEnumerator CheckPlayerIsAir()
    {
        while (true)
        {
            // �÷��̾ ������ ��� ���� ��
            if (Mathf.Approximately(rigidBody2D.velocity.y, 0.0f))
            {
                isJump = false;
                animator.SetBool("onFall", false);
                rigidBody2D.gravityScale = 1.0f;
            }
            // �÷��̾ ���߿� ���� ��
            else
            {
                // ���߿��� �ȴ� �ִϸ��̼��� �������̶�� �ߴ��Ѵ�.
                if (animator.GetBool("onWalk"))
                {
                    animator.SetBool("onWalk", false);
                }

                isJump = true;
                animator.SetBool("onFall", true);
            }

            yield return new WaitForSeconds(CHECK_SECONDS);
        }
    }

    // �÷��̾ ��ֹ��� ����� ��, �ڷ� �з����� �Ѵ�.
    public void Bounce(float power, BounceMode mode = BounceMode.Normal)
    {
        rigidBody2D.velocity = Vector2.zero;
        rigidBody2D.AddForce(new Vector2(0.0f, power), ForceMode2D.Impulse);

        if (mode == BounceMode.Damage)
        {
            isHurt = true;
            StartCoroutine(BounceRoutine());
        }
    }

    // Hurt �ִϸ��̼��� ����� �� HURT_ANIMATION_DURATION ���� Hurt �ִϸ��̼��� ���� �ڷ�ƾ
    private IEnumerator BounceRoutine()
    {   
        animator.SetBool("onHurt", true);

        yield return hurtAnimationDuration;

        animator.SetBool("onHurt", false);
        isHurt = false;
    }
}
