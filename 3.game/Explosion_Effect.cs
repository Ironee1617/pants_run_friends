using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion_Effect : MonoBehaviour
{
    public int objectID;
    // Start is called before the first frame update
    private void OnEnable()
    {
        StartCoroutine(EffectOff());
    }

    IEnumerator EffectOff()
    {
        yield return new WaitForSeconds(1.5f);
        Manager_ObjectPool.Instance.PushToPool(objectID, this.gameObject);
    }
}
