using UnityEngine;
using System.Collections;

public class AISmall : MonoBehaviour {

	static bool oneAttackPlayer;

	static Vector2[] VectorList;
	static bool[]	VectorBoolList;

	static public Collider2D _playerCollider;

	//-----validation that it is not outside clamping----
	static public float VectorMinX;
	static public float VectorMaxX;
	static public float VectorMinY;
	static public float VectorMaxY;

	//---clamping vars-----
	static public int xMin = -130;
	static public int xMax = 130;
	static public int yMin = -50;
	static public int yMax = -35;

	[HideInInspector]
	public float playerDistance;
	public float normalMovementSpeed;
	private float movingSpeed;

	public Vector2 destinationVector;

	//---range vars
	public int xRange = 1;
	public int yRange = 1;

	public float attackTimer;
	private float permentTimer;

	public Transform targetPlayer;

	public GameObject _player;
	public GameObject _camera;

	private Vector3 vectorDestination;

	public bool playerTouch = false;
	public bool forceAttack = false;
	public bool xReached = false;
	public bool yReached = false;

	// Use this for initialization
	void Start()
	{
		_playerCollider = _player.GetComponent<Collider2D>();
		movingSpeed = normalMovementSpeed;
		permentTimer = attackTimer;

		VectorBoolList = new bool [4]; //init the bool list
		
		for(int index = 0; index < VectorBoolList.Length; index ++)
		{
			VectorBoolList[index] = true;
		}

	}
	
	// Update is called once per frame
	void FixedUpdate()
	{
		//---------------clamping--------------
		transform.position = new Vector3 (Mathf.Clamp (transform.position.x, xMin, xMax), Mathf.Clamp (transform.position.y, yMin, yMax), 0);

		//---------------Resetting Attack Timer---------
		if (attackTimer > 0) 
		{
			attackTimer -= 1 * Time.deltaTime;
		}

		//------calc player Distance-------------
		playerDistance = Vector3.Distance (targetPlayer.position, transform.position);

		//--------figure out what phase based up playerdistance--------
		if (playerDistance < 10)
		{
			AttackPhase();
		} 
		else if (playerDistance < 20)
		{
			IdlePhase();
		} 
		else if (playerDistance < 50) 
		{
			MovePhase(_player.transform.position);
		}
		else 
		{
			//do nothing
		}
			
		
	}

	void AttackPhase()
	{
		oneAttackPlayer = true;
		
		print ("Attack Phase");
		if (playerTouch) 
		{
			if (attackTimer < 1)
			{
				if (_player.GetComponent<PlayerReceivesDamage> () != null) 
				{
					_player.GetComponent<PlayerReceivesDamage> ().meleeHits++;
					attackTimer = permentTimer;
				}
			}
		} 
		else
			MovePhase(_player.transform.position);
	}

	void IdlePhase()
	{
		print ("Idle");

		if (oneAttackPlayer) 
		{
			CalcVectors ();

			for (int index = 0; index < VectorList.Length; index ++) 
			{
				if (VectorBoolList [index]) 
				{
					//print (VectorBoolList [index]);
					destinationVector = VectorList [index];
					VectorBoolList [index] = false;

					index = VectorList.Length;
				}
			}

			if (!xReached && !yReached)
			{
				MovePhase (destinationVector);
			}
			else
			{
				//do nothing
			}
		} 
		else 
		{
			_camera.GetComponent<CheckForEnemies>().CheckWhoClosest();

			if (forceAttack)
			{
				AttackPhase();
			}
		}
	}
//---------------CalcVectors---------------
	static void CalcVectors()
	{
		VectorList = new Vector2[4];

		VectorMinX =  _playerCollider.bounds.min.x;
		VectorMinY =  _playerCollider.bounds.min.y;
		VectorMaxX =  _playerCollider.bounds.max.x;
		VectorMaxY =  _playerCollider.bounds.max.y;

		//----------validation that it is within clamping-----
		if (VectorMinX < xMin)
			VectorMinX = xMin;

		if (VectorMinY < yMin)
			VectorMinY = yMin;

		if (VectorMaxX > xMax)
			VectorMaxX = xMax;

		if (VectorMaxY > yMax)
			VectorMaxY = yMax;
		   	
		VectorList[0] = new Vector2((VectorMinX - 5), //how far left of the player
		                            VectorMaxY);

		VectorList[1] = new Vector2((VectorMaxX + 5), //how far right of the player
		                            VectorMaxY); 

		VectorList[2] = new Vector2((VectorMinX - 5), //how far left of the player
		                            VectorMinY); 

		VectorList[3] = new Vector2((VectorMaxX + 5), //how far right of the player
		                            VectorMinY); 

	}
//====================MOVING PHASE=====================
	void MovePhase(Vector2 destination)
	{
		print ("Move Phase");
		//moving up and down towards destination
		if (destination.y > transform.position.y)
		{
			transform.position += transform.up * movingSpeed * Time.deltaTime;
		}
		else if (destination.y < transform.position.y)
		{
			transform.position += transform.up * -movingSpeed * Time.deltaTime;
		}
		else
		{
			transform.position += transform.up * 0;
			xReached = true;
		}

		//moving left and right towards destination
		if (destination.x > transform.position.x)
		{
			transform.position += transform.right * movingSpeed * Time.deltaTime;
		}
		else if (destination.x < transform.position.x)
		{
			transform.position += transform.right * -movingSpeed * Time.deltaTime;
		}
		else
		{
			transform.position += transform.right * 0;
			yReached = true;
		}
	}

	void OnCollisionStay2D(Collision2D playerC)
	{
		if (playerC.gameObject.tag == "Player")
			playerTouch = true;
	}

	void OnCollisionExit2D(Collision2D playerC)
	{
		if (playerC.gameObject.tag == "Player")
			playerTouch = false;
	}
}