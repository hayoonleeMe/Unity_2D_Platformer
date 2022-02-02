using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAlignment : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private StageData stageData;

    private float minLimitX, maxLimitX;

    private void Awake()
    {
        float halfViewPortXSize = (Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f))).x - 
                                  (Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.5f, 0f))).x;
        minLimitX = stageData.LimitMin.x + halfViewPortXSize;
        maxLimitX = stageData.LimitMax.x - halfViewPortXSize;
    }

    private void Update()
    {
        //Debug.Log(worldPoint.x);
    }

    // Update is called once per frame
    private void LateUpdate()
    {


        Vector3 cameraPosition = transform.position;
        // ī�޶� �÷��̾��� ��ġ�� �̵�
        cameraPosition.x = playerTransform.position.x;
        cameraPosition.y = playerTransform.position.y;
        transform.position = new Vector3(Mathf.Clamp(cameraPosition.x, minLimitX, maxLimitX), cameraPosition.y, cameraPosition.z);

        // �̵������� ���������� ���� ī�޶��� ������ �ɸ����� Ȯ��
        //minX = (Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.5f, 0f))).x;
        //maxX = (Camera.main.ViewportToWorldPoint(new Vector3(1f, 0.5f, 0f))).x;

        //if (minX < stageData.LimitMin.x || maxX > stageData.LimitMax.x)
        //{
        //    transform.position = new Vector3(originalCamera.x, transform.position.y, transform.position.z);
        //}
    }
}
