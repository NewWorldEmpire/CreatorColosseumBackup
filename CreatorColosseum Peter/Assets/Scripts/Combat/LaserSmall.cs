using UnityEngine;
using System.Collections;

public class LaserSmall : MonoBehaviour {

	public float	smallLaserDamage;

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag.Equals ("Player")) 
		{ 
			other.GetComponent<PlayerReceivesDamage> ().InitiateCBT (smallLaserDamage.ToString ()).GetComponent<Animator> ().SetTrigger ("Hit"); //changed playerReceivesDamge
			other.GetComponent<CombatScript> ().health -= (smallLaserDamage - other.GetComponent<CombatScript> ().armor);
		}
	}
}
