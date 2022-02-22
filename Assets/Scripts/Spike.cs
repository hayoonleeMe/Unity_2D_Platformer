using System.Collections;
using UnityEngine;

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
            playerObject.GetComponent<PlayerController>().Bounce(bouncePower);
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
