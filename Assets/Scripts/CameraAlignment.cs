using UnityEngine;

public class CameraAlignment : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private StageData stageData;

    [SerializeField]
    private float offsetY = 2.0f;

    // ī�޶� �̵����� �ʵ��� �����ϴ� �ּ�, �ִ� x��ǥ
    private float minLimitX, maxLimitX;

    // ī�޶� �̵����� �ʵ��� �����ϴ� �ּ�, �ִ� y��ǥ
    private float minLimitY, maxLimitY;

    private void Awake()
    {
        float halfViewPortXSize = (Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f))).x - 
                                  (Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.5f, 0f))).x;

        float halfViewPortYSize = (Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1.0f, 0f))).y -
                                  (Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f))).y;

        minLimitX = stageData.LimitMin.x + halfViewPortXSize;
        maxLimitX = stageData.LimitMax.x - halfViewPortXSize;

        minLimitY = stageData.LimitMin.y + halfViewPortYSize;
        maxLimitY = stageData.LimitMax.y - halfViewPortYSize;
    }

    private void LateUpdate()
    {
        // �÷��̾� ������Ʈ�� �̵��� �� ī�޶� �÷��̾��� ��ġ�� �̵���Ų��.
        Vector3 cameraPosition = transform.position;
        cameraPosition.x = playerTransform.position.x;
        cameraPosition.y = playerTransform.position.y + offsetY;

        // ī�ް��� �ּ�, �ִ� x, y��ǥ�� ���� �ʵ��� �����Ͽ� �̵���Ų��.
        transform.position = new Vector3(Mathf.Clamp(cameraPosition.x, minLimitX, maxLimitX), 
                                         Mathf.Clamp(cameraPosition.y, minLimitY, maxLimitY), cameraPosition.z);
    }
}
