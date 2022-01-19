using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Movement2D movement2D;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        movement2D = GetComponent<Movement2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // 키보드 입력을 받아서 플레이어의 이동방향을 정한다.
        float x = Input.GetAxisRaw("Horizontal");
        movement2D.MoveTo(new Vector3(x, 0.0f, 0.0f));

        // 이동방향에 맞춰 플레이어의 스프라이트를 회전시킨다.
        if (x < 0) spriteRenderer.flipX = true;
        else if (x > 0) spriteRenderer.flipX = false;

        // 걷기 애니메이션을 실행한다.
        bool walk = (x != 0) ? true : false;
        animator.SetBool("onWalk", walk);

        // 스페이스바를 눌러 플레이어를 점프시킨다.
        bool space = Input.GetKey(KeyCode.Space);
        if (space) movement2D.Jump();

    }
}
