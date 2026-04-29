using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

public class SocketRandomizer : MonoBehaviour
{
    // Liste de tous tes sockets (à remplir dans l'Inspecteur)
    public List<XRSocketInteractor> allSockets;

    void Start()
    {
        ActivateOneRandomSocket();
    }

    public void ActivateOneRandomSocket()
    {
        if (allSockets.Count == 0) return;

        // 1. Choisir un index au hasard
        int randomIndex = Random.Range(0, allSockets.Count);

        // 2. Parcourir la liste pour activer le bon et éteindre les autres
        for (int i = 0; i < allSockets.Count; i++)
        {
            // On active seulement celui qui correspond à l'index choisi
            allSockets[i].gameObject.SetActive(i == randomIndex);
        }

        Debug.Log("Socket actif : " + allSockets[randomIndex].name);
    }
}