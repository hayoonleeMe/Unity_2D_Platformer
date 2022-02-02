using UnityEngine;

[CreateAssetMenu]
public class StageData : ScriptableObject
{
    [SerializeField]
    private Vector2 limitMin;
    [SerializeField]
    private Vector2 limitMax;

    // 다른 클래스에서 사용할 수 있는 프로퍼티
    public Vector2 LimitMin => limitMin;
    public Vector2 LimitMax => limitMax;
}
