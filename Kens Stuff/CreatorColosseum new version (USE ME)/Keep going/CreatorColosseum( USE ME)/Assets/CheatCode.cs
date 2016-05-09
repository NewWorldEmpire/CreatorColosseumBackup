using UnityEngine;
using System.Collections;

public class CheatCode : MonoBehaviour {


	private string[] cheatCode;
	private string[] cheatCodea;
	private int index;
	public GameObject Player;
	
	void Start() {
		// Code is "idkfa", user needs to input this in the right order
		cheatCode = new string[] { "k", "e", "n", };
		cheatCodea = new string[] { "s", "a", "m", };
		index = 0;

	}
	
	void Update() {
		// Check if any key is pressed
		if (Input.anyKeyDown) {
			// Check if the next key in the code is pressed
			if (Input.GetKeyDown(cheatCode[index])) {
				// Add 1 to index to check the next key in the code
				index++;
			}
			// Wrong key entered, we reset code typing
			else {
				index = 0;    
			}
		}

		if (Input.anyKeyDown) {
			// Check if the next key in the code is pressed
			if (Input.GetKeyDown(cheatCodea[index])) {
				// Add 1 to index to check the next key in the code
				index++;
			}
			// Wrong key entered, we reset code typing
			else {
				index = 0;    
			}
		}


		// If index reaches the length of the cheatCode string, 
		// the entire code was correctly entered
		if (index == cheatCode.Length) 
		{
			Player.GetComponent<CombatScript>().CheatCodes();
			index = 0;
		}
		if (index == cheatCodea.Length) 
		{
			transform.position = new Vector3(30, 0, 0);
			index = 0;
		}
	}
}
