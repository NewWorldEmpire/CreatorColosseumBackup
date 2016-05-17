using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControlsButton : MonoBehaviour {

    public GameObject mainMenu;
    public GameObject controlsMenu;

	public void ControlsMenu()
	{
		controlsMenu.SetActive(true);
		mainMenu.SetActive(false);
	}
}
