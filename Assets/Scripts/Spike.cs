using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    // �÷��̾�� ������ ������ũ ������
    [SerializeField]
    private float damage = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
