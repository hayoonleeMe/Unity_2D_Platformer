using System.Collections;
using UnityEngine;

public class Background : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    // ���������� ������ ��� ��ü
    [SerializeField]
    private StageData stageData;

    // �÷��̾��� ���¸� üũ�ϴ� �ð�
    private const float CHECK_SECONDS = 0.07f;

    // ī�޶� ����Ʈ�� x�� ũ��
    private float halfViewPortXSize = 0.0f;

    // ī�޶� ����Ʈ�� y�� ũ��
    private float halfViewPortYSize = 0.0f;

    // ��� ��������Ʈ�� �̵���ų�� �����ϴ� ������ ���ϴ� ����
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

    // ��� ��������Ʈ�� �̵��ؾ� �ϴ��� CHECK_SECONDS ���� üũ�ϴ� �ڷ�ƾ �Լ�
    IEnumerator CheckAlign()
    {
        while (true)
        {
            float bgEdgeXRightPos = transform.position.x + spriteRenderer.bounds.size.x / 2;
            float bgEdgeXLeftPos = transform.position.x - spriteRenderer.bounds.size.x / 2;

            float cameraEdgeXRightPos = Camera.main.transform.position.x + halfViewPortXSize;
            float cameraEdgeXLeftPos = Camera.main.transform.position.x - halfViewPortXSize;

            // �÷��̾ ���������� �̵��ϴٰ� ��� ��������Ʈ�� �ű�� ���
            if (cameraEdgeXRightPos >= bgEdgeXRightPos - offset)
            {
                Vector3 pos = transform.position;
                transform.position = new Vector3(cameraEdgeXLeftPos + spriteRenderer.bounds.size.x / 2 - offset * 2, pos.y, pos.z);
            }
            // �÷��̾ �������� �̵��ϴٰ� ��� ��������Ʈ�� �ű�� ���
            else if (cameraEdgeXLeftPos <= bgEdgeXLeftPos + offset)
            {
                Vector3 pos = transform.position;
                transform.position = new Vector3(cameraEdgeXRightPos - spriteRenderer.bounds.size.x / 2 + offset * 2, pos.y, pos.z);
            }

            float bgEdgeYUpPos = transform.position.y + spriteRenderer.bounds.size.y / 2;
            float bgEdgeYDownPos = transform.position.y - spriteRenderer.bounds.size.y / 2;

            float cameraEdgeYUpPos = Camera.main.transform.position.y + halfViewPortYSize;
            float cameraEdgeYDownPos = Camera.main.transform.position.y - halfViewPortYSize;

            // �÷��̾ �������� �̵��ϴٰ� ��� ��������Ʈ�� �ű�� ���
            if (cameraEdgeYUpPos >= bgEdgeYUpPos - offset)
            {
                Vector3 pos = transform.position;
                transform.position = new Vector3(pos.x, cameraEdgeYDownPos + spriteRenderer.bounds.size.y / 2 - offset * 2, pos.z);
            }
            // �÷��̾ �Ʒ������� �̵��ϴٰ� ��� ��������Ʈ�� �ű�� ���
            else if (cameraEdgeYDownPos <= bgEdgeYDownPos + offset)
            {
                Vector3 pos = transform.position;
                transform.position = new Vector3(pos.x, cameraEdgeYUpPos - spriteRenderer.bounds.size.y / 2 + offset * 2, pos.z);
            }

            yield return new WaitForSeconds(CHECK_SECONDS);
        }
    }
}
