using UnityEngine;
using System.Collections;

public class PauseGame : MonoBehaviour {

    public GameObject player;
    public GameObject pauseMenu;
    public GameObject spells;
    public GameObject playerStatusHUD;
    bool paused = false;

    void Start()
    {
        Time.timeScale = 1;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            paused = togglePause();
            if (paused)
            {
                player.GetComponent<CombatScript>().enabled = false;
                pauseMenu.SetActive(true);
                spells.SetActive(false);
                playerStatusHUD.SetActive(false);
            }
            else
            {
                player.GetComponent<CombatScript>().enabled = true;
                pauseMenu.SetActive(false);
                spells.SetActive(true);
                playerStatusHUD.SetActive(true);
            }
        }
    }

    bool togglePause()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            return (false);
        }
        else
        {
            Time.timeScale = 0;
            return (true);
        }
    }
}