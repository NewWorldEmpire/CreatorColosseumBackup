using UnityEngine;
using System.Collections;

public class LaserBig : MonoBehaviour {
	
	public float	bigLaserDamage;

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag.Equals("Player"))
		{
			other.GetComponent<PlayerReceivesDamage> ().InitiateCBT (bigLaserDamage.ToString ()).GetComponent<Animator> ().SetTrigger ("Hit"); //changed playerReceivesDamge
			other.GetComponent<CombatScript> ().health -= (bigLaserDamage - other.GetComponent<CombatScript> ().armor);
		}
	}
}
