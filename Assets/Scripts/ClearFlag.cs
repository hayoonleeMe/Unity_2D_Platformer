using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearFlag : MonoBehaviour
{
    // 플레이어의 PlayerController 스크립트
    [SerializeField]
    private PlayerController playerController;

    // scoreManager의 ManageScore 스크립트
    [SerializeField]
    private ManageScore manageScore;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // StageClear Scene에서 나타낼 Score를 저장한다.
            PlayerPrefs.SetInt("score", manageScore.getScore());

            // StageClear Scene에서 나타낼 Scene 이름을 저장한다.
            PlayerPrefs.SetString("prevSceneName", SceneManager.GetActiveScene().name);

            // 스테이지 클리어 연출을 실행하고 StageClear Scene으로 이동한다.
            StartCoroutine(StageClearCoroutine());
        }
    }

    // 스테이지 클리어 연출을 나타내는 코루틴
    private IEnumerator StageClearCoroutine()
    {
        // 플레이어의 이동, 점프를 방지한다.
        playerController.RestrictMove();

        playerController.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
        yield return new WaitForSeconds(0.5f);
        playerController.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        
        // StageClear Scene으로 이동한다.
        SceneManager.LoadScene("StageClear");
    }
}
