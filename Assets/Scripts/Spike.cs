using UnityEngine;
using Environment;

public class Spike : MonoBehaviour
{
    // 플레이어에게 입히는 스파이크 데미지
    [SerializeField]
    private float damage;

    // 스파이크가 플레이어를 밀어내는 힘
    [SerializeField]
    private float bouncePower;

    // 스파이크에 부딪힌 플레이어 오브젝트
    private GameObject playerObject = null;

    private void FixedUpdate()
    {
        // 플레이어가 스파이크에 닿아 playerObject 를 받아오고,
        // 플레이어가 피격 당한 상태가 아닐 때
        if (playerObject != null && playerObject.GetComponent<PlayerHP>().IsHit == false)
        {
            // 플레이어에게 데미지를 입히고 뒤로 밀리게 한다.
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
