using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    // 스테이지의 정보를 담는 객체
    [SerializeField]
    private StageData stageData;

    // 플레이어의 상태를 체크하는 시간
    private const float CHECK_SECONDS = 0.07f;

    // 카메라의 사각형 범위의 x축 크기
    private float halfCameraRectXSize = 0.0f;

    // 배경 스프라이트를 이동시킬지 결정하는 범위를 정하는 변수
    private float offsetX = 5.0f;

    void Start()
    {
        halfCameraRectXSize = (Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f))).x -
                            (Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.5f, 0f))).x;

        StartCoroutine(CheckAlign());
    }

    // 배경 스프라이트가 이동해야 하는지 CHECK_SECONDS 마다 체크하는 코루틴 함수
    IEnumerator CheckAlign()
    {
        while (true)
        {
            float bgEdgeXRightPos = transform.position.x + transform.localScale.x / 2;
            float bdEdgeXLeftPos = transform.position.x - transform.localScale.x / 2;

            float cameraEdgeXRightPos = Camera.main.transform.position.x + halfCameraRectXSize;
            float cameraEdgeXLeftPos = Camera.main.transform.position.x - halfCameraRectXSize;

            // 플레이어가 오른쪽으로 이동하다가 배경 스프라이트를 옮기는 경우
            if (cameraEdgeXRightPos >= bgEdgeXRightPos - offsetX)
            {
                transform.position += new Vector3(transform.localScale.x / 2, 0, 0);
            }
            // 플레이어가 왼쪽으로 이동하다가 배경 스프라이트를 옮기는 경우
            else if (cameraEdgeXLeftPos <= bdEdgeXLeftPos + offsetX)
            {
                transform.position -= new Vector3(transform.localScale.x / 2, 0, 0);
            }

            yield return new WaitForSeconds(CHECK_SECONDS);
        }
    }
}
