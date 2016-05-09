using UnityEngine;
using System.Collections;

public class Transitions : MonoBehaviour
{
	public AudioSource music;
	public AudioClip battleMusic;
    public GameObject map;
    public GameObject canvas;
    public GameObject player;
	public GameObject mouse;
	public GameObject mainCam;
    public GameObject cutsceneCam;

	public Sprite[] backgroundArray;
	public Sprite[] foregroundArray;

	public GameObject background;
	public GameObject foreground;

    public GameObject mikeEnemies;
    public GameObject sethEnemies;
    public GameObject emilEnemies;
    //public GameObject finalBoss;

	public GameObject mikeEnemiesList;
	public GameObject sethEnemiesList;
	public GameObject emilEnemiesList;

    private float wait;

	public int	levelSelect = 1;
	private bool sethTransition;
	private bool emilTransition;
	private bool runOnce = true;

    public bool hasDied;
    void Start ()
    {
		background.GetComponent<SpriteRenderer>().sprite = backgroundArray[0];
		foreground.GetComponent<SpriteRenderer>().sprite = foregroundArray[0];
		mikeEnemies.SetActive (true);
		sethEnemies.SetActive (false);
		emilEnemies.SetActive (false);
		sethTransition = true;
		emilTransition = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (levelSelect == 2 && sethTransition)
        {
			sethTransition = StartTransition(sethEnemies, sethEnemiesList, mikeEnemiesList, backgroundArray[1], foregroundArray[1]);
        }
		else if (levelSelect == 3 && emilTransition)
        {
			emilTransition = StartTransition(emilEnemies, emilEnemiesList, sethEnemiesList, backgroundArray[2], foregroundArray[2]);
        }
		//else if (levelSelect == 3)
        //{
            //StartTransition(emilBoss, sceneThree, sceneFour);
        //}
    }
  

   	bool StartTransition(GameObject enemies, GameObject enemyList, GameObject previousList, Sprite nextBackground, Sprite nextForeground)
    {
		if (runOnce) 
		{
			wait = Time.time;
			Camera.main.gameObject.SetActive (false);
			cutsceneCam.SetActive (true);
			player.transform.position = new Vector3 (-35, -50, 0);
			previousList.SetActive(false);
			map.SetActive(true);
			canvas.SetActive(false);
			player.SetActive(false);
			mouse.SetActive (false);
			runOnce = false;
		}
        
		if (Time.time > wait + 5) 
		{
			background.GetComponent<SpriteRenderer> ().sprite = nextBackground;
			foreground.GetComponent<SpriteRenderer> ().sprite = nextForeground;
			enemies.SetActive (true);
			enemyList.SetActive (true);
			EndTransition ();

			return false;
		} 
		else 
		{
			return true;
		}
    }

    void EndTransition()
    {
		mainCam.SetActive (true);
		cutsceneCam.SetActive (false);
		canvas.SetActive (true);
		player.SetActive (true);
		mouse.SetActive (true);
		map.SetActive (false);
		music.clip = battleMusic;
		music.Play ();
	}

 }

