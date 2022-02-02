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

    // ���� �ӵ�
    [SerializeField]
    private float jumpAmount = 20.0f;

    // ������Ʈ���� ����Ǵ� �߷� ��
    [SerializeField]
    private float gravityScale = 7.0f;

    // ������Ʈ���� �����ϴ� ������Ʈ�� ���������� �߷� ��
    [SerializeField]
    private float fallingGravityScale = 17.0f;

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

    private void Update()
    {
        // Ű���� �Է��� �޾Ƽ� �÷��̾��� �̵������� ���Ѵ�.
        float x = Input.GetAxisRaw("Horizontal");

        // �̵����⿡ ���� �÷��̾��� ��������Ʈ�� ȸ����Ų��.
        if (x < 0) spriteRenderer.flipX = true;
        else if (x > 0) spriteRenderer.flipX = false;

        // �÷��̾ �̵���Ų��.
        movement2D.MoveTo(new Vector3(x, 0.0f, 0.0f));

        // �ȱ� �ִϸ��̼��� �����Ѵ�.
        // ���� �߿��� ���� �ִϸ��̼Ǹ� �����Ѵ�.
        bool walk = (x != 0) ? true : false;
        if (!isJump)
        {
            animator.SetBool("onWalk", walk);
        }

        // �����̽��ٸ� ���� �÷��̾ ������Ų��.
        bool space = Input.GetKeyDown(KeyCode.Space);
        if (space && !isJump) Jump();

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

    // �÷��̾� ������Ʈ�� ������Ű�� �޼ҵ�
    private void Jump()
    {
        // �ȱ� �ִϸ��̼��� �������̸� �����Ѵ�.
        if (animator.GetBool("onWalk"))
        {
            animator.SetBool("onWalk", false);
        }

        // ���� �ִϸ��̼��� �����Ų��.
        animator.SetBool("onJump", true);
        // ����� ���� ������Ų��.
        rigidBody2D.AddForce(Vector2.up * jumpAmount, ForceMode2D.Impulse);
        isJump = true;
    }
}
