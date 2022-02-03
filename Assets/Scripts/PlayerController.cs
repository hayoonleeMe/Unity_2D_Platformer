using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Movement2D movement2D;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody2D;
    private BoxCollider2D boxCollider2D;

    // ���������� ������ �����ϴ� StageData
    [SerializeField]
    private StageData stageData;

    // ������ �������� ���
    [SerializeField]
    private float jumpAmount = 20.0f;

    // �����̽��ٸ� Ű�ٿ��Ҷ� �� �����Ӹ��� jumpAmount�� �������� offset 
    private float jumpOffset = 0.05f;

    // �ִ� ���� ���
    private const float MAX_JUMP_AMOUNT = 30.0f;

    // �ּ� ���� ���
    private const float MIN_JUMP_AMOUNT = 20.0f;

    // �÷��̾��� ���¸� üũ�ϴ� �ð�
    private const float CHECK_SECONDS = 0.07f;

    // ������Ʈ���� ����Ǵ� �߷� ��
    [SerializeField]
    private float gravityScale = 7.0f;

    // ������Ʈ���� �����ϴ� ������Ʈ�� ���������� �߷� ��
    [SerializeField]
    private float fallingGravityScale = 17.0f;

    // �����̽��ٸ� ������ �ִ����� ��Ÿ���� ���º���
    private bool isSpaceDown = false;

    // �÷��̾ ���� �������� ��Ÿ���� ���º����� ������Ƽ
    private bool isJump = false;
    public bool IsJump
    {
        set
        {
            if (isJump != value)
            {
                isJump = value;
            }
        }
        get => isJump;
    }    

    private void Awake()
    {
        movement2D = GetComponent<Movement2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
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
            if (rigidBody2D.velocity.y >= 0)
            {
                rigidBody2D.gravityScale = gravityScale;
            }
            // �Ʒ��� ������ ��
            else
            {
                rigidBody2D.gravityScale = fallingGravityScale;
            }
        }
    }

    private void LateUpdate()
    {
        // �÷��̾��� �̵��ݰ��� �����Ѵ�.
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, stageData.LimitMin.x + boxCollider2D.size.x / 2, stageData.LimitMax.x - boxCollider2D.size.x / 2),
                                         Mathf.Clamp(transform.position.y, stageData.LimitMin.y + boxCollider2D.size.y / 2, stageData.LimitMax.y - boxCollider2D.size.y / 2));
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
    IEnumerator CheckPlayerIsAir()
    {
        while (true)
        {
            // �÷��̾ ������ ��� ���� ��
            if (rigidBody2D.velocity.y == 0)
            {
                isJump = false;
                animator.SetBool("onFall", false);
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
}
