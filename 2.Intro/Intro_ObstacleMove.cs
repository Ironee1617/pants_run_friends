using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro_ObstacleMove : MonoBehaviour
{
    Coroutine cor;

    void Start()
    {
        cor = StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while(true)
        {
            transform.Translate(Vector3.forward * 4 * Time.deltaTime, Space.World);

            yield return null;
        }
    }

    //public void StopMove() => StopCoroutine(cor);
}
