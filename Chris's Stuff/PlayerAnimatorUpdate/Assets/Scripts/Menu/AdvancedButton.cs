using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AdvancedButton : MonoBehaviour {

    public GameObject activateMenu;
    public GameObject closeMenu;

	public void AdvanceMenu()
	{
		activateMenu.SetActive(true);
		closeMenu.SetActive(false);
	}
}
