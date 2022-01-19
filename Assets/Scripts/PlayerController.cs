using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Movement2D movement2D;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody2D;

    // 점프 속도
    [SerializeField]
    private float jumpSpeed = 0.0f;

    // 플레이어가 점프 중인지를 나타내는 상태변수와 프로퍼티
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
        // 키보드 입력을 받아서 플레이어의 이동방향을 정한다.
        float x = Input.GetAxisRaw("Horizontal");
        
        // 이동방향에 맞춰 플레이어의 스프라이트를 회전시킨다.
        if (x < 0) spriteRenderer.flipX = true;
        else if (x > 0) spriteRenderer.flipX = false;

        // 점프중일때 이동을 제한한다.
        if (isJump)
        {
            x = 0.0f;
        }
        movement2D.MoveTo(new Vector3(x, 0.0f, 0.0f));

        // 걷기 애니메이션을 실행한다.
        bool walk = (x != 0) ? true : false;
        animator.SetBool("onWalk", walk);

        // 스페이스바를 눌러 플레이어를 점프시킨다.
        bool space = Input.GetKeyDown(KeyCode.Space);
        if (space && !isJump) Jump();

    }

    // 플레이어 오브젝트를 점프시키는 메소드
    private void Jump()
    {
        animator.SetBool("onJump", true);
        float x = movement2D.MoveDirectionX;
        rigidBody2D.AddForce(new Vector2(x, jumpSpeed), ForceMode2D.Impulse);
        isJump = true;
    }
}
