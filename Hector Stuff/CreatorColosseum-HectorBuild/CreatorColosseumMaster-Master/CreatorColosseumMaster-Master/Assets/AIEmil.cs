using UnityEngine;
using System.Collections;

public class AIEmil : MonoBehaviour
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
            print(isDead + "Emil");
           cam.GetComponent<Transitions>().Died();
        }

    }
}
