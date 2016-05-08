using UnityEngine;
using System.Collections;

public class AIEmil : MonoBehaviour {

	public int		topDivider;
	public int		bottomDivider;
	public int		laserNum = 1;

	public float	movingSpeed;

	//---------small laser--------
	public float	smallLaserDelay;
	public float	smallPositionShootDelay = .5f;
	public float   	smallLaserDuration	= .3f;

	//----------big laser--------
	public float	bigLaserDelay;
	public float	bigPositionShootDelay = .5f;
	public float   	bigLaserDuration	= .3f;
	private float	wait;

	public Vector2	destination;
	public Vector2  playerPosition;

	public Vector3  bigLaserPoint = new Vector3 (-120, -45);

	public GameObject	_player;
	public GameObject	_face;
	public GameObject 	_laser;

	public Sprite currentFace;
	public Sprite face1;
	public Sprite face2;
	public Sprite face3;

	public bool yReached;
	public bool grabTime;
	public bool grabPosition;
	// Use this for initialization

	void Start () 
	{
		destination = new Vector2 (this.gameObject.transform.position.x, bottomDivider);
		DescendPhase ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		SelectFace ();
		//transform.LookAt (_player.transform.position);
		if (!yReached)
			DescendPhase ();
		else 
		{
			if (laserNum == 1 || laserNum == 2)
			{
				SmallLaser ();
			}
			else
				BigLaser();

		}
			

	}

	void DescendPhase()
	{
		if ((destination.y + 1) < transform.position.y)
		{
			transform.position += transform.up * -movingSpeed * Time.deltaTime;
			yReached = false;
		}
		else
			yReached = true;
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
			_laser.SetActive(true);
			_laser.GetComponent<LineRenderer> ().SetPosition (0, this.transform.position);
			_laser.GetComponent<LineRenderer> ().SetPosition (1, playerPosition);
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
			_face.GetComponent<SpriteRenderer>().color = Color.red;
			grabPosition = true;
		}
		
		if ((Time.time - wait) > bigLaserDelay) 
		{
			_laser.SetActive(true);
			_laser.GetComponent<LineRenderer>().SetWidth (10, 10);
			_laser.GetComponent<LineRenderer> ().SetPosition (0, this.transform.position);
			_laser.GetComponent<LineRenderer> ().SetPosition (1, bigLaserPoint);
		}
		
		if ((Time.time - wait) > (bigLaserDelay + bigLaserDuration)) 
		{
			_laser.SetActive(false);
			grabTime = false;
			grabPosition = false;
			_face.GetComponent<SpriteRenderer>().color = Color.clear;
			_laser.GetComponent<LineRenderer>().SetWidth (2, 2);
			laserNum = 1;
		}
	}

	void SelectFace()
	{
		if (_player.transform.position.y > topDivider) 
		{
			currentFace = face1;
		}
		else if (_player.transform.position.y > bottomDivider)
		{
			currentFace = face2;
		} 
		else 
		{
			currentFace = face3;
		}

		_face.GetComponent<SpriteRenderer> ().sprite = currentFace;
	}
}

