using UnityEngine;

public class CameraAlignment : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private StageData stageData;

    // 카메라가 이동하지 않도록 제한하는 최소 x좌표
    private float minLimitX;

    // 카메라가 이동하지 않도록 제한하는 최대 x좌표
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
        // 플레이어 오브젝트가 이동한 후 카메라를 플레이어의 위치로 이동시킨다.
        Vector3 cameraPosition = transform.position;
        cameraPosition.x = playerTransform.position.x;
        cameraPosition.y = playerTransform.position.y;

        // 카메가가 최소, 최대 x좌표를 넘지 않도록 조정하여 이동시킨다.
        transform.position = new Vector3(Mathf.Clamp(cameraPosition.x, minLimitX, maxLimitX), cameraPosition.y, cameraPosition.z);
    }
}
