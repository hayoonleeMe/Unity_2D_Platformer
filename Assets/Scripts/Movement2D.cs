using UnityEngine;

public class Movement2D : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;
    private PlayerController playerController;

    // �̵� �ӵ�
    [SerializeField]
    private float moveSpeed = 0.0f;

    // �̵� ����
    [SerializeField]
    private Vector2 moveDirection = Vector3.zero;

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        // �̵��������� �̵��Ѵ�.
        if (playerController.IsHurt)
        {
            return;
        }

        rigidBody2D.velocity = new Vector2(moveDirection.x * moveSpeed * Time.fixedDeltaTime, rigidBody2D.velocity.y);
        
        //rigidBody2D.AddForce(moveDirection * moveSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
        //rigidBody2D.velocity = new Vector2(Mathf.Clamp(rigidBody2D.velocity.x, -5.0f, 5.0f), rigidBody2D.velocity.y);
    }

    // ������Ʈ�� �̵������� ���ϴ� �޼ҵ�
    public void MoveTo(Vector2 direction)
    {
        if (moveDirection.x == 0)
        {
            //rigidBody2D.velocity = new Vector2(0.0f, rigidBody2D.velocity.y);
        }

        moveDirection = direction;
    }
}
