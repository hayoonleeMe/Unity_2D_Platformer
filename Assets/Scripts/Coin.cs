using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // 동전의 점수
    private int score;

    // 회전 y축 오프셋
    private float rotOffsetY = 5f;

    // 이동 x축 오프셋
    private float posOffsetX;

    // CoinBounceRoutine 코루틴의 IEnumerator
    private IEnumerator coinBounceRoutine;

    // 코인이 사라지는 경과시간
    private const float COIN_DIE_DURATION = 0.15f;

    private void Awake()
    {
        score = 100;

        posOffsetX = 1.5f * Time.fixedDeltaTime;

        coinBounceRoutine = CoinBounceRoutine();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().AddScore(score);

            // 특정 효과 후 코인은 사라진다.
            StartCoroutine(CoinDieRoutine());
        }
    }

    private void FixedUpdate()
    {
        // 회전하는 동전을 나타낸다.
        transform.Rotate(0f, rotOffsetY, 0f);
    }

    // 코인이 사라지게 하는 코루틴
    private IEnumerator CoinDieRoutine()
    {
        GetComponent<CircleCollider2D>().enabled = false;

        rotOffsetY *= 3;

        StartCoroutine(coinBounceRoutine);

        yield return new WaitForSeconds(COIN_DIE_DURATION);

        posOffsetX *= -0.6f;

        StartCoroutine(CoinFadeAwayRoutine());

        yield return new WaitForSeconds(COIN_DIE_DURATION);

        StopCoroutine(coinBounceRoutine);

        Destroy(gameObject);
    }

    // 코인이 튀어올랐다가 내려오는 효과를 재생하는 코루틴
    private IEnumerator CoinBounceRoutine()
    {
        while (true)
        {
            Vector3 pos = transform.position;
            pos.y += posOffsetX;
            transform.position = pos;

            yield return null;
        }
    }

    // 코인이 점점 사라지는 효과를 재생하는 코루틴
    private IEnumerator CoinFadeAwayRoutine()
    {
        float elapsedTime = 0f;
        Vector2 originScale = transform.localScale;

        while (elapsedTime < COIN_DIE_DURATION)
        {
            elapsedTime += Time.deltaTime;

            transform.localScale = Vector2.Lerp(originScale, Vector2.zero, elapsedTime / COIN_DIE_DURATION);

            yield return null;
        }
    }
}
