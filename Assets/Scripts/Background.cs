using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    // ���������� ������ ��� ��ü
    [SerializeField]
    private StageData stageData;

    // �÷��̾��� ���¸� üũ�ϴ� �ð�
    private const float CHECK_SECONDS = 0.07f;

    // ī�޶��� �簢�� ������ x�� ũ��
    private float halfCameraRectXSize = 0.0f;

    // ��� ��������Ʈ�� �̵���ų�� �����ϴ� ������ ���ϴ� ����
    private float offsetX = 5.0f;

    void Start()
    {
        halfCameraRectXSize = (Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f))).x -
                            (Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.5f, 0f))).x;

        StartCoroutine(CheckAlign());
    }

    // ��� ��������Ʈ�� �̵��ؾ� �ϴ��� CHECK_SECONDS ���� üũ�ϴ� �ڷ�ƾ �Լ�
    IEnumerator CheckAlign()
    {
        while (true)
        {
            float bgEdgeXRightPos = transform.position.x + transform.localScale.x / 2;
            float bdEdgeXLeftPos = transform.position.x - transform.localScale.x / 2;

            float cameraEdgeXRightPos = Camera.main.transform.position.x + halfCameraRectXSize;
            float cameraEdgeXLeftPos = Camera.main.transform.position.x - halfCameraRectXSize;

            // �÷��̾ ���������� �̵��ϴٰ� ��� ��������Ʈ�� �ű�� ���
            if (cameraEdgeXRightPos >= bgEdgeXRightPos - offsetX)
            {
                transform.position += new Vector3(transform.localScale.x / 2, 0, 0);
            }
            // �÷��̾ �������� �̵��ϴٰ� ��� ��������Ʈ�� �ű�� ���
            else if (cameraEdgeXLeftPos <= bdEdgeXLeftPos + offsetX)
            {
                transform.position -= new Vector3(transform.localScale.x / 2, 0, 0);
            }

            yield return new WaitForSeconds(CHECK_SECONDS);
        }
    }
}
