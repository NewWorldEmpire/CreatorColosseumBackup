using UnityEngine;
using System.Collections;

public class IceScript : MonoBehaviour {

    public float waitTime;

    void Start()
    {
        StartCoroutine(Wait(waitTime));
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.2f);
    }

    IEnumerator Wait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
