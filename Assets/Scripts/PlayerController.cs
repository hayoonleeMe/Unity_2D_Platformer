using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Movement2D movement2D;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        movement2D = GetComponent<Movement2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Ű���� �Է��� �޾Ƽ� �÷��̾��� �̵������� ���Ѵ�.
        float x = Input.GetAxisRaw("Horizontal");
        movement2D.MoveTo(new Vector3(x, 0.0f, 0.0f));

        // �̵����⿡ ���� �÷��̾��� ��������Ʈ�� ȸ����Ų��.
        if (x < 0) spriteRenderer.flipX = true;
        else if (x > 0) spriteRenderer.flipX = false;

        // �ȱ� �ִϸ��̼��� �����Ѵ�.
        bool walk = (x != 0) ? true : false;
        animator.SetBool("onWalk", walk);

        // �����̽��ٸ� ���� �÷��̾ ������Ų��.
        bool space = Input.GetKey(KeyCode.Space);
        if (space) movement2D.Jump();

    }
}
