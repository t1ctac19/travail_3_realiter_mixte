using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class joueur : MonoBehaviour
{

    public GameObject ampoule;
    public VideoPlayer videoPlayer;
    public Animator animLumiere;
    public int count;  
    public HighScorePersistant pointage;
    public Animator animCoin;

    void OnTriggerEnter(Collider other)
{
     if (other.CompareTag("sale_a_manger"))
    {
        ampoule.SetActive(true);
    }

     if (other.CompareTag("salon"))
    {
        videoPlayer.Play(); 
    }

     if (other.CompareTag("entree_maison"))
    {
        // Démarre l'animation
        animLumiere.Play("fade_in");
    }

         if (other.CompareTag("coin"))
    {
        other.gameObject.SetActive(false);
        pointage.OnChangerPointage(count++);
    }

         if (other.CompareTag("coin_fuite"))
    {
        // Démarre l'animation
        animCoin.Play("etoile_fuite");
    }
}

void OnTriggerExit(Collider other)
{

         if (other.CompareTag("sale_a_manger"))
    {
        ampoule.SetActive(false);
    }

         if (other.CompareTag("salon"))
    {
        videoPlayer.Stop(); 
    }
         if (other.CompareTag("entree_maison"))
    {
        // Démarre l'animation
        animLumiere.Play("fade_out");
    }

}

}
