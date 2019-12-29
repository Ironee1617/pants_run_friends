using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Effect : MonoBehaviour {
    public GameObject impactParticle;
    public GameObject projectileParticle;
    public GameObject muzzleParticle;
    public GameObject[] trailParticles;
    [HideInInspector]
    public Vector3 impactNormal; //Used to rotate impactparticle.

    private bool hasCollided = false;
    private AudioSource sound;

    private void Awake()
    {
        projectileParticle = Instantiate(projectileParticle, transform) as GameObject;
        if (sound == null)
            sound = projectileParticle.GetComponent<AudioSource>();
        if (!Manager_Game.Instance.Sound)
            sound.mute = true;
        else
            sound.mute = false;
    }
    private void OnEnable()
    {
        if (sound == null)
            sound = projectileParticle.GetComponent<AudioSource>();
        if (!Manager_Game.Instance.Sound)
            sound.mute = true;
        else
            sound.mute = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Boom")
        {
            GameObject impactParticleObj = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;
            Destroy(impactParticleObj, 2f);

            //foreach (GameObject trail in trailParticles)
            //{
            //    GameObject curTrail = transform.Find(projectileParticle.name + "/" + trail.name).gameObject;
            //    print("curTrail " + curTrail);
            //    curTrail.transform.parent = null;
            //    Destroy(curTrail, 3f);
            //}

            //ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();
            //print("trails " + trails);
            ////Component at[0] is that of the parent i.e. this object(if there is any)
            //for (int i = 1; i < trails.Length; i++)
            //{

            //    ParticleSystem trail = trails[i];

            //    if (trail.gameObject.name.Contains("Trail"))
            //    {
            //        trail.transform.SetParent(null);
            //        Destroy(trail.gameObject, 2f);
            //    }
            //}
        }
    }
}
