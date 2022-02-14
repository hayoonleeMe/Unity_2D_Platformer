using UnityEngine;
using UnityEngine.UI;

public class ManageHeart : MonoBehaviour
{
    [SerializeField]
    private PlayerHP playerHP;

    // 하트 스프라이트의 배열, 순서대로 Full, Half, Empty 하트이다.
    [SerializeField]
    private Sprite[] heartSprites;

    // 하트 오브젝트의 프리팹, 기본 스프라이트는 Full 이다.
    [SerializeField]
    private Image heartPrefab;

    // 캔버스에 표시될 하트 오브젝트의 배열, 왼쪽부터 0번째이다.
    private Image[] hearts;

    // 하트 오브젝트 사이의 거리 , 31.8 + 5
    private float offsetX = 36.8f;

    private const int FULL = 0;
    private const int HALF = 1;
    private const int EMPTY = 2;

    void Start()
    {
        // 최대 체력만큼의 하트 오브젝트를 생성한다.
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

    // 플레이어의 HP 가 변경되면 호출되는 함수
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
