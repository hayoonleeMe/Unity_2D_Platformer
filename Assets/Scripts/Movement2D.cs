using UnityEngine;

public class Movement2D : MonoBehaviour
{
    // �̵� �ӵ�
    // �ν����� �信�� ������Ʈ������ �ӵ��� �����Ѵ�.
    [SerializeField]
    private float moveSpeed = 0.0f;

    // �̵� ����
    [SerializeField]
    private Vector3 moveDirection = Vector3.zero;

    // ���� �ӵ�
    [SerializeField]
    private float jumpSpeed = 0.0f;

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

    // ������Ʈ�� ������Ű�� �޼ҵ�
    public void Jump()
    {
        Debug.Log("Jump");
    }

}
