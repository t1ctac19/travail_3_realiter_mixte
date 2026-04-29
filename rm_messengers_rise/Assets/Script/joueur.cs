using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class joueur : MonoBehaviour
{
    public GameObject Beam;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Socket_1"))
        {
            Beam.SetActive(false);
            Debug.Log("Trigger Enter : Le Beam est désactivé !");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Socket_1"))
        {
            Beam.SetActive(true);
            Debug.Log("Trigger Exit : Le Beam est réactivé !");
        }
    }
}