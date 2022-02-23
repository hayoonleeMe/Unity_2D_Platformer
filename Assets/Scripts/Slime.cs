using System.Collections;
using UnityEngine;

public class Slime : MonoBehaviour
{
    private Movement2D movement2D;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        movement2D = GetComponent<Movement2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        StartCoroutine(SlimeMoveRoutine());
    }

    private IEnumerator SlimeMoveRoutine()
    {
        while (true)
        {
            movement2D.MoveTo(Vector2.left);
            spriteRenderer.flipX = false;

            yield return new WaitForSeconds(2.0f);

            movement2D.MoveTo(Vector2.right);
            spriteRenderer.flipX = true;

            yield return new WaitForSeconds(2.0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �÷��̾ �����Ӱ� ������ �ε����� �� �÷��̾�� �������� ������.
        if (collision.gameObject.CompareTag("Player") && collision.collider.friction == 0)
        {
            if (collision.gameObject.GetComponent<PlayerHP>().IsHit == false)
            {
                Debug.Log("Player is hitted by slime");
                collision.gameObject.GetComponent<PlayerHP>().TakeDamage(1.0f);
                collision.gameObject.GetComponent<PlayerController>().Bounce(15.0f);
            }
        }
        // �÷��̾ ������ ���� ���� �� �������� �״´�.
        else if (collision.gameObject.CompareTag("Player") && collision.collider.friction != 0)
        {
            Debug.Log("Slime is die");
        }
    }
}
