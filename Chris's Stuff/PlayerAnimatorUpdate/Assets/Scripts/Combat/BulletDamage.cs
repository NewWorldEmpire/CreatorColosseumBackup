using UnityEngine;
using System.Collections;

public class BulletDamage : MonoBehaviour {

	public GameObject _player;
	public GameObject _bullet;

	public float		bulletDamage;

	private float 		armor;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D playerC)
	{
		if (playerC.gameObject.tag == "Player")
		{
			armor = _player.GetComponent<CombatScript>().armor;
			_player.GetComponent<PlayerReceivesDamage>().InitiateCBT(bulletDamage.ToString()).GetComponent<Animator>().SetTrigger("Hit"); //changed playerReceivesDamge
			//this.gameObject.SetActive(false);
			_bullet.gameObject.SetActive(false);
			
			if (armor < bulletDamage)
			{
				_player.GetComponent<CombatScript>().health -= (bulletDamage - armor);
			}
		}
	}


}
