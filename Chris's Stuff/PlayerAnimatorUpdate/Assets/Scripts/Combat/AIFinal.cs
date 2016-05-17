using UnityEngine;
using System.Collections;

public class AIFinal : MonoBehaviour {

	public GameObject _player;
	public GameObject _level;
	public GameObject bulletSpawnPoint;
	public GameObject laserSpawnPoint;

	public Sprite	  facingRight;
	public Sprite 	  facingLeft;
	public Sprite	  laserFar;
	public Sprite	  laserNear;

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
	public bool		isDead;
	public bool		yReached = true;
	public bool		playSoundOnce;
	private bool 	isMikeStart	 = true;
	private bool 	isSethStart	 = true;
	private bool	isEmilStart  = true;

	//-------------audioClips--------
	public AudioSource  source;
	public AudioClip   	tankFire;
	public AudioClip  	bikeHit;
	public AudioClip   	smallLaser;
	public AudioClip	chargeLaser;
	public AudioClip    bigLaser;

	public Animator		anim;

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
		else if (this.gameObject.GetComponent<EnemiesReceiveDamage> ().hp > 0)
		{
			if (isEmilStart)
				EmilStart();
			else
				EmilPhase();
		}
		else
			if (!isDead) 
			{
				_level.GetComponent<Transitions>().levelSelect ++;
				isDead = true;
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
			GetComponent<Animator>().SetBool("Idle", true);
			GetComponent<Animator>().SetBool("Shoot", false);
		}
		else 
		{
			if (Time.time > lastShot + mikeFireFreq)
			{
				ShootPhase();
				destination = CreateDestination ();
			}
			else 
			{
				GetComponent<Animator>().SetBool("Idle", true);
				GetComponent<Animator>().SetBool("Shoot", false);
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
		source.PlayOneShot(tankFire);    
		lastShot = Time.time;

		GetComponent<Animator>().SetBool("Idle", false);
		GetComponent<Animator>().SetBool("Shoot", true);
		
		GameObject obj = EnemyFinalPooling.current.GetPooledObject ();
		
		if (obj == null) 
		{
			return;
		}
		
		obj.transform.position = bulletSpawnPoint.transform.position;
		obj.transform.rotation = transform.rotation;
		obj.SetActive (true);
		
	}
//=========================SETH PHASE===================================
	void SethStart()
	{
		GetComponent<Animator>().SetBool("Shoot", false);
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
		
		MovePhase (destination, sethChargeSpeed);
		
		if (yReached && xReached) 
		{
			//print ("Hello! + RESET PHASE");
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
				print ("left");
				GetComponent<Animator>().SetBool("GoLeft", true);
				GetComponent<Animator>().SetBool("GoRight", false);
				//GetComponent<SpriteRenderer>().sprite = facingLeft;
			}
			else if (resetPoint < 0)
			{
				print ("right");
				GetComponent<Animator>().SetBool("GoRight", true);
				GetComponent<Animator>().SetBool("GoLeft", false);
				//GetComponent<SpriteRenderer>().sprite = facingRight;
			}
			destination = new Vector2 (resetPoint, _player.transform.position.y);
			grabPlayerY = true;
			wait = Time.time;
		}
		
		MovePhase (destination, sethResetSpeed);

		if ((Time.time - wait) > (sethWaitTime + 3.5)) //3.5 to compinsiate for getting to point
		{
			if (yReached && xReached) 
			{
				//print ("Hello! + ATTACK END");
				isAttack = true;
				isReset = false;
				grabPlayerY = false;
			}
		}
	}

//=========================Emil PHASE===================================
	void EmilStart()
	{
		GetComponent<Animator>().SetBool("GoLeft", true);
		GetComponent<Animator>().SetBool("GoRight", false);
		MovePhase (emilResetPoint.transform.position, emilResetSpeed);

		if (xReached && yReached) 
		{
			isEmilStart = false;
		}

	}

	void EmilPhase()
	{
		facePosition = laserSpawnPoint.transform.position;

		if (laserNum == 1 || laserNum == 2)
		{
			SmallLaser ();

			if (_player.transform.position.y > topDivider) 
			{
				GetComponent<Animator>().SetBool("ShootHigh", true);
				GetComponent<Animator>().SetBool("ShootMiddle", false);
				GetComponent<Animator>().SetBool("ShootLow", false);
				//GetComponent<SpriteRenderer>().sprite = laserFar;
			}
			else if (_player.transform.position.y > bottomDivider)
			{
				GetComponent<Animator>().SetBool("ShootHigh", false);
				GetComponent<Animator>().SetBool("ShootMiddle", true);
				GetComponent<Animator>().SetBool("ShootLow", false);
				//GetComponent<SpriteRenderer>().sprite = facingLeft;
			} 
			else 
			{
				GetComponent<Animator>().SetBool("ShootHigh", false);
				GetComponent<Animator>().SetBool("ShootMiddle", false);
				GetComponent<Animator>().SetBool("ShootLow", true);
				//GetComponent<SpriteRenderer>().sprite = laserNear;
			}
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
			EndLaser();
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
			source.PlayOneShot(chargeLaser);
			playerPosition = _player.transform.position;
			grabPosition = true;
			GetComponent<SpriteRenderer>().sprite = facingLeft;
		}
		
		if ((Time.time - wait) > bigLaserDelay) 
		{
			CreateBigLaser();
			//GetComponent<SpriteRenderer>().sprite = facingLeft;
		}
		
		if ((Time.time - wait) > (bigLaserDelay + bigLaserDuration)) 
		{
			EndLaser();
			source.Stop ();
			laserNum = 1;
		}
	}

	void CreateSmallLaser()
	{
		if (!playSoundOnce) 
		{
			source.Stop ();
			source.PlayOneShot(smallLaser);
			playSoundOnce = true;
		}
		_laser.SetActive(true);
		smallLaserCollider.gameObject.SetActive (true);
		bigLaserCollider.gameObject.SetActive (false);
		
		_laser.GetComponent<LineRenderer>().SetWidth (2, 2);
		
		_laser.GetComponent<LineRenderer> ().SetPosition (0, facePosition);
		_laser.GetComponent<LineRenderer> ().SetPosition (1, playerPosition);
		smallLaserCollider.transform.position = playerPosition;
	}
	
	void CreateBigLaser()
	{
		if (!playSoundOnce) 
		{
			source.Stop ();
			source.clip = bigLaser;
			source.Play();
			playSoundOnce = true;
		}
		
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
	
	void EndLaser()
	{
		//source.Stop ();
		playSoundOnce = false;
		_laser.SetActive(false);
		grabTime = false;
		grabPosition = false;
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
	void OnCollisionEnter2D(Collision2D playerC)
	{
		if (playerC.gameObject.tag.Equals ("Player") && (this.gameObject.GetComponent<EnemiesReceiveDamage> ().hp > 
		                                                 (this.gameObject.GetComponent<EnemiesReceiveDamage> ().maxHp * .333)) ) 
		{
			source.clip = bikeHit;
			source.Play();
		}
	}

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
