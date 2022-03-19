using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpEffectAnimationDestroyer : MonoBehaviour
{
    private void OnEffectEnd()
    {
        Destroy(gameObject);
    }
}
