using TMPro;
using UnityEngine;

public class ResultScoreViewer : MonoBehaviour
{
    private TextMeshProUGUI textScore;

    private void Awake()
    {
        textScore = GetComponent<TextMeshProUGUI>();

        textScore.text = "Score : " + PlayerPrefs.GetInt("score");
    }
}
