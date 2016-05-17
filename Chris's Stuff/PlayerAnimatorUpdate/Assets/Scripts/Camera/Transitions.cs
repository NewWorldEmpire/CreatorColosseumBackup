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
    public GameObject playerMarker;

    public Sprite[] backgroundArray;
    public Sprite[] foregroundArray;
    public GameObject[] anim; //New

    public GameObject background;
    public GameObject foreground;

    public GameObject mikeEnemies;
    public GameObject sethEnemies;
    public GameObject emilEnemies;
    public GameObject finalEnemies;

    public GameObject mikeEnemiesList;
    public GameObject sethEnemiesList;
    public GameObject emilEnemiesList;
    public GameObject finalEnemiesList;

    private float wait;
    public float transitionTime;    //New //Public just in case but set it to 13

    public int levelSelect = 1;
    private bool sethTransition;
    private bool emilTransition;
    private bool finalTransition;
    private bool runOnce = true;

    public bool hasDied;
    //public Animator anim;
    void Start()
    {
        background.GetComponent<SpriteRenderer>().sprite = backgroundArray[0];
        foreground.GetComponent<SpriteRenderer>().sprite = foregroundArray[0];
        mikeEnemies.SetActive(true);
        sethEnemies.SetActive(false);
        emilEnemies.SetActive(false);
		finalEnemies.SetActive(false);
        sethTransition = true;
        emilTransition = true;
        finalTransition = true;
		mikeEnemiesList.SetActive(true);
        //playerMarker.GetComponent<Animator>().SetBool("Idle", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (levelSelect == 2 && sethTransition)
        {
            sethTransition = StartTransition(sethEnemies, sethEnemiesList, mikeEnemiesList, backgroundArray[1], foregroundArray[1], "Seth");
        }
        else if (levelSelect == 3 && emilTransition)
        {
            emilTransition = StartTransition(emilEnemies, emilEnemiesList, sethEnemiesList, backgroundArray[2], foregroundArray[2], "Emil");
        }
        else if (levelSelect == 4 && finalTransition)
        {
            finalTransition = StartTransition(finalEnemies, finalEnemiesList, emilEnemiesList, backgroundArray[3], foregroundArray[3], "FinalBoss");
        }
		else if (levelSelect == 5)
		{
			Application.LoadLevel("WinScreen");
		}
    }


    bool StartTransition(GameObject enemies, GameObject enemyList, GameObject previousList, Sprite nextBackground, Sprite nextForeground, string sceneName)
    {
        if (runOnce)
        {
            wait = Time.time;
            Camera.main.gameObject.SetActive(false);
            cutsceneCam.SetActive(true);
            playerMarker.SetActive(true);

            if (sceneName == "Seth")        //New
            {
                anim[0].SetActive(true);
                anim[1].SetActive(false);
                anim[2].SetActive(false);
            }
            else if (sceneName == "Emil")           //New
            {
                anim[0].SetActive(false);
                anim[1].SetActive(true);
                anim[2].SetActive(false);
            }
            else if (sceneName == "FinalBoss")        //New
            {
                anim[0].SetActive(false);
                anim[1].SetActive(false);
                anim[2].SetActive(true);
            }
            player.transform.position = new Vector3(-35, -50, 0);
            previousList.SetActive(false);
            map.SetActive(true);
            canvas.SetActive(false);
            player.SetActive(false);
            mouse.SetActive(false);
            runOnce = false;
        }

        if (Time.time > wait + transitionTime)
        {
            background.GetComponent<SpriteRenderer>().sprite = nextBackground;
            foreground.GetComponent<SpriteRenderer>().sprite = nextForeground;
            enemies.SetActive(true);
            enemyList.SetActive(true);
            EndTransition();
            runOnce = true;
            return false;
        }
        else
        {
            return true;
        }
    }

    void EndTransition()
    {
        mainCam.SetActive(true);
        cutsceneCam.SetActive(false);
        canvas.SetActive(true);
        player.SetActive(true);
		player.GetComponent<CombatScript> ().armor = 0;
        mouse.SetActive(true);
        map.SetActive(false);
        music.clip = battleMusic;
        music.Play();
    }

}