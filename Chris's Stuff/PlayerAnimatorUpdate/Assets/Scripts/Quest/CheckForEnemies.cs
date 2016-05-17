using UnityEngine;
using System.Collections;

public class CheckForEnemies : MonoBehaviour {

    public AudioSource music;
    public AudioClip battleMusic;
    public AudioClip bossMusic;
    public GameObject[] enemies;
    public GameObject boss;
    public GameObject bossHealthBar;
    public float bossWait;

    //--------AI Vars------
    [HideInInspector]
    public int lowDistance;
    [HideInInspector]
    public int indexClose;
    [HideInInspector]
    public int tempDistance;

    //--------------------
    int counter;
    bool bossSpawned;
    float audio1Volume = 1.0f;
    float audio2Volume = 0.0f;
    bool bossMusicPlaying;

    // Use this for initialization
    void Start()
    {
        boss.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        counter = 0;
        foreach (GameObject enemy in enemies)
        {
            if (enemy.activeInHierarchy == false)
            {
                counter++;
                if (counter == enemies.Length)
                {
                    FadeOut();
                    if (audio1Volume <= 0.1)
                    {
                        if (!bossMusicPlaying)
                        {
                            bossMusicPlaying = true;
                            music.clip = bossMusic;
                            music.Play();
                        }
                        FadeIn();
                    }

                    if (!bossSpawned)
                    {
                        StartCoroutine(SpawnWait());
                    }
                }
            }
        }

        CheckWhoClosest();
    }

    IEnumerator SpawnWait()
    {
        bossSpawned = true;
        yield return new WaitForSeconds(bossWait);
        bossHealthBar.SetActive(true);
        yield return new WaitForSeconds(bossWait);
        boss.SetActive(true);
    }

    void FadeOut()
    {
        if (audio1Volume > 0.1)
        {
            audio1Volume -= 0.5f * Time.deltaTime;
            music.volume = audio1Volume;
        }
    }

    void FadeIn()
    {
        if (audio2Volume < 0.5)
        {
            audio2Volume += 0.25f * Time.deltaTime;
            music.volume = audio2Volume;
        }
    }
    //--------------checkWhoClosest--------------
    public void CheckWhoClosest()
    {
        lowDistance = 100;

        for (int index = 0; index < enemies.Length; index++)
        {
            if (enemies[index].activeSelf)
            {
                tempDistance = (int)enemies[index].GetComponent<AISmall>().playerDistance;
                enemies[indexClose].GetComponent<AISmall>().forceAttack = false;

                if (tempDistance < lowDistance)
                {
                    lowDistance = tempDistance;
                    indexClose = index;
                }
            }
        }

        for (int index = 0; index < enemies.Length; index++)
        {
            if (indexClose == index)
            {
                enemies[index].GetComponent<AISmall>().forceAttack = true;
            }
            else
            {
                enemies[index].GetComponent<AISmall>().forceAttack = false;
            }
        }
    }
}