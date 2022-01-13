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
        // 키보드 입력을 받아서 물체의 이동방향을 정한다.
        float x = Input.GetAxis("Horizontal");
        movement2D.MoveTo(new Vector3(x, 0.0f, 0.0f));
    }
}
