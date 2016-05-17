using UnityEngine;
using System.Collections;

public class LaserSmall : MonoBehaviour {

	public float	smallLaserDamage;
	public float	armor;

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag.Equals ("Player")) 
		{ 
			armor = other.GetComponent<CombatScript> ().armor;
			other.GetComponent<PlayerReceivesDamage> ().InitiateCBT (smallLaserDamage.ToString ()).GetComponent<Animator> ().SetTrigger ("Hit"); //changed playerReceivesDamge

			if (armor < smallLaserDamage)
			{
				other.GetComponent<CombatScript> ().health -= (smallLaserDamage - armor);
			}
		}
	}
}
