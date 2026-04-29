using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

public class SocketRandomizer : MonoBehaviour
{
    public List<XRSocketInteractor> allSockets;

    // Cette variable va mémoriser le socket qui a été choisi au hasard
    private XRSocketInteractor socketActif;

    void Start()
    {
        ActivateOneRandomSocket();
        EteindreLigne(); // On s'assure que la ligne est éteinte au démarrage du jeu
    }

    public void ActivateOneRandomSocket()
    {
        if (allSockets.Count == 0) return;

        int randomIndex = Random.Range(0, allSockets.Count);

        for (int i = 0; i < allSockets.Count; i++)
        {
            allSockets[i].enabled = (i == randomIndex);

            if (i == randomIndex)
            {
                socketActif = allSockets[i]; // On enregistre le socket gagnant
            }
        }
    }

    // Fonction à appeler quand tu PRENDS l'objet
    public void AllumerLigne()
    {
        if (socketActif != null)
        {
            // On cherche spécifiquement l'enfant qui s'appelle "Line"
            Transform childLine = socketActif.transform.Find("Line");

            if (childLine != null)
            {
                var visual = childLine.GetComponent<XRInteractorLineVisual>();
                if (visual != null) visual.enabled = true;

                var line = childLine.GetComponent<LineRenderer>();
                if (line != null) line.enabled = true;
            }
        }
    }

    // Fonction à appeler quand tu LÂCHES l'objet (ou le mets dans le socket)
    public void EteindreLigne()
    {
        if (socketActif != null)
        {
            // On cherche spécifiquement l'enfant qui s'appelle "Line"
            Transform childLine = socketActif.transform.Find("Line");

            if (childLine != null)
            {
                var visual = childLine.GetComponent<XRInteractorLineVisual>();
                if (visual != null) visual.enabled = false;

                var line = childLine.GetComponent<LineRenderer>();
                if (line != null) line.enabled = false;
            }
        }
    }
}