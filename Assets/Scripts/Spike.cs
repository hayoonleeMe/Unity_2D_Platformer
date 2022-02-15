using UnityEngine;

public class Spike : MonoBehaviour
{
    // �÷��̾�� ������ ������ũ ������
    [SerializeField]
    private float damage = 1.0f;

    // ������ũ�� �÷��̾ �о�� ��
    [SerializeField]
    private float movebackPower;

    // ������ũ�� �ε��� �÷��̾� ������Ʈ
    private GameObject playerObject = null;

    private void Update()
    {
        // �÷��̾ ������ũ�� ��� playerObject �� �޾ƿ���,
        // �÷��̾ �ǰ� ���� ���°� �ƴ� ��
        if (playerObject != null && playerObject.GetComponent<PlayerHP>().IsHit == false)
        {
            // �÷��̾�� �������� ������ �ڷ� �и��� �Ѵ�.
            playerObject.GetComponent<PlayerHP>().TakeDamage(damage);
            playerObject.GetComponent<PlayerController>().MoveBack(movebackPower);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.friction != 0)
        {
            playerObject = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.friction != 0)
        {
            playerObject = null;
        }
    }
}
