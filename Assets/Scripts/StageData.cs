using UnityEngine;

// ���������� �ּ�, �ִ� x, y ��ǥ�� ������Ƽ�� �����ϴ� Ŭ����
// Unity Editor �� Asset ���� �߰��� �� �ִ�.
[CreateAssetMenu]
public class StageData : ScriptableObject
{
    // ���� �ּ� ��ǥ
    [SerializeField]
    private Vector2 limitMin;

    // ���� �ִ� ��ǥ
    [SerializeField]
    private Vector2 limitMax;

    // �ٸ� Ŭ�������� ����� �� �ִ� ������Ƽ
    public Vector2 LimitMin => limitMin;
    public Vector2 LimitMax => limitMax;
}
