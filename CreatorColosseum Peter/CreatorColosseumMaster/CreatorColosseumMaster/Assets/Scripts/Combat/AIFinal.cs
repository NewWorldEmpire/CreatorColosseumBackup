using UnityEngine;
using System.Collections;

public class AIFinal : MonoBehaviour {

	public GameObject _player;
	public GameObject spawnPoint;

	//---------------Mike Vars-------------
	public float 	mikePhaseSpeed;
	public float	mikeFireFreq;

	private float	xPos;
	private float 	lastShot;
	
	private int		randomY;
	public int		yMin;
	public int		yMax;

	private Vector2 destination;

	//---------------Seth Vars-------------
	public int sethResetSpeed;
	public int sethChargeSpeed;
	private int resetPoint;
	
	public int chargeDamage;

	public float sethWaitTime;
	private float wait;
	
	public int xMin;
	public int xMax;
	private int armor;
	
	private int yRange = 2;
	private int xRange = 2;

	bool grabPlayerY;
	
	public bool isReset = true;
	public bool isAttack;

	//-------------Emil Vars------------
	public int		topDivider;
	public int		bottomDivider;
	int		laserNum = 1;

	public float	emilResetSpeed;
	
		//---------small laser--------
	public float	smallLaserDelay;
	public float	smallPositionShootDelay = .5f;
	public float   	smallLaserDuration	= .3f;
	
		//----------big laser--------
	public float	bigLaserDelay;
	public float	bigPositionShootDelay = .5f;
	public float   	bigLaserDuration	= .3f;

	Vector2  playerPosition;
	Vector2	facePosition;
	Vector2 tempVector;
	Vector2  bigLaserPoint = new Vector2 (-120, -45);

	public GameObject emilResetPoint;
	public GameObject 	_laser;
	public GameObject	_face;
	
	public Collider2D	smallLaserCollider;
	public Collider2D	bigLaserCollider;

	private bool grabTime;
	private bool grabPosition;

	//-----------shared bools-------------
	
	public bool		xReached;
	public bool		yReached = true;
	private bool 	isMikeStart	 = true;
	private bool 	isSethStart	 = true;
	private bool	isEmilStart  = true;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (this.gameObject.GetComponent<EnemiesReceiveDamage> ().hp > 
			(this.gameObject.GetComponent<EnemiesReceiveDamage> ().maxHp * .667)) 
		{
			if (isMikeStart)
				MikeStart();
			else
				MikePhase();
		} 
		else if (this.gameObject.GetComponent<EnemiesReceiveDamage> ().hp > 
			(this.gameObject.GetComponent<EnemiesReceiveDamage> ().maxHp * .333)) 
		{
			if (isSethStart)
				SethStart();
			else
				SethPhase();
		} 
		else 
		{
			if (isEmilStart)
				EmilStart();
			else
				EmilPhase();
		}
	}

//=================Mike Phase========================================
	//==========================================================
	void MikeStart()
	{
		xPos = transform.position.x;
		destination = new Vector2 (transform.position.x, transform.position.y); //where he starts
		isMikeStart = false;
	}
	void MikePhase()
	{
		if (!yReached) 
		{
			MovePhase (destination, mikePhaseSpeed);
		}
		else 
		{
			if (Time.time > lastShot + mikeFireFreq)
			{
				ShootPhase();
				destination = CreateDestination ();
			}
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

	//------------------ShootPhase()---------
	void ShootPhase()
	{
		lastShot = Time.time;
		
		GameObject obj = EnemyFinalPooling.current.GetPooledObject ();
		
		if (obj == null) 
		{
			return;
		}
		
		obj.transform.position = spawnPoint.transform.position;
		obj.transform.rotation = transform.rotation;
		obj.SetActive (true);
		
	}
//=========================SETH PHASE===================================
	void SethStart()
	{
		isReset = true;
		resetPoint =  xMax - 20;
		isSethStart = false;
	}

	void SethPhase()
	{
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
				print ("GO LEFT: " + grabPlayerY);
				destination = new Vector2 (xMin - 20, transform.position.y);
				resetPoint = xMin + 20;
			}
			else 
			{
				print ("GO RIGHT: " + grabPlayerY);
				destination = new Vector2 (xMax + 20, transform.position.y);
				resetPoint = xMax - 20;
			}
			
			grabPlayerY = true;
		}
		
		MovePhase (destination, sethChargeSpeed);
		
		if (yReached && xReached) 
		{
			print ("Hello! + RESET PHASE");
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
		
		MovePhase (destination, sethResetSpeed);

		if ((Time.time - wait) > (sethWaitTime + 3.5)) //3.5 to compinsiate for getting to point
		{
			if (yReached && xReached) 
			{
				print ("Hello! + ATTACK END");
				isAttack = true;
				isReset = false;
				grabPlayerY = false;
			}
		}
	}

//=========================Emil PHASE===================================
	void EmilStart()
	{
		MovePhase (emilResetPoint.transform.position, emilResetSpeed);

		if (xReached && yReached) 
		{
			isEmilStart = false;
		}

	}

	void EmilPhase()
	{
		if (laserNum == 1 || laserNum == 2)
		{
			SmallLaser ();
		}
		else
			BigLaser();
	}

	void SmallLaser()
	{
		if (!grabTime) 
		{
			wait = Time.time;
			grabTime = true;
		}
		
		if ((Time.time - wait) > (smallLaserDelay - smallPositionShootDelay) && !grabPosition) 
		{
			playerPosition = _player.transform.position;
			grabPosition = true;
		}
		
		if ((Time.time - wait) > smallLaserDelay) 
		{
			CreateSmallLaser();
		}
		
		if ((Time.time - wait) > (smallLaserDelay + smallLaserDuration)) 
		{
			_laser.SetActive(false);
			grabTime = false;
			grabPosition = false;
			laserNum ++;
		}
	}
	
	void BigLaser()
	{
		if (!grabTime) 
		{
			wait = Time.time;
			grabTime = true;
		}
		
		if ((Time.time - wait) > (bigLaserDelay - bigPositionShootDelay) && !grabPosition) 
		{
			playerPosition = _player.transform.position;
			grabPosition = true;
		}
		
		if ((Time.time - wait) > bigLaserDelay) 
		{
			CreateBigLaser();
		}
		
		if ((Time.time - wait) > (bigLaserDelay + bigLaserDuration)) 
		{
			_laser.SetActive(false);
			grabTime = false;
			grabPosition = false;
			laserNum = 1;
		}
	}
	void CreateSmallLaser()
	{
		_laser.SetActive(true);
		smallLaserCollider.gameObject.SetActive (true);
		bigLaserCollider.gameObject.SetActive (false);
		
		_laser.GetComponent<LineRenderer>().SetWidth (2, 2);
		
		_laser.GetComponent<LineRenderer> ().SetPosition (0, _face.transform.position);
		_laser.GetComponent<LineRenderer> ().SetPosition (1, playerPosition);
		smallLaserCollider.transform.position = playerPosition;
	}
	
	void CreateBigLaser()
	{
		_laser.SetActive(true);
		smallLaserCollider.gameObject.SetActive (false);
		bigLaserCollider.gameObject.SetActive (true);
		
		_laser.GetComponent<LineRenderer>().SetWidth (10, 10);
		
		tempVector = new Vector2 (this.transform.position.x, bigLaserPoint.y);
		_laser.GetComponent<LineRenderer> ().SetPosition (0, tempVector);
		_laser.GetComponent<LineRenderer> ().SetPosition (1, bigLaserPoint);
		
		tempVector = new Vector2 (0, bigLaserPoint.y);
		bigLaserCollider.transform.position = tempVector;
		
	}
	//----------------MovePhase--------------
	void MovePhase(Vector2 destination, float movingSpeed)
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
		if ((destination.x - 1) > transform.position.x)
		{
			transform.position += transform.right * movingSpeed * Time.deltaTime;
			xReached = false;
		}
		else if ((destination.x + 1) < transform.position.x)
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
//=================OnCollision===============
	void OnCollisionStay2D(Collision2D playerC)
	{
		if (this.gameObject.GetComponent<EnemiesReceiveDamage> ().hp > 
			(this.gameObject.GetComponent<EnemiesReceiveDamage> ().maxHp * .333))
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
}
