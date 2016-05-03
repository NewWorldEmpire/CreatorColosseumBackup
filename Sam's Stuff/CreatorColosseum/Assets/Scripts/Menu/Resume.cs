using UnityEngine;
using System.Collections;

public class Resume : MonoBehaviour {

    public GameObject player;
    public GameObject pauseMenu;
    public GameObject statusHUD;
    public GameObject spells;
	
	public void ResumeGame()
	{
        player.GetComponent<CombatScript>().enabled = true;
        pauseMenu.SetActive (false);
        statusHUD.SetActive(true);
        spells.SetActive(true);
		Time.timeScale = 1;
	}
}
