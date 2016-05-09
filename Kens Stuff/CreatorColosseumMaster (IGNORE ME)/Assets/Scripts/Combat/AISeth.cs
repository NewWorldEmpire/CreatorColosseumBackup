using UnityEngine;
using System.Collections;

public class AISeth : MonoBehaviour {

	public int resetSpeed;
	public int chargeSpeed;

	public int chargeDamage;

	public float waitTime;
	private float wait;

	public int xMin;
	public int xMax;
	private int armor;

	public int yRange = 2;
	public int xRange = 2;

	public Vector2 destination;
	public int resetPoint;

	public GameObject _player;

	public bool xReached;
	public bool yReached;
	public bool isDead;
	public bool grabPlayerY;

	public bool isReset = true;
	public bool isAttack;

	// Use this for initialization
	void Start () 
	{
		isReset = true;
		resetPoint =  xMax - 20;
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
		if (!grabPlayerY) 
		{
			if (transform.position.x > _player.transform.position.x) 
			{
				//print ("GO LEFT: " + grabPlayerY);
				destination = new Vector2 (xMin - 20, transform.position.y);
				resetPoint = xMin + 20;
			}
			else 
			{
				//print ("GO RIGHT: " + grabPlayerY);
				destination = new Vector2 (xMax + 20, transform.position.y);
				resetPoint = xMax - 20;
			}

			grabPlayerY = true;
		}

		MovePhase (destination, chargeSpeed);

		if (yReached && xReached) 
		{
			//print ("Hello!");
			isAttack = false;
			isReset = true;
			grabPlayerY = false;
		}
	}

//---------------ResetPhase()--------------
	void ResetPhase()
	{
		if (!grabPlayerY) 
		{
			destination = new Vector2 (resetPoint, _player.transform.position.y);
			grabPlayerY = true;
			wait = Time.time;
		}
		
		MovePhase (destination, resetSpeed);

		//print (wait + ": wait");
		//print (Time.time + ": time");

		if ((Time.time - wait) > (waitTime + 3.5)) //3.5 to compinsiate for getting to point
		{
			if (yReached && xReached) 
			{
//				print ("Hello!");
				isAttack = true;
				isReset = false;
				grabPlayerY = false;
			}
		}
	}


//====================MOVING PHASE=====================
	void MovePhase(Vector2 destination, int movingSpeed)
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
	void OnCollisionStay2D(Collision2D playerC)
	{
		if (isAttack) 
		{ 
			armor = _player.GetComponent<CombatScript> ().armor;
			_player.GetComponent<PlayerReceivesDamage> ().InitiateCBT (chargeDamage.ToString ()).GetComponent<Animator> ().SetTrigger ("Hit"); //changed playerReceivesDamge
			_player.GetComponent<CombatScript> ().health -= (chargeDamage - armor);
		}
	}
}
