using UnityEngine;

public class Movement2D : MonoBehaviour
{
    private Rigidbody2D rigidBody2D;

    // �̵� �ӵ�
    [SerializeField]
    private float moveSpeed;

    // �̵� ����
    private Vector3 moveDirection = Vector2.zero;

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // �̵��������� �̵��Ѵ�.
        //transform.position += moveDirection * moveSpeed * Time.fixedDeltaTime;
        rigidBody2D.velocity = new Vector2(moveDirection.x * moveSpeed * Time.fixedDeltaTime, rigidBody2D.velocity.y);
    }

    // ������Ʈ�� �̵������� ���ϴ� �޼ҵ�
    public void MoveTo(Vector3 direction)
    {
        moveDirection = direction;
    }
}
