using UnityEngine;

public class Movement2D : MonoBehaviour
{
    // 이동 속도
    // 인스펙터 뷰에서 오브젝트마다의 속도를 지정한다.
    [SerializeField]
    private float moveSpeed = 0.0f;

    // 이동 방향
    [SerializeField]
    private Vector3 moveDirection = Vector3.zero;

    // 점프 속도
    [SerializeField]
    private float jumpSpeed = 0.0f;

    private void Update()
    {
       // 이동방향으로 이동한다.
       transform.position += moveDirection * moveSpeed * Time.deltaTime; 
    }

    // 오브젝트의 이동방향을 정하는 메소드
    public void MoveTo(Vector3 direction)
    {
        moveDirection = direction;
    }

    // 오브젝트를 점프시키는 메소드
    public void Jump()
    {
        Debug.Log("Jump");
    }

}
