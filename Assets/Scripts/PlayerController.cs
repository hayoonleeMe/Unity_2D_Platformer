using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Movement2D movement2D;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody2D;
    private BoxCollider2D boxCollider2D;

    // 스테이지의 정보를 저장하는 StageData
    [SerializeField]
    private StageData stageData;

    // 점프 속도
    [SerializeField]
    private float jumpAmount = 20.0f;

    // 오브젝트에만 적용되는 중력 값
    [SerializeField]
    private float gravityScale = 7.0f;

    // 오브젝트에만 적용하는 오브젝트가 떨어질때의 중력 값
    [SerializeField]
    private float fallingGravityScale = 17.0f;

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
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        // 키보드 입력을 받아서 플레이어의 이동방향을 정한다.
        float x = Input.GetAxisRaw("Horizontal");

        // 이동방향에 맞춰 플레이어의 스프라이트를 회전시킨다.
        if (x < 0) spriteRenderer.flipX = true;
        else if (x > 0) spriteRenderer.flipX = false;

        // 플레이어를 이동시킨다.
        movement2D.MoveTo(new Vector3(x, 0.0f, 0.0f));

        // 걷기 애니메이션을 실행한다.
        // 점프 중에는 점프 애니메이션만 실행한다.
        bool walk = (x != 0) ? true : false;
        if (!isJump)
        {
            animator.SetBool("onWalk", walk);
        }

        // 스페이스바를 눌러 플레이어를 점프시킨다.
        bool space = Input.GetKeyDown(KeyCode.Space);
        if (space && !isJump) Jump();

        if (isJump)
        {
            // 위로 올라갈 때
            if (rigidBody2D.velocity.y >= 0)
            {
                rigidBody2D.gravityScale = gravityScale;
            }
            // 아래로 내려갈 때
            else
            {
                rigidBody2D.gravityScale = fallingGravityScale;
            }
        }

    }

    private void LateUpdate()
    {
        // 플레이어의 이동반경을 제한한다.
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, stageData.LimitMin.x + boxCollider2D.size.x / 2, stageData.LimitMax.x - boxCollider2D.size.x / 2),
                                         Mathf.Clamp(transform.position.y, stageData.LimitMin.y + boxCollider2D.size.y / 2, stageData.LimitMax.y - boxCollider2D.size.y / 2));
    }

    // 플레이어 오브젝트를 점프시키는 메소드
    private void Jump()
    {
        // 걷기 애니메이션이 실행중이면 중지한다.
        if (animator.GetBool("onWalk"))
        {
            animator.SetBool("onWalk", false);
        }

        // 점프 애니메이션을 실행시킨다.
        animator.SetBool("onJump", true);
        // 운동량을 더해 점프시킨다.
        rigidBody2D.AddForce(Vector2.up * jumpAmount, ForceMode2D.Impulse);
        isJump = true;
    }
}
