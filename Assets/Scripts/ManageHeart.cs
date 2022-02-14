using UnityEngine;
using UnityEngine.UI;

public class ManageHeart : MonoBehaviour
{
    [SerializeField]
    private PlayerHP playerHP;

    // ��Ʈ ��������Ʈ�� �迭, ������� Full, Half, Empty ��Ʈ�̴�.
    [SerializeField]
    private Sprite[] heartSprites;

    // ��Ʈ ������Ʈ�� ������, �⺻ ��������Ʈ�� Full �̴�.
    [SerializeField]
    private Image heartPrefab;

    // ĵ������ ǥ�õ� ��Ʈ ������Ʈ�� �迭, ���ʺ��� 0��°�̴�.
    private Image[] hearts;

    // ��Ʈ ������Ʈ ������ �Ÿ� , 31.8 + 5
    private float offsetX = 36.8f;

    private const int FULL = 0;
    private const int HALF = 1;
    private const int EMPTY = 2;

    void Start()
    {
        // �ִ� ü�¸�ŭ�� ��Ʈ ������Ʈ�� �����Ѵ�.
        hearts = new Image[(int)playerHP.MaxHP];

        for (int i = 0; i < hearts.Length; ++i)
        {
            hearts[i] = Instantiate(heartPrefab, Vector3.zero, Quaternion.identity, transform);
            hearts[i].rectTransform.anchorMin = new Vector2(0.0f, 1.0f);
            hearts[i].rectTransform.anchorMax = new Vector2(0.0f, 1.0f);
            hearts[i].rectTransform.pivot = new Vector2(0.0f, 1.0f);
            hearts[i].rectTransform.anchoredPosition = new Vector3(15.0f + offsetX * i, -15.0f, 0.0f);
        }
    }

    // �÷��̾��� HP �� ����Ǹ� ȣ��Ǵ� �Լ�
    public void ApplyHeart(float damage)
    {
        if (damage == 0)
        {
            return;
        }    

        for (int i = (int)playerHP.CurrentHP - 1; i >= (int)playerHP.CurrentHP - damage; --i)
        {
            hearts[i].sprite = heartSprites[EMPTY];
        }
    }
}
