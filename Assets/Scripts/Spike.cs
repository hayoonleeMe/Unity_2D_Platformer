using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    // �÷��̾�� ������ ������ũ ������
    [SerializeField]
    private float damage = 1.0f;

    // ������ũ�� �÷��̾�� �������� ������ ������
    [SerializeField]
    private float hitDelay = 0.5f;

    // �÷��̾� ������Ʈ
    private GameObject playerObject = null;

    // �ڷ�ƾ RepeatHitProcess ���� �޾ƿ� IEnumerator
    private IEnumerator repeatHitProcess;

    private void Start()
    {
        repeatHitProcess = RepeatHitProcess();  
    }

    private IEnumerator RepeatHitProcess()
    {
        while (true)
        {
            if (playerObject != null)
            {
                // �÷��̾�� �������� ������.
                playerObject.GetComponent<PlayerHP>().TakeDamage(damage);
            }

            // hitDelay ��ŭ ����Ѵ�.
            yield return new WaitForSeconds(hitDelay);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.friction != 0)
        {
            playerObject = collision.gameObject;

            StartCoroutine(repeatHitProcess);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.friction != 0)
        {
            StopCoroutine(repeatHitProcess);
            playerObject = null;
        }
    }
}
