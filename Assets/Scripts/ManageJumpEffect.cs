using UnityEngine;

public class ManageJumpEffect: MonoBehaviour
{
    // 생성할 점프 이펙트의 프리팹
    [SerializeField]
    private GameObject jumpEffectPrefab;
    
    // 점프 이펙트의 위치를 설정하는데 사용하는 오프셋
    private float offsetY = -0.1f;

    // 플레이어가 점프할 수 있는 최대 횟수
    // PlayerController 스크립트의 Awake 에서 SetJumpCount 메소드를 통해 설정한다.
    private int maxJumpCount;

    // 점프 이펙트들을 담고있는 배열
    private GameObject[] jumpEffects;

    private void Start()
    {
        // 초기에 필요한 JumpEffect 를 생성한다.
        jumpEffects = new GameObject[maxJumpCount - 1];

        for (int i = 0; i < jumpEffects.Length; ++i)
        {
            jumpEffects[i] = Instantiate(jumpEffectPrefab, Vector3.zero, Quaternion.identity, transform);

            // 초기에는 비활성화한다.
            jumpEffects[i].SetActive(false);
        }
    }

    // 비활성화된(애니메이션을 실행중이지 않는) 점프 이펙트를 실행한다.
    public void PlayEffect(Vector3 playerPos)
    {
        Vector3 pos = playerPos;
        pos.y += offsetY;

        // 비활성화된 점프 이펙트가 없는 경우는 처리하지 않았다.
        foreach (GameObject jumpEffect in jumpEffects)
        {
            if (!jumpEffect.activeSelf)
            {
                jumpEffect.SetActive(true);
                jumpEffect.transform.position = pos;
                jumpEffect.GetComponent<JumpEffect>().PlayEffect();

                break;
            }
        }
    }

    // maxJumpCount 를 외부에서 초기화시킨다.
    public void SetJumpCount(int count)
    {
        maxJumpCount = count;
    }
}
