using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHP : MonoBehaviour
{
    // ĵ����
    [SerializeField]
    private ManageHeart manageHeart;

    // �÷��̾��� �ִ� ü��
    [SerializeField]
    private float maxHP = 3.0f;

    // maxHP �� ������Ƽ (only get)
    public float MaxHP => maxHP;

    // �÷��̾��� ���� ü��
    private float currentHP;

    // currentHP �� ������Ƽ (set, get)
    public float CurrentHP
    {
        set => currentHP = Mathf.Clamp(value, 0, maxHP);
        get => currentHP;
    }

    // Start is called before the first frame update
    void Awake()
    {
        currentHP = maxHP;
    }

    // damage ��ŭ �÷��̾��� ü���� �϶��Ѵ�.
    public void TakeDamage(float damage)
    {
        manageHeart.ApplyHeart(damage);
        currentHP -= damage;

        if (currentHP <= 0)
        {
            OnDie();
        }
    }

    private void OnDie()
    {
        Debug.Log("Player is Die");
    }
}
