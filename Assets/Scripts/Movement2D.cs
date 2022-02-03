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
        //transform.position += moveDirection * moveSpeed * Time.deltaTime; 

        rigidBody2D.velocity += moveDirection * moveSpeed * Time.deltaTime;
    }

    // ������Ʈ�� �̵������� ���ϴ� �޼ҵ�
    public void MoveTo(Vector2 direction)
    {
        moveDirection = direction;
    }
}
