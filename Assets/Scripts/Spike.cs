using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    // �÷��̾�� ������ ������ũ ������
    [SerializeField]
    private float damage = 1.0f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ������ũ�� �÷��̾�� �ε��� ��
        if (collision.gameObject.CompareTag("Player"))
        {
            // �÷��̾��� Collider �� frictionless �� Collider �̹Ƿ� �����Ѵ�.
            if (collision.friction == 0)
            {
                return;
            }
            
            // ������ũ�� ������ ��ŭ �÷��̾�� �������� ������.
            collision.gameObject.GetComponent<PlayerHP>().TakeDamage(damage);
        }
    }
}
