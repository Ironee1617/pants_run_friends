using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            other.GetComponent<Transform>().localEulerAngles += new Vector3(0, 0, 180);
            other.GetComponent<Bullet>().reverse = true;
            DisplayEffect(other.transform);
            Manager_Fever.Instance.FeverGaugeDown();
        }
    }

    void DisplayEffect(Transform _transform)
    {
        GameObject effect = Manager_ObjectPool.Instance.PopFromPool(5);
        effect.transform.position = _transform.position;
        effect.SetActive(true);
        StartCoroutine(PushEffect(effect));
    }

    IEnumerator PushEffect(GameObject _effect)
    {
        yield return new WaitForSeconds(1f);
        Manager_ObjectPool.Instance.PushToPool(5, _effect);
    }
}
