using UnityEngine;
using System.Collections;

public class CheatCode : MonoBehaviour {

	public GameObject Player;
	private string[] cheatCode1;
	private string[] cheatCode2;
	private int index1;
	private int index2;


	
	void Start() {
		// Code is "idkfa", user needs to input this in the right order
		cheatCode1 = new string[] { "k", "e", "n"};
		cheatCode2 = new string[] { "p", "e", "t", "e", "r"};
		index1 = 0;    
	}
	
	void Update() {
		// Check if any key is pressed
		if (Input.anyKeyDown) {
			// Check if the next key in the code is pressed
			if (Input.GetKeyDown(cheatCode1[index1])) {
				// Add 1 to index to check the next key in the code
				index1++;
			}
			// Wrong key entered, we reset code typing
			else {
				index1 = 0;

			}
		
		// If index reaches the length of the cheatCode string, 
		// the entire code was correctly entered
		if (index1 == cheatCode1.Length) {
			Player.transform.position = new Vector3 (0, 0, 0);
		}
		
		}
			else if (Input.anyKeyDown) {
				// Check if the next key in the code is pressed
				if (Input.GetKeyDown(cheatCode2[index2])) {
					// Add 1 to index to check the next key in the code
					index2++;
				}
				// Wrong key entered, we reset code typing
				else {
					index2 = 0;    
				}
			}
		if (index2 == cheatCode2.Length) {
			Player.transform.position = new Vector3(10, 10, 0);
	}

}
}