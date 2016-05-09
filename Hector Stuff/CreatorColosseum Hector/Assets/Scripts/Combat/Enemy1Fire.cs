using UnityEngine;
using System.Collections;

public class Enemy1Fire : MonoBehaviour {

    public float fireFreq;
    public GameObject enemyLaser;
    float lastShot;

	// Update is called once per frame
	void Update () {
        fireFreq = Random.Range(4f, 6f);
        if (Time.time > lastShot + fireFreq)
        {
            Fire();
        }
    }

    void Fire()
    {
        lastShot = Time.time;
        GameObject obj = EnemyFluttershyPooling.current.GetPooledObject();

        if (obj == null)
        {
            return;
        }

        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        obj.SetActive(true);
    }
}
