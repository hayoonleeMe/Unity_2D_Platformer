using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    // 플레이어에게 입히는 스파이크 데미지
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
        // 스파이크가 플레이어와 부딪힐 때
        if (collision.gameObject.CompareTag("Player"))
        {
            // 플레이어의 Collider 가 frictionless 인 Collider 이므로 제외한다.
            if (collision.friction == 0)
            {
                return;
            }
            
            // 스파이크의 데미지 만큼 플레이어에게 데미지를 입힌다.
            collision.gameObject.GetComponent<PlayerHP>().TakeDamage(damage);
        }
    }
}
