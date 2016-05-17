using UnityEngine;
using System.Collections;

public class Resume : MonoBehaviour {

    public GameObject player;
	public GameObject pauseMenu;
    public GameObject statusHUD;
    public GameObject spells;
	
	public void ResumeGame()
	{
		pauseMenu.SetActive (false);
        player.GetComponent<CombatScript>().enabled = true;
        statusHUD.SetActive(true);
        spells.SetActive(true);
		Time.timeScale = 1;
	}
}
