using UnityEngine;

public class Movement2D : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;
    private PlayerController playerController;

    // 이동 속도
    [SerializeField]
    private float moveSpeed = 0.0f;

    // 이동 방향
    [SerializeField]
    private Vector2 moveDirection = Vector3.zero;

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        // 플레이어가 데미지를 받아 밀려나는 중이면 이동 제어를 제한한다.
        if (playerController.IsHurt)
        {
            return;
        }

        // 이동방향으로 이동한다.
        rigidBody2D.velocity = new Vector2(moveDirection.x * moveSpeed * Time.fixedDeltaTime, rigidBody2D.velocity.y);
    }

    // 오브젝트의 이동방향을 정하는 메소드
    public void MoveTo(Vector2 direction)
    {
        moveDirection = direction;
    }
}
