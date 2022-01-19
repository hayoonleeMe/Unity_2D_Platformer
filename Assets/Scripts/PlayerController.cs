using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Movement2D movement2D;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody2D;

    // ���� �ӵ�
    [SerializeField]
    private float jumpSpeed = 0.0f;

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
    }

    private void Update()
    {
        // Ű���� �Է��� �޾Ƽ� �÷��̾��� �̵������� ���Ѵ�.
        float x = Input.GetAxisRaw("Horizontal");
        
        // �̵����⿡ ���� �÷��̾��� ��������Ʈ�� ȸ����Ų��.
        if (x < 0) spriteRenderer.flipX = true;
        else if (x > 0) spriteRenderer.flipX = false;

        // �������϶� �̵��� �����Ѵ�.
        if (isJump)
        {
            x = 0.0f;
        }
        movement2D.MoveTo(new Vector3(x, 0.0f, 0.0f));

        // �ȱ� �ִϸ��̼��� �����Ѵ�.
        bool walk = (x != 0) ? true : false;
        animator.SetBool("onWalk", walk);

        // �����̽��ٸ� ���� �÷��̾ ������Ų��.
        bool space = Input.GetKeyDown(KeyCode.Space);
        if (space && !isJump) Jump();

    }

    // �÷��̾� ������Ʈ�� ������Ű�� �޼ҵ�
    private void Jump()
    {
        animator.SetBool("onJump", true);
        float x = movement2D.MoveDirectionX;
        rigidBody2D.AddForce(new Vector2(x, jumpSpeed), ForceMode2D.Impulse);
        isJump = true;
    }
}
