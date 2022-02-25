using UnityEngine;
using Environment;

public class Spike : MonoBehaviour
{
    // �÷��̾�� ������ ������ũ ������
    [SerializeField]
    private float damage;

    // ������ũ�� �÷��̾ �о�� ��
    [SerializeField]
    private float bouncePower;

    // ������ũ�� �ε��� �÷��̾� ������Ʈ
    private GameObject playerObject = null;

    private void FixedUpdate()
    {
        // �÷��̾ ������ũ�� ��� playerObject �� �޾ƿ���,
        // �÷��̾ �ǰ� ���� ���°� �ƴ� ��
        if (playerObject != null && playerObject.GetComponent<PlayerHP>().IsHit == false)
        {
            // �÷��̾�� �������� ������ �ڷ� �и��� �Ѵ�.
            playerObject.GetComponent<PlayerHP>().TakeDamage(damage);
            playerObject.GetComponent<PlayerController>().Bounce(bouncePower, BounceMode.Damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerObject = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerObject = null;
        }
    }
}
