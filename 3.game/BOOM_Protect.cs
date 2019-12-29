using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOOM_Protect : MonoBehaviour, BOOM
{
    public BOOM_Ordinary bomb;
    public Vector3 initPos;

    public void PushToPool()
    {
        GameObject effect = Manager_ObjectPool.Instance.PopFromPool(9);
        effect.transform.position = transform.position;
        effect.SetActive(true);

        Manager_ObjectPool.Instance.PushToPool(6, this.gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            other.GetComponent<Bullet>().PushToPool();
            Role();
        }
    }

    public void Role()
    {
        PushToPool();
        bomb.boxCollider.enabled = true;
        bomb.protector = null;
        bomb = null;
    }
}
