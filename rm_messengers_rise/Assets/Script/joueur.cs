using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class joueur : MonoBehaviour
{
    public GameObject Beam;
    public GameObject exit;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Socket_1"))
        {
            Beam.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Socket_1"))
        {
            Beam.SetActive(true);
        }
    }
}