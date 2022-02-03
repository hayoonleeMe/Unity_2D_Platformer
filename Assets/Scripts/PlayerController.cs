using System.Collections;
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

    // 점프에 가해지는 운동량
    [SerializeField]
    private float jumpAmount = 20.0f;

    // 스페이스바를 키다운할때 매 프레임마다 jumpAmount에 더해지는 offset 
    private float jumpOffset = 0.05f;

    // 최대 점프 운동량
    private const float MAX_JUMP_AMOUNT = 30.0f;

    // 최소 점프 운동량
    private const float MIN_JUMP_AMOUNT = 20.0f;

    // 플레이어의 상태를 체크하는 시간
    private const float CHECK_SECONDS = 0.07f;

    // 오브젝트에만 적용되는 중력 값
    [SerializeField]
    private float gravityScale = 7.0f;

    // 오브젝트에만 적용하는 오브젝트가 떨어질때의 중력 값
    [SerializeField]
    private float fallingGravityScale = 17.0f;

    // 스페이스바를 누르고 있는지를 나타내는 상태변수
    private bool isSpaceDown = false;

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

    private void Start()
    {
        // 플레이어가 공중에 있는지 체크하는 코루틴을 실행한다.
        StartCoroutine(CheckPlayerIsAir());
    }

    private void Update()
    {
        // 키보드 입력을 받아서 플레이어의 이동방향을 정한다.
        float x = Input.GetAxisRaw("Horizontal");

        // 이동방향에 맞춰 플레이어의 스프라이트를 회전시킨다.
        if (x < 0) spriteRenderer.flipX = true;
        else if (x > 0) spriteRenderer.flipX = false;

        // 플레이어를 이동시킨다.
        movement2D.MoveTo(new Vector2(x, 0.0f));

        // 걷기 애니메이션을 실행한다.
        // 점프 중에는 점프 애니메이션만 실행한다.
        bool walk = (x != 0) ? true : false;
        if (!isJump)
        {
            animator.SetBool("onWalk", walk);
        }

        // 스페이스바를 키다운중이 아닐때만 스페이스바를 누르는지 체크한다.
        if (!isSpaceDown)
        {
            isSpaceDown = Input.GetKeyDown(KeyCode.Space);
        }

        // 점프중이 아니면서 스페이스바를 키다운 중이면 점프를 준비한다.
        // 스페이스바를 키다운할수록 플레이어가 점프하는 높이가 커진다.
        if (isSpaceDown && !isJump)
        {
            PrepareJump();
        }

        // 스페이스바를 떼면 정해진 점프 운동량만큼 플레이어가 점프한다.
        bool spaceUp = Input.GetKeyUp(KeyCode.Space);
        if (spaceUp && !isJump)
        {
            isSpaceDown = false;
            Jump();
            jumpAmount = MIN_JUMP_AMOUNT;
        }

        // 플레이어 오브젝트가 점프를 할 때 중력 값을 조절한다.
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

    // 스페이스바를 키다운할 때 호출되는 함수
    // 점프에 가해지는 운동량이 점점 늘어난다.
    private void PrepareJump()
    {
        // 매 프레임마다 점프 운동량이 늘어난다.
        jumpAmount += jumpOffset;
        // 최대 점프 운동량을 넘지 않는다.
        jumpAmount = Mathf.Clamp(jumpAmount, MIN_JUMP_AMOUNT, MAX_JUMP_AMOUNT);
    }

    // 플레이어 오브젝트를 점프시키는 메소드
    private void Jump()
    {
        // 운동량을 더해 점프시킨다.
        rigidBody2D.AddForce(Vector2.up * jumpAmount, ForceMode2D.Impulse);
    }

    // 플레이어가 공중에 있는지 CHECK_SECONDS 초마다 체크하는 코루틴
    IEnumerator CheckPlayerIsAir()
    {
        while (true)
        {
            // 플레이어가 지면을 밟고 있을 때
            if (rigidBody2D.velocity.y == 0)
            {
                isJump = false;
                animator.SetBool("onFall", false);
            }
            // 플레이어가 공중에 있을 때
            else
            {
                // 공중에서 걷는 애니메이션이 실행중이라면 중단한다.
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
