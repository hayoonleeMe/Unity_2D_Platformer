using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject slimePrefab;

    private Vector2 pos1 = new Vector2(21.0f, -0.56f);
    private Vector2 pos2 = new Vector2(26.0f, -0.56f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            // 적 생성 
            Instantiate(slimePrefab, pos1, Quaternion.identity);
            Instantiate(slimePrefab, pos2, Quaternion.identity);
        }
    }
}