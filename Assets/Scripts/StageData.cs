using UnityEngine;

// 스테이지의 최소, 최대 x, y 좌표를 프로퍼티로 제공하는 클래스
// Unity Editor 의 Asset 으로 추가할 수 있다.
[CreateAssetMenu]
public class StageData : ScriptableObject
{
    // 맵의 최소 좌표
    [SerializeField]
    private Vector2 limitMin;

    // 맵의 최대 좌표
    [SerializeField]
    private Vector2 limitMax;

    // 다른 클래스에서 사용할 수 있는 프로퍼티
    public Vector2 LimitMin => limitMin;
    public Vector2 LimitMax => limitMax;
}
