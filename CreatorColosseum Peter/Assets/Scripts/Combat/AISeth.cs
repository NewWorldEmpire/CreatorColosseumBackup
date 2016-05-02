using UnityEngine;
using System.Collections;

public class AISeth : MonoBehaviour {

	public int movingSpeed;

	public float waitTime;
	private float wait;

	public int xMin;
	public int xMax;

	public int yRange = 2;
	public int xRange = 2;

	public Vector2 destination;
	public int resetPoint;

	public GameObject _player;

	public bool xReached;
	public bool yReached;
	public bool isDead;

	public bool isReset = true;
	public bool isAttack;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//waitTime = Time.time;
		if (isReset)
		{
			ResetPhase ();
		}
		if (isAttack) 
		{
			AttackPhase ();
		}
	}

//------------attackphase()----------
	void AttackPhase()
	{
		if (transform.position.x > _player.transform.position.x) 
		{
			destination = new Vector2 (xMin - 20, transform.position.y);
			MovePhase (destination);
			resetPoint = xMin + 20;
		}
		else 
		{
			destination = new Vector2 (xMax + 20, transform.position.y);
			MovePhase (destination);
			resetPoint = xMax - 20;
		}

		if (yReached && xReached) 
		{
			isAttack = false;
			isReset = true;
		}
	}

//---------------ResetPhase()--------------
	void ResetPhase()
	{
		wait = Time.time + waitTime;
		destination = new Vector2 (resetPoint, _player.transform.position.y);
		MovePhase (destination);

		if (Time.time > wait) 
		{
			if (yReached && xReached) 
			{
				isAttack = true;
				isReset = false;
			}
		}
	}


//====================MOVING PHASE=====================
	void MovePhase(Vector2 destination)
	{
		//moving up and down towards destination
		if ((destination.y - yRange) > transform.position.y)
		{
			transform.position += transform.up * movingSpeed * Time.deltaTime;
			yReached = false;
		}
		else if ((destination.y + yRange) < transform.position.y)
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
		if ((destination.x - xRange) > transform.position.x)
		{
			transform.position += transform.right * movingSpeed * Time.deltaTime;
			xReached = false;
		}
		else if ((destination.x + xRange) < transform.position.x)
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
}
