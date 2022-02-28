using System.Collections;
using UnityEngine;

public class Background : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    // 스테이지의 정보를 담는 객체
    [SerializeField]
    private StageData stageData;

    // 플레이어의 상태를 체크하는 시간
    private const float CHECK_SECONDS = 0.07f;

    // 카메라 뷰포트의 x축 크기
    private float halfViewPortXSize = 0.0f;

    // 카메라 뷰포트의 y축 크기
    private float halfViewPortYSize = 0.0f;

    // 배경 스프라이트를 이동시킬지 결정하는 범위를 정하는 변수
    private float offset = 2.5f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        halfViewPortXSize = (Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f))).x -
                            (Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.5f, 0f))).x;

        halfViewPortYSize = (Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1.0f, 0f))).y -
                            (Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f))).y;

        StartCoroutine(CheckAlign());
    }

    // 배경 스프라이트가 이동해야 하는지 CHECK_SECONDS 마다 체크하는 코루틴 함수
    IEnumerator CheckAlign()
    {
        while (true)
        {
            float bgEdgeXRightPos = transform.position.x + spriteRenderer.bounds.size.x / 2;
            float bgEdgeXLeftPos = transform.position.x - spriteRenderer.bounds.size.x / 2;

            float cameraEdgeXRightPos = Camera.main.transform.position.x + halfViewPortXSize;
            float cameraEdgeXLeftPos = Camera.main.transform.position.x - halfViewPortXSize;

            // 플레이어가 오른쪽으로 이동하다가 배경 스프라이트를 옮기는 경우
            if (cameraEdgeXRightPos >= bgEdgeXRightPos - offset)
            {
                Vector3 pos = transform.position;
                transform.position = new Vector3(cameraEdgeXLeftPos + spriteRenderer.bounds.size.x / 2 - offset * 2, pos.y, pos.z);
            }
            // 플레이어가 왼쪽으로 이동하다가 배경 스프라이트를 옮기는 경우
            else if (cameraEdgeXLeftPos <= bgEdgeXLeftPos + offset)
            {
                Vector3 pos = transform.position;
                transform.position = new Vector3(cameraEdgeXRightPos - spriteRenderer.bounds.size.x / 2 + offset * 2, pos.y, pos.z);
            }

            float bgEdgeYUpPos = transform.position.y + spriteRenderer.bounds.size.y / 2;
            float bgEdgeYDownPos = transform.position.y - spriteRenderer.bounds.size.y / 2;

            float cameraEdgeYUpPos = Camera.main.transform.position.y + halfViewPortYSize;
            float cameraEdgeYDownPos = Camera.main.transform.position.y - halfViewPortYSize;

            // 플레이어가 위쪽으로 이동하다가 배경 스프라이트를 옮기는 경우
            if (cameraEdgeYUpPos >= bgEdgeYUpPos - offset)
            {
                Vector3 pos = transform.position;
                transform.position = new Vector3(pos.x, cameraEdgeYDownPos + spriteRenderer.bounds.size.y / 2 - offset * 2, pos.z);
            }
            // 플레이어가 아래쪽으로 이동하다가 배경 스프라이트를 옮기는 경우
            else if (cameraEdgeYDownPos <= bgEdgeYDownPos + offset)
            {
                Vector3 pos = transform.position;
                transform.position = new Vector3(pos.x, cameraEdgeYUpPos - spriteRenderer.bounds.size.y / 2 + offset * 2, pos.z);
            }

            yield return new WaitForSeconds(CHECK_SECONDS);
        }
    }
}
