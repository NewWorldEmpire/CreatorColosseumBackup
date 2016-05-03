using UnityEngine;
using System.Collections;

public class AISeth : MonoBehaviour
{
    public bool isDead;
    public GameObject cam;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (this.gameObject.GetComponent<EnemiesReceiveDamage>().hp < 0 && !isDead)
        {
            isDead = true;
            cam.GetComponent<Transitions>().Died();
        }
    }
}
