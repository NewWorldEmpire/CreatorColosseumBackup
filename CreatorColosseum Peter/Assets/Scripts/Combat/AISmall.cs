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
	public Transform targetObject;
	public Transform targetRayCast;

	private Vector3 vectorDestination;

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
			_enemy = playerC.gameObject;
			
			if (_enemy.GetComponent<AISmall>().movingUp)
			{
				enemyNav = true;
				// print("he's going up!");
				
				hit = RayLeft(targetRayCast); //check left
				
				if (hit.collider == null)
				{
					//print("move to the left!");
					targetObject.position = vectorDestination;
				}
				else
				{
					hit = RayRight(targetRayCast); //check right
					
					if (hit.collider == null)
					{
						//print("move to the right!");
						targetObject.position = vectorDestination;
					}
					else
					{
						hit = RayUp(targetRayCast); //check up
						if (hit.collider == null)
						{
							// print("move to the up!");
							targetObject.position = vectorDestination;
						}
						else
						{
							hit = RayDown(targetRayCast); //check down
							if (hit.collider == null)
							{
								//print("move to the down!");
								targetObject.position = vectorDestination;
							}
							else
							{
								//print("HELP I'M TRAPPED!");
								enemyNav = false;
							}
						}
					}
				}
				
			}
			
			else if (_enemy.GetComponent<AISmall>().movingRight)
			{
				enemyNav = true;
				//print("he's going right!");
				
				hit = RayUp(targetRayCast); //check up
				if (hit.collider == null)
				{
					//print("move to the up!");
					targetObject.position = vectorDestination;
				}
				else
				{
					hit = RayDown(targetRayCast); //check down
					
					if (hit.collider == null)
					{
						//print("move to the down!");
						targetObject.position = vectorDestination;
					}
					else
					{
						
						hit = RayLeft(targetRayCast); //check left
						
						if (hit.collider == null)
						{
							//print("move to the left!");
							targetObject.position = vectorDestination;
						}
						else
						{
							hit = RayRight(targetRayCast); // check right
							
							if (hit.collider == null)
							{
								//print("move to the right!");
								targetObject.position = vectorDestination;
							}
							else
							{
								// print("HELP I'M TRAPPED~");
								enemyNav = false;
							}
						}
					}
				}
			}
			else //playerTouch = true
			{
				// print("This guy's attacking....");
				
				_collider = playerC.collider;
				
				if (_collider.bounds.max.x <= _ownCollider.bounds.min.x || _collider.bounds.min.x > _ownCollider.bounds.max.x) //on the left or right side
				{
					//numHits++;
					vectorDestination = new Vector3(_ownCollider.bounds.center.x, (_ownCollider.bounds.center.y + 6), 0);
					playerC.gameObject.GetComponent<AISmall>().targetObject.position = vectorDestination;
					// print("We're going around!");
					enemyNav = true;
				}
				else
				{
					//numHits++;
					vectorDestination = new Vector3((_ownCollider.bounds.center.x + 6), _ownCollider.bounds.center.y, 0);
					targetObject.position = vectorDestination;
					//print("We're going up!");
					enemyNav = true;
				}
			}
			}
		}
		RaycastHit2D RayLeft(Transform targetRayCast)
		{
			targetRayCast.transform.localPosition = new Vector3(-1, 0, 0);
			hit = Physics2D.Raycast(targetRayCast.transform.position, -Vector2.right, 2); //check if left is open
			//hit = Physics2D.BoxCast (targetRayCast.transform.position, new Vector2 (_ownCollider.bounds.min.x - 3, _ownCollider.bounds.min.y), 0, -Vector2.right);
			Debug.DrawRay(targetRayCast.transform.position, -Vector2.right, Color.green, 2);
			
			vectorDestination = new Vector3((_ownCollider.bounds.min.x - 3), _ownCollider.bounds.center.y, 0);
			//numHits++;
			return hit;
		}
		
		RaycastHit2D RayRight(Transform targetRayCast)
		{
			targetRayCast.transform.localPosition = new Vector3(1, 0, 0);
			hit = Physics2D.Raycast(targetRayCast.transform.position, Vector2.right, 2); //check if right is open
			Debug.DrawRay(targetRayCast.transform.position, Vector2.right, Color.red, 2);
			
			vectorDestination = new Vector3((_ownCollider.bounds.min.x + 3), _ownCollider.bounds.center.y, 0);
			
			//numHits++;
			return hit;
		}
		
		RaycastHit2D RayUp(Transform targetRayCast)
		{
			targetRayCast.transform.localPosition = new Vector3(0, 2, 0);
			hit = Physics2D.Raycast(targetRayCast.transform.position, Vector2.up, 2); //check if up is open
			Debug.DrawRay(targetRayCast.transform.position, Vector2.up, Color.black, 2);
			
			vectorDestination = new Vector3(_ownCollider.bounds.min.x, (_ownCollider.bounds.center.y + 4), 0);
			//numHits++;
			return hit;
		}
		
		RaycastHit2D RayDown(Transform targetRayCast)
		{
			targetRayCast.transform.localPosition = new Vector3(0, -2, 0);
			hit = Physics2D.Raycast(targetRayCast.transform.position, -Vector2.up, 2); //check if down is open
			Debug.DrawRay(targetRayCast.transform.position, -Vector2.up, Color.yellow, 2);
			
			vectorDestination = new Vector3(_ownCollider.bounds.min.x, (_ownCollider.bounds.center.y - 4), 0);
			//numHits++;
			return hit;
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