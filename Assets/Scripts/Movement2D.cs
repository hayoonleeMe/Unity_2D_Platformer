using UnityEngine;

public class Movement2D : MonoBehaviour
{
    // �̵� �ӵ�
    [SerializeField]
    private float moveSpeed = 0.0f;

    // �̵� ����
    [SerializeField]
    private Vector3 moveDirection = Vector3.zero;
    public float MoveDirectionX
    {
        get => moveDirection.x * moveSpeed / 2; 
    }

    private void Update()
    {
       // �̵��������� �̵��Ѵ�.
       transform.position += moveDirection * moveSpeed * Time.deltaTime; 
    }

    // ������Ʈ�� �̵������� ���ϴ� �޼ҵ�
    public void MoveTo(Vector3 direction)
    {
        moveDirection = direction;
    }
}
