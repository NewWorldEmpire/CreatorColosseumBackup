using UnityEngine;
using System.Collections;

public class PetersHead : MonoBehaviour {
	public float smooth = 5.0f;
	public float tiltAngle = 30.0f;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		float tiltAroundZ = Input.GetAxis ("Horizontal") * tiltAngle;
		Quaternion target = Quaternion.Euler (0, 0, tiltAroundZ);
		transform.rotation = Quaternion.Slerp (transform.rotation, target, Time.deltaTime * smooth);
	}
}
