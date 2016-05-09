using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public GameObject _player;

	public int movingSpeed;
	public int bulletDamage;

	public int xMin;

	public Vector2 destination;

	// Use this for initialization
	void Start () 
	{
		destination = new Vector2 (xMin, transform.position.y);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (this.gameObject.activeSelf)
		{
			MovePhase();
		}
	
	}

	void MovePhase()
	{
		if (destination.x < transform.position.x) 
		{
			transform.position += transform.right * -movingSpeed * Time.deltaTime;
		} 
		else
			this.gameObject.SetActive (false);
	}

	void OnCollisionEnter2D(Collision2D playerC)
	{
		if (playerC.gameObject.tag == "Player") 
		{
			_player.GetComponent<PlayerReceivesDamage>().InitiateCBT(bulletDamage.ToString()).GetComponent<Animator>().SetTrigger("Hit"); //changed playerReceivesDamge
			_player.GetComponent<CombatScript>().health -= bulletDamage;
			this.gameObject.SetActive(false);
		}
	}
}
