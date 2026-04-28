using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class joueur : MonoBehaviour
{
    public GameObject Beam;


    void OnTriggerEnter(Collider other)
{
     if (other.CompareTag("Socket_1"))
    {
        Beam.SetActive(false);
    }

}

void OnTriggerExit(Collider other)
{

         if (other.CompareTag("Socket_1"))
    {
        Beam.SetActive(true);
    }
}

}
