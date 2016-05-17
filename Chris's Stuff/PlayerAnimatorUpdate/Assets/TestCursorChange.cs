using UnityEngine;
using System.Collections;

public class TestCursorChange : MonoBehaviour {

	public Texture2D texture1;
	public Texture2D texture2;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0)) 
		{
			Cursor.SetCursor (texture1, Vector2.zero, CursorMode.Auto);
		} 
		else if (Input.GetMouseButtonDown (1)) 
		{
			Cursor.SetCursor (texture2, Vector2.zero, CursorMode.Auto);
		}
	
	}
}
