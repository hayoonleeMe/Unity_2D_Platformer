using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Movement2D movement2D;

    private void Awake()
    {
        movement2D = GetComponent<Movement2D>();
    }

    private void Update()
    {
        // Ű���� �Է��� �޾Ƽ� ��ü�� �̵������� ���Ѵ�.
        float x = Input.GetAxis("Horizontal");
        movement2D.MoveTo(new Vector3(x, 0.0f, 0.0f));
    }
}
