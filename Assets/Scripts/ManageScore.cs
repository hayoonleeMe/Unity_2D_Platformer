using UnityEngine;

public class ManageScore : MonoBehaviour
{
    // 게임의 총 점수
    private int score;

    [SerializeField]
    private ScoreViewer scoreViewer;

    private void Awake()
    {
        score = 0;
    }

    // 점수를 업데이트한다.
    public void updateScore(int amount)
    {
        if (amount >= 0)
        {
            score = Mathf.Min(int.MaxValue, score + amount);
        }
        else
        {
            score = Mathf.Max(0, score - amount);
        }

        scoreViewer.UpdateText(score);
    }

    // 점수를 반환한다.
    public int getScore()
    {
        return score;
    }
}
