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
        float halfCameraRectXSize = (Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f))).x - 
                                  (Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.5f, 0f))).x;

        float halfCameraRectYSize = (Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1.0f, 0f))).y -
                                  (Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f))).y;

        minLimitX = stageData.LimitMin.x + halfCameraRectXSize;
        maxLimitX = stageData.LimitMax.x - halfCameraRectXSize;

        minLimitY = stageData.LimitMin.y + halfCameraRectYSize;
        maxLimitY = stageData.LimitMax.y - halfCameraRectYSize;
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
