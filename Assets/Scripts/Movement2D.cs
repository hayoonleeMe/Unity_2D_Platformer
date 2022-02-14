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
        // 이동방향으로 이동한다.
        if (playerController.IsHurt)
        {
            return;
        }

        rigidBody2D.velocity = new Vector2(moveDirection.x * moveSpeed * Time.fixedDeltaTime, rigidBody2D.velocity.y);
        
        //rigidBody2D.AddForce(moveDirection * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
        //rigidBody2D.velocity = new Vector2(Mathf.Clamp(rigidBody2D.velocity.x, -5.0f, 5.0f), rigidBody2D.velocity.y);
    }

    // 오브젝트의 이동방향을 정하는 메소드
    public void MoveTo(Vector2 direction)
    {
        if (moveDirection.x == 0)
        {
            //rigidBody2D.velocity = new Vector2(0.0f, rigidBody2D.velocity.y);
        }

        moveDirection = direction;
    }
}
