using System.Collections;
using UnityEngine;
using Environment;

public class PlayerController : MonoBehaviour
{
    private Movement2D movement2D;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody2D;

    // 스테이지의 정보를 저장하는 StageData
    [SerializeField]
    private StageData stageData;

    // 플레이어 오브젝트의 접촉을 필터링한다.
    private ContactFilter2D tilemapFilter;

    // 점프에 가해지는 운동량
    private float jumpAmount = 16f;

    // 오브젝트에만 적용하는 오브젝트가 올라갈때의 중력 값
    private const float IN_AIR_GRAVITY_SCALE = 5.0f;

    // 현재 가능한 점프 횟수
    public int jumpCount;

    // 최대 점프 횟수
    private int maxJumpCount = 2;

    // 스페이스바를 누르고 있는지를 나타내는 상태변수
    private bool isSpaceDown = false;

    // 플레이어가 공중에 있는지를 나타내는 상태변수
    private bool isInAir = false;

    // 플레이어가 땅 위에 있는지를 반환하는 프로퍼티
    public bool IsGrounded => rigidBody2D.IsTouching(tilemapFilter);

    // 플레이어가 피격되었는지를 나타내는 상태변수와 프로퍼티
    private bool isHurt = false;
    public bool IsHurt => isHurt;

    // 플레이어의 최소 타격 지점과 프로퍼티
    [SerializeField]
    private Transform minAttackSpot;
    public Transform MinAttackSpot
    {
        get
        {
            // 플레이어가 오른쪽을 보고 있을 때 (타격지점의 순서가 정방향)
            if (transform.localScale.x > 0f)
            {
                return minAttackSpot;
            }
            // 플레이어가 왼쪽을 보고 있을 때 (타격지점의 순서가 역방향)
            else
            {
                return maxAttackSpot;
            }
        }
    }

    // 플레이어의 최대 타격 지점과 프로퍼티
    [SerializeField]
    private Transform maxAttackSpot;
    public Transform MaxAttackSpot
    {
        get
        {
            // 플레이어가 오른쪽을 보고 있을 때 (타격지점의 순서가 정방향)
            if (transform.localScale.x > 0f)
            {
                return maxAttackSpot;
            }
            // 플레이어가 왼쪽을 보고 있을 때 (타격지점의 순서가 역방향)
            else
            {
                return minAttackSpot;
            }
        }
    }

    // 피격 애니메이션 재생시간
    private const float HURT_ANIMATION_DURATION = 0.3f;

    // 점수를 관리하는 오브젝트
    [SerializeField]
    private ManageScore manageScore;

    [SerializeField]
    private ManageJumpEffect jumpEffectManager;

    /// <summary>
    /// 객체를 미리 만들어 두는 게 퍼포먼스에 관련있는지 확인 필요
    /// -> 코루틴 최적화 필요
    /// </summary>
    // 피격 애니메이션 재생시간을 나타내는 WaitForSeconds 객체
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

        jumpEffectManager.SetJumpCount(maxJumpCount);
    }

    private void Update()
    {
        // 키보드 입력을 받아서 플레이어의 이동방향을 정한다.
        float horizontal = Input.GetAxisRaw("Horizontal");

        // 이동방향에 맞춰 플레이어의 스프라이트를 좌우로 반전시킨다.
        if (transform.localScale.x * horizontal < 0f)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }

        // 플레이어를 이동시킨다.
        movement2D.MoveTo(new Vector2(horizontal, 0.0f));

        // 걷기 애니메이션을 실행한다.
        // 점프 중에는 점프 애니메이션만 실행한다.
        animator.SetFloat("speed", Mathf.Abs(horizontal));

        //IntensityControlJump();
        MultipleJump();
    }

    private void LateUpdate()
    {
        // 플레이어의 이동반경을 제한한다.
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, stageData.LimitMin.x + spriteRenderer.bounds.size.x / 2, stageData.LimitMax.x - spriteRenderer.bounds.size.x / 2),
                                         Mathf.Clamp(transform.position.y, stageData.LimitMin.y + spriteRenderer.bounds.size.y / 2, stageData.LimitMax.y - spriteRenderer.bounds.size.y / 2));
    }

    private void FixedUpdate()
    {
        // 플레이어가 땅 위에 있는지, 공중에 있는지 체크한다.
        // 플레이어가 땅 위에 있을 때
        if (IsGrounded)
        {
            isInAir = false;
            animator.SetBool("isJumping", isInAir);
            rigidBody2D.gravityScale = 1.0f;

            if (!isSpaceDown && jumpCount != maxJumpCount)
            {
                jumpCount = maxJumpCount;
            }
        }
        // 플레이어가 공중에 있을 때
        else
        {
            // 땅 위에 있다가 점프없이 공중에 있게 될 경우
            if (isSpaceDown == false && isInAir == false)
            {
                // 점프 횟수를 차감한다.
                --jumpCount;
            }

            isInAir = true;
            animator.SetBool("isJumping", isInAir);
        }

        // 플레이어 오브젝트가 점프를 할 때 중력 값을 조절한다.
        if (isInAir)
        {
            rigidBody2D.gravityScale = IN_AIR_GRAVITY_SCALE;
        }
    }

    // 플레이어 오브젝트를 점프시키는 메소드
    private void Jump()
    {
        Vector2 vel = rigidBody2D.velocity;
        vel.y = 0.0f;
        rigidBody2D.velocity = vel;

        // 운동량을 더해 점프시킨다.
        rigidBody2D.AddForce(Vector2.up * jumpAmount, ForceMode2D.Impulse);
    }

    // 플레이어를 여러번 점프시킨다.
    private void MultipleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isSpaceDown = true;

            if (jumpCount > 0f)
            {
                // 더블점프 이펙트 애니메이션 재생
                if (jumpCount != maxJumpCount)
                {
                    jumpEffectManager.PlayEffect(transform.position);
                }

                Jump();
                --jumpCount;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isSpaceDown = false;
        }
    }

    // 플레이어가 장애물에 닿았을 때, 뒤로 밀려나게 한다.
    public void Bounce(float power, BounceMode mode = BounceMode.Normal)
    {
        rigidBody2D.velocity = Vector2.zero;
        rigidBody2D.AddForce(new Vector2(0.0f, power), ForceMode2D.Impulse);

        if (mode == BounceMode.Damage)
        {
            isHurt = true;
            StartCoroutine(DamageBounceRoutine());
        }
    }

    // 플레이어가 장애물에 닿았을 때, 뒤로 밀려나게 한다.
    public void Bounce(Vector2 powerDir, BounceMode mode = BounceMode.Normal)
    {
        rigidBody2D.velocity = Vector2.zero;
        rigidBody2D.AddForce(powerDir, ForceMode2D.Impulse);

        if (mode == BounceMode.Damage)
        {
            isHurt = true;
            StartCoroutine(DamageBounceRoutine());
        }
    }

    // Hurt 애니메이션을 재생한 후 HURT_ANIMATION_DURATION 이후 Hurt 애니메이션을 끄는 코루틴
    private IEnumerator DamageBounceRoutine()
    {   
        animator.SetBool("isHurt", true);

        yield return hurtAnimationDuration;

        animator.SetBool("isHurt", false);
        isHurt = false;
    }

    // amount만큼의 점수를 업데이트한다.
    public void AddScore(int amount)
    {
        manageScore.updateScore(amount);
    }

    #region Not Use - 강도 조절 점프 관련 변수들
    // Not Use - 스페이스바를 키다운할때 매 프레임마다 jumpAmount에 더해지는 offset 
    //private float jumpOffset = 0.05f;

    // Not Use - 최대 점프 운동량
    //private float MAX_JUMP_AMOUNT = 22.0f;

    // Not Use - 최소 점프 운동량
    //private float MIN_JUMP_AMOUNT = 15.0f;                   
    #endregion

    #region For Debugging - public void InitializeControl() 플레이어를 초기화하는 함수 
    //public void InitializeControl()
    //{
    //    transform.position = initialPos;
    //}
    #endregion

    #region Not Use - private void IntensityControlJump() 스페이스바를 키다운해 점프의 세기를 조절하는 함수
    //private void IntensityControlJump()
    //{
    //    // 스페이스바를 키다운중이 아닐때만 스페이스바를 누르는지 체크한다.
    //    if (!isSpaceDown)
    //    {
    //        isSpaceDown = Input.GetKeyDown(KeyCode.Space);
    //    }

    //    // 점프중이 아니면서 스페이스바를 키다운 중이면 점프를 준비한다.
    //    // 스페이스바를 키다운할수록 플레이어가 점프하는 높이가 커진다.
    //    if (isSpaceDown && !isInAir)
    //    {
    //        PrepareJump();
    //    }

    //    // 스페이스바를 떼면 정해진 점프 운동량만큼 플레이어가 점프한다.
    //    bool spaceUp = Input.GetKeyUp(KeyCode.Space);
    //    if (spaceUp && !isInAir)
    //    {
    //        isSpaceDown = false;
    //        Jump();
    //        jumpAmount = MIN_JUMP_AMOUNT;
    //    }
    //}
    #endregion

    # region Not Use - private void PrepareJump() 스페이스바를 키다운할 때 호출되는 함수
    // 점프에 가해지는 운동량이 점점 늘어난다.
    //private void PrepareJump()
    //{
    //    // 매 프레임마다 점프 운동량이 늘어난다.
    //    jumpAmount += jumpOffset;
    //    // 최대 점프 운동량을 넘지 않는다.
    //    jumpAmount = Mathf.Clamp(jumpAmount, MIN_JUMP_AMOUNT, MAX_JUMP_AMOUNT);
    //}
    #endregion
}
