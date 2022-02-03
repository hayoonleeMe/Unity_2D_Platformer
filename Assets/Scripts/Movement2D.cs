using UnityEngine;

public class Movement2D : MonoBehaviour
{
    // 이동 속도
    [SerializeField]
    private float moveSpeed = 0.0f;

    // 이동 방향
    [SerializeField]
    private Vector2 moveDirection = Vector3.zero;

    private Rigidbody2D rigidBody2D;

    private void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // 이동방향으로 이동한다.
        //transform.position += moveDirection * moveSpeed * Time.deltaTime; 

        rigidBody2D.velocity += moveDirection * moveSpeed * Time.deltaTime;
    }

    // 오브젝트의 이동방향을 정하는 메소드
    public void MoveTo(Vector2 direction)
    {
        moveDirection = direction;
    }
}
