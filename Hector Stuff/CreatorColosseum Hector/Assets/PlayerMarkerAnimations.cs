using UnityEngine;
using System.Collections;

public class PlayerMarkerAnimations : MonoBehaviour
{
   public GameObject[] anim;
    
    public void FirstTransition()
    {
        anim[0].SetActive(true);
        anim[1].SetActive(false);
        anim[2].SetActive(false);
    }
    public void SecondTransition()
    {
        anim[0].SetActive(false);
        anim[1].SetActive(true);
        anim[2].SetActive(false);
    }
    public void ThirdTransition()
    {
        anim[0].SetActive(false);
        anim[1].SetActive(false);
        anim[2].SetActive(true);
    }
}
