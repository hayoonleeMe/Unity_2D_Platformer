using UnityEngine;

public class Movement2D : MonoBehaviour
{
    // �̵� �ӵ�
    [SerializeField]
    private float moveSpeed = 0.0f;

    // �̵� ����
    [SerializeField]
    private Vector2 moveDirection = Vector3.zero;

    private Rigidbody2D rigidBody2D;

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // �̵��������� �̵��Ѵ�.
        //rigidBody2D.AddForce(new Vector2(moveDirection.x * moveSpeed, rigidBody2D.velocity.y) - rigidBody2D.velocity, ForceMode2D.Impulse);
        rigidBody2D.velocity = new Vector2(moveDirection.x * moveSpeed, rigidBody2D.velocity.y);
    }

    // ������Ʈ�� �̵������� ���ϴ� �޼ҵ�
    public void MoveTo(Vector2 direction)
    {
        moveDirection = direction;

        //if (moveDirection != direction)
        //{
        //    moveDirection = direction;
        //    rigidBody2D.velocity = moveDirection * moveSpeed;
        //}
    }
}
