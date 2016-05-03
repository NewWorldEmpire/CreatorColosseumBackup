﻿using UnityEngine;
using System.Collections;

public class CameraZoom : MonoBehaviour 
{
	public Transform target;
	public GameObject _player;
	public float zoom = - 125.0f;
	public float smooth = 2.0f;
	public float normal = - 100.0f;
	public float cameraRange = 10.0f;
	private float cameraSpeed = 1.5f;
	[HideInInspector]
	public float distance = 5.0f;
    public float minXClamp;
    public float maxXClamp;

    // Use this for initialization
    void Start () 
	{
		_player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
		//camera follow
		transform.position = Vector3.Lerp (transform.position, new Vector3 (target.position.x, transform.position.y, transform.position.z), Time.deltaTime * cameraSpeed);
		distance = Vector3.Distance (this.gameObject.transform.position, _player.transform.position);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minXClamp, maxXClamp), transform.position.y, transform.position.z);
    }
}
