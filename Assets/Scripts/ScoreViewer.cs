using TMPro;
using UnityEngine;

public class FinalScoreViewer : MonoBehaviour
{
    private TextMeshProUGUI textScore;

    private void Awake()
    {
        textScore = GetComponent<TextMeshProUGUI>();

        textScore.text = "Score : " + PlayerPrefs.GetInt("score");
    }
}
