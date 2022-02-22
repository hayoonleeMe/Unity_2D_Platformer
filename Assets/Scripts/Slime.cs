using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
