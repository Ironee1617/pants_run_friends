using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public bool reverse;
    public void Shot()
    {
        StartCoroutine(Fire());
    }

    IEnumerator Fire()
    {
        while (true)
        {
            if(!reverse)
                transform.Translate(Vector3.forward * Time.deltaTime * 11, Space.World);
            else
                transform.Translate(Vector3.back * Time.deltaTime * 11, Space.World);
            yield return null;
        }
    }

    public void PushToPool()
    {
        reverse = false;
        Manager_ObjectPool.Instance.PushToPool(1, this.gameObject);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("OutZone"))
        {
            PushToPool();
        }
    }
}
