using TMPro;
using UnityEngine;

public class ScoreViewer : MonoBehaviour
{
    private TextMeshProUGUI textScore;

    private void Awake()
    {
        textScore = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateText(int score)
    {
        textScore.text = "Score " + score;
    }
}
