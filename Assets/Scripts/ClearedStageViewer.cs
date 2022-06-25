using TMPro;
using UnityEngine;

public class ClearedStageViewer: MonoBehaviour
{
    private TextMeshProUGUI textStage;

    private void Awake()
    {
        textStage = GetComponent<TextMeshProUGUI>();

        textStage.text = PlayerPrefs.GetString("prevSceneName") + " Clear!";
    }
}
