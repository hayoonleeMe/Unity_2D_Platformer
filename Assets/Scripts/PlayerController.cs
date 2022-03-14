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

    // �÷��̾� ������Ʈ�� ������ ���͸��Ѵ�.
    private ContactFilter2D tilemapFilter;

    // �÷��̾��� �ʱ� ��ġ
    private Vector2 initialPos = new Vector2(0.0f, -0.3f); 

    // ������ �������� ���
    private float jumpAmount = 16f;

    // �����̽��ٸ� Ű�ٿ��Ҷ� �� �����Ӹ��� jumpAmount�� �������� offset 
    private float jumpOffset = 0.05f;

    // �ִ� ���� ���
    private float MAX_JUMP_AMOUNT = 22.0f;

    // �ּ� ���� ���
    private float MIN_JUMP_AMOUNT = 15.0f;

    // ������Ʈ���� �����ϴ� ������Ʈ�� �ö󰥶��� �߷� ��
    private const float IN_AIR_GRAVITY_SCALE = 5.0f;

    // ���� ������ ���� Ƚ��
    public int jumpCount;

    // �ִ� ���� Ƚ��
    private int maxJumpCount = 2;

    // �����̽��ٸ� ������ �ִ����� ��Ÿ���� ���º���
    private bool isSpaceDown = false;

    // �÷��̾ ���߿� �ִ����� ��Ÿ���� ���º���
    private bool isInAir = false;

    // �÷��̾ �� ���� �ִ����� ��ȯ�ϴ� ������Ƽ
    public bool IsGrounded => rigidBody2D.IsTouching(tilemapFilter);

    // �÷��̾ �ǰݵǾ������� ��Ÿ���� ���º����� ������Ƽ
    private bool isHurt = false;
    public bool IsHurt => isHurt;

    // �÷��̾��� �ּ� Ÿ�� ������ ������Ƽ
    [SerializeField]
    private Transform minAttackSpot;
    public Transform MinAttackSpot
    {
        get
        {
            // �÷��̾ �������� ���� ���� �� (Ÿ�������� ������ ������)
            if (transform.localScale.x > 0f)
            {
                return minAttackSpot;
            }
            // �÷��̾ ������ ���� ���� �� (Ÿ�������� ������ ������)
            else
            {
                return maxAttackSpot;
            }
        }
    }

    // �÷��̾��� �ִ� Ÿ�� ������ ������Ƽ
    [SerializeField]
    private Transform maxAttackSpot;
    public Transform MaxAttackSpot
    {
        get
        {
            // �÷��̾ �������� ���� ���� �� (Ÿ�������� ������ ������)
            if (transform.localScale.x > 0f)
            {
                return maxAttackSpot;
            }
            // �÷��̾ ������ ���� ���� �� (Ÿ�������� ������ ������)
            else
            {
                return minAttackSpot;
            }
        }
    }

    // �ǰ� �ִϸ��̼� ����ð�
    private const float HURT_ANIMATION_DURATION = 0.3f;

    /// <summary>
    /// ��ü�� �̸� ����� �δ� �� �����ս��� �����ִ��� Ȯ�� �ʿ�
    /// -> �ڷ�ƾ ����ȭ �ʿ�
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

        tilemapFilter.SetLayerMask(LayerMask.GetMask("Ground"));
        tilemapFilter.SetNormalAngle(45.0f, 135.0f);

        jumpCount = maxJumpCount;
    }

    private void Update()
    {
        // Ű���� �Է��� �޾Ƽ� �÷��̾��� �̵������� ���Ѵ�.
        float x = Input.GetAxisRaw("Horizontal");

        // �̵����⿡ ���� �÷��̾��� ��������Ʈ�� �¿�� ������Ų��.
        if (transform.localScale.x * x < 0f)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        // �÷��̾ �̵���Ų��.
        movement2D.MoveTo(new Vector2(x, 0.0f));

        // �ȱ� �ִϸ��̼��� �����Ѵ�.
        // ���� �߿��� ���� �ִϸ��̼Ǹ� �����Ѵ�.
        bool walk = (x != 0) ? true : false;
        if (!isInAir)
        { 
            animator.SetBool("onWalk", walk);
        }

        //IntensityControlJump();
        MultipleJump();
        
    }

    private void LateUpdate()
    {
        // �÷��̾��� �̵��ݰ��� �����Ѵ�.
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, stageData.LimitMin.x + spriteRenderer.bounds.size.x / 2, stageData.LimitMax.x - spriteRenderer.bounds.size.x / 2),
                                         Mathf.Clamp(transform.position.y, stageData.LimitMin.y + spriteRenderer.bounds.size.y / 2, stageData.LimitMax.y - spriteRenderer.bounds.size.y / 2));
    }

    private void FixedUpdate()
    {
        // �÷��̾ �� ���� �ִ���, ���߿� �ִ��� üũ�Ѵ�.
        // �÷��̾ �� ���� ���� ��
        if (IsGrounded)
        {
            isInAir = false;
            animator.SetBool("onFall", isInAir);
            rigidBody2D.gravityScale = 1.0f;

            if (!isSpaceDown && jumpCount != maxJumpCount)
            {
                jumpCount = maxJumpCount;
            }
        }
        // �÷��̾ ���߿� ���� ��
        else
        {
            // ���߿��� �ȴ� �ִϸ��̼��� �������̶�� �ߴ��Ѵ�.
            if (animator.GetBool("onWalk"))
            {
                animator.SetBool("onWalk", false);
            }

            isInAir = true;
            animator.SetBool("onFall", isInAir);
        }

        // �÷��̾� ������Ʈ�� ������ �� �� �߷� ���� �����Ѵ�.
        if (isInAir)
        {
            rigidBody2D.gravityScale = IN_AIR_GRAVITY_SCALE;
        }
    }

    // �÷��̾ �ʱ�ȭ ��Ų��.
    public void InitializeControl()
    {
        transform.position = initialPos;
    }

    // �÷��̾� ������Ʈ�� ������Ű�� �޼ҵ�
    private void Jump()
    {
        Vector2 vel = rigidBody2D.velocity;
        vel.y = 0.0f;
        rigidBody2D.velocity = vel;

        // ����� ���� ������Ų��.
        rigidBody2D.AddForce(Vector2.up * jumpAmount, ForceMode2D.Impulse);
    }

    // �÷��̾ ������ ������Ų��.
    private void MultipleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isSpaceDown = true;

            if (jumpCount > 0f)
            {
                Jump();
                --jumpCount;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isSpaceDown = false;
        }
    }

    // �����̽��ٸ� Ű�ٿ��� ������ ���⸦ �����Ѵ�.
    private void IntensityControlJump()
    {
        // �����̽��ٸ� Ű�ٿ����� �ƴҶ��� �����̽��ٸ� �������� üũ�Ѵ�.
        if (!isSpaceDown)
        {
            isSpaceDown = Input.GetKeyDown(KeyCode.Space);
        }

        // �������� �ƴϸ鼭 �����̽��ٸ� Ű�ٿ� ���̸� ������ �غ��Ѵ�.
        // �����̽��ٸ� Ű�ٿ��Ҽ��� �÷��̾ �����ϴ� ���̰� Ŀ����.
        if (isSpaceDown && !isInAir)
        {
            PrepareJump();
        }

        // �����̽��ٸ� ���� ������ ���� �����ŭ �÷��̾ �����Ѵ�.
        bool spaceUp = Input.GetKeyUp(KeyCode.Space);
        if (spaceUp && !isInAir)
        {
            isSpaceDown = false;
            Jump();
            jumpAmount = MIN_JUMP_AMOUNT;
        }
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

    // �÷��̾ ��ֹ��� ����� ��, �ڷ� �з����� �Ѵ�.
    public void Bounce(Vector2 powerDir, BounceMode mode = BounceMode.Normal)
    {
        rigidBody2D.velocity = Vector2.zero;
        rigidBody2D.AddForce(powerDir, ForceMode2D.Impulse);

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
