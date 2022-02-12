using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    // 플레이어에게 입히는 스파이크 데미지
    [SerializeField]
    private float damage = 1.0f;

    // 스파이크가 플레이어에게 데미지를 입히는 딜레이
    [SerializeField]
    private float hitDelay = 0.5f;

    // 플레이어 오브젝트
    private GameObject playerObject = null;

    // 코루틴 RepeatHitProcess 에서 받아온 IEnumerator
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
                // 플레이어에게 데미지를 입힌다.
                playerObject.GetComponent<PlayerHP>().TakeDamage(damage);
            }

            // hitDelay 만큼 대기한다.
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
