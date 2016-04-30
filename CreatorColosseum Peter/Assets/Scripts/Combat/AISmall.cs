using UnityEngine;
using System.Collections;

public class AISmall : MonoBehaviour {
	[HideInInspector]
	public float targetDistance;
	public float attackDistance;
	
	public float sprintDistance;
	
	public float normalMovementSpeed;
	public float sprintMovementSpeed;
	[HideInInspector]
	public float slowMovementSpeed;
	[HideInInspector]
	public float fastMovementSpeed;

	//---clamping vars-----
	public int xMin = -130;
	public int xMax = 130;
	public int yMin = -50;
	public int yMax = -35;

	//---range vars
	public int xRange = 1;
	public int yRange = 1;

	public float attackTimer;
	private float permentTimer;

	public Transform targetPlayer;

	public GameObject _player;
	private GameObject _enemy;
	
	[HideInInspector]
	public GameObject _destination;
	
	[HideInInspector]
	public Collider2D _collider;
	public Collider2D _ownCollider;
	
	private RaycastHit2D hit;
	
	//--------bools for direction-----
	public bool movingUp = true;
	public bool movingRight = true;
	
	public bool playerTouch = false;
	public bool enemyTouch = false;
	
	bool noDamage = true;
	public bool enemyNav = false;
	
	// Use this for initialization
	void Start()
	{
		fastMovementSpeed = sprintMovementSpeed;
		slowMovementSpeed = normalMovementSpeed;
		
		permentTimer = attackTimer;

	}
	
	// Update is called once per frame
	void FixedUpdate()
	{
		//---------------clamping--------------
		transform.position = new Vector3(Mathf.Clamp(transform.position.x, xMin, xMax), Mathf.Clamp(transform.position.y, yMin, yMax), 0);

		if (attackTimer > 0)
		{
			//print (attackTimer + " : AttackTimer");
			attackTimer -= 1 * Time.deltaTime;
		}
		
		targetDistance = Vector3.Distance(targetPlayer.position, transform.position);
		
		//-------Speed Changes Based Upon Distance------------
		if (targetDistance > sprintDistance)
		{
			normalMovementSpeed = fastMovementSpeed;
		}
		else
			normalMovementSpeed = slowMovementSpeed;
		
		//--------If Hit, always Chances------------
		if (noDamage)
		{
			if (this.gameObject.GetComponent<EnemiesReceiveDamage>().hp < this.gameObject.GetComponent<EnemiesReceiveDamage>().maxHp)
			{
				attackDistance = 1000;
				noDamage = false;
				//print ("I've Been Hit!");
			}
		}
		
		//------Moves Towards the Player-------------
		if (targetDistance < attackDistance)
		{
			if (playerTouch == false && enemyTouch == false)
			{
				MovingPhase(targetPlayer, normalMovementSpeed);
			}
		}
		//-------Enemy Moves Around Obstacle-----------
		if (playerTouch) 
		{ // prevents enemy from pushing the enemy touching the player
			enemyNav = false;
			enemyTouch = false;
		} 

	}
	
	void MovingPhase(Transform target, float movingSpeed)
	{
		if ((target.position.y - yRange)> transform.position.y)
		{
			transform.position += transform.up * movingSpeed * Time.deltaTime;
			movingUp = true;
		}
		else if ((target.position.y  + yRange) < transform.position.y)
		{
			transform.position += transform.up * -movingSpeed * Time.deltaTime;
			movingUp = false;
		}
		else
		{
			transform.position += transform.up * 0;
		}
		
		if ((target.position.x -xRange )> transform.position.x)
		{
			transform.position += transform.right * movingSpeed * Time.deltaTime;
			movingRight = true;
		}
		else if ((target.position.x + xRange) < transform.position.x)
		{
			transform.position += transform.right * -movingSpeed * Time.deltaTime;
			movingRight = false;
		}
		else
		{
			transform.position += transform.right * 0;
		}
	}
	
	void OnCollisionEnter2D(Collision2D playerC)
	{
		if (playerC.gameObject.tag == "Player")
		{
			playerTouch = true;
			enemyNav = false;
		}
		else if (playerC.gameObject.tag == "Enemy")
		{
			enemyTouch = true;
		}
	}

	void OnCollisionStay2D(Collision2D playerC)
	{
		if (playerC.gameObject.tag == "Player")
		{
			enemyTouch = false;
			_player = playerC.gameObject;
			this.gameObject.GetComponent<EnemiesReceiveDamage>().rb.mass = 5000;
			movingUp = false;
			movingRight = false;
			if (attackTimer < 1)
			{
				if (playerC.gameObject.GetComponent<PlayerReceivesDamage>() != null)
				{
					playerC.gameObject.GetComponent<PlayerReceivesDamage>().meleeHits++;
					attackTimer = permentTimer;
				}
			}
		}
	
		else if (playerC.gameObject.tag == "Enemy")
		{

		}
	}
	
	void OnCollisionExit2D(Collision2D playerC)
	{
		if (playerC.gameObject.tag == "Player")
		{
			playerTouch = false;
		}
		else if (playerC.gameObject.tag == "Enemy")
		{
			enemyTouch = false;
		}
	}


	void OnTriggerEnter2D(Collider2D _point)
	{
		if (_point.tag == "Point")
		{
			//print("you got there!");
			enemyNav = false;
			enemyTouch = false;
		}
	}
}