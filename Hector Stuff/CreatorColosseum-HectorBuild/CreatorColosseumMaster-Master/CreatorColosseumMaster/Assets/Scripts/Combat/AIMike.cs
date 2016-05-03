﻿using UnityEngine;
using System.Collections;

public class AIMike : MonoBehaviour {

	public float 	movingSpeed;
	private float	xPos;

	private int		randomY;
	public int		yMin;
	public int		yMax;
	public int		xBulletOffset = 3;

	public Vector2 destination;
		
	public bool		xReached;
	public bool		yReached = true;

	// Use this for initialization
	void Start () 
	{
		xPos = transform.position.x;
		destination = new Vector2 (transform.position.x, transform.position.y); //where he starts
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!yReached) 
		{
			MovePhase (destination);
		}
		else 
		{
			ShootPhase();
			destination = CreateDestination ();
		}
	
	}

//----------------CreateDestination-----------------
	Vector2 CreateDestination()
	{
		if (yReached) 
		{
			randomY = Random.Range (yMin, yMax);

			destination = new Vector2 (xPos, randomY);
			yReached = false;
		}

			return destination;
	}
	
//====================MOVING PHASE=====================
	void MovePhase(Vector2 destination)
	{
		//moving up and down towards destination
		if ((destination.y - 1) > transform.position.y)
		{
			transform.position += transform.up * movingSpeed * Time.deltaTime;
			yReached = false;
		}
		else if ((destination.y + 1) < transform.position.y)
		{
			transform.position += transform.up * -movingSpeed * Time.deltaTime;
			yReached = false;
		}
		else
		{
			transform.position += transform.up * 0;
			yReached = true;
		}
		
		//moving left and right towards destination
		if ((destination.x) > transform.position.x)
		{
			transform.position += transform.right * movingSpeed * Time.deltaTime;
			xReached = false;
		}
		else if ((destination.x) < transform.position.x)
		{
			transform.position += transform.right * -movingSpeed * Time.deltaTime;
			xReached = false;
		}
		else
		{
			transform.position += transform.right * 0;
			xReached = true;
		}
	}
//------------------ShootPhase()---------
	void ShootPhase()
	{
		GameObject obj = EnemyFluttershyPooling.current.GetPooledObject();
		
		if (obj == null)
		{
			return;
		}
		
		obj.transform.position = new Vector2 ((transform.position.x - xBulletOffset), transform.position.y);
		obj.transform.rotation = transform.rotation;
		obj.SetActive(true);
	}
}
