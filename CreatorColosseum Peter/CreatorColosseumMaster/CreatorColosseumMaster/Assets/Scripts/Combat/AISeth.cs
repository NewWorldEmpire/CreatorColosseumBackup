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
	public GameObject _level;

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

		if (this.gameObject.GetComponent<EnemiesReceiveDamage>().hp <= 0 && !isDead) 
		{
			_level.GetComponent<Transitions>().levelSelect ++;
            isDead = true;
		}
	}

//------------attackphase()----------
	void AttackPhase()
	{
		if (!grabPlayerY) 
		{
			if (transform.position.x > _player.transform.position.x) 
			{
				destination = new Vector2 (xMin - 20, transform.position.y);
				resetPoint = xMin + 20;
			}
			else 
			{
				destination = new Vector2 (xMax + 20, transform.position.y);
				resetPoint = xMax - 20;
			}

			grabPlayerY = true;
		}

		MovePhase (destination, chargeSpeed);

		if (yReached && xReached) 
		{
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
            //reset poistive, left, reset negative, right
            if (resetPoint > 0)
            {
                GetComponent<Animator>().SetBool("SethLeft", true);
                GetComponent<Animator>().SetBool("SethRight", false);
            }
            else if (resetPoint < 0)
            {
                GetComponent<Animator>().SetBool("SethRight", true);
                GetComponent<Animator>().SetBool("SethLeft", false);
            }

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

			if (armor < chargeDamage)
			{
				_player.GetComponent<CombatScript> ().health -= (chargeDamage - armor);
			}

		}
	}
}
