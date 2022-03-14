using UnityEngine;

public class Movement2D : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;

    // 이동 속도
    [SerializeField]
    private float moveSpeed;

    // 이동 방향
    private Vector2 moveDirection = Vector2.zero;

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // 이동방향으로 이동한다.
        rigidBody2D.velocity = new Vector2(moveDirection.x * moveSpeed, rigidBody2D.velocity.y);
    }

    // 오브젝트의 이동방향을 정하는 메소드
    public void MoveTo(Vector2 direction)
    {
        moveDirection = direction;
    }
}
