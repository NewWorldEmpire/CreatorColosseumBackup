using UnityEngine;
using System.Collections;

public class LaserBig : MonoBehaviour {
	
	public float	bigLaserDamage;
	private float	armor;

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.tag.Equals("Player"))
		{
			armor = other.GetComponent<CombatScript> ().armor;
			other.GetComponent<PlayerReceivesDamage> ().InitiateCBT (bigLaserDamage.ToString ()).GetComponent<Animator> ().SetTrigger ("Hit"); //changed playerReceivesDamge

			if (armor < bigLaserDamage)
				other.GetComponent<CombatScript> ().health -= (bigLaserDamage - armor);
		}
	}
}
