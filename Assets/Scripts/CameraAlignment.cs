using UnityEngine;

public class CameraAlignment : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private StageData stageData;

    // ī�޶� �̵����� �ʵ��� �����ϴ� �ּ� x��ǥ
    private float minLimitX;

    // ī�޶� �̵����� �ʵ��� �����ϴ� �ִ� x��ǥ
    private float maxLimitX;

    private void Awake()
    {
        float halfViewPortXSize = (Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f))).x - 
                                  (Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.5f, 0f))).x;
        minLimitX = stageData.LimitMin.x + halfViewPortXSize;
        maxLimitX = stageData.LimitMax.x - halfViewPortXSize;
    }

    private void LateUpdate()
    {
        // �÷��̾� ������Ʈ�� �̵��� �� ī�޶� �÷��̾��� ��ġ�� �̵���Ų��.
        Vector3 cameraPosition = transform.position;
        cameraPosition.x = playerTransform.position.x;
        cameraPosition.y = playerTransform.position.y;

        // ī�ް��� �ּ�, �ִ� x��ǥ�� ���� �ʵ��� �����Ͽ� �̵���Ų��.
        transform.position = new Vector3(Mathf.Clamp(cameraPosition.x, minLimitX, maxLimitX), cameraPosition.y, cameraPosition.z);
    }
}
