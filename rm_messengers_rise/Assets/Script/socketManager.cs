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
        EteindreLigne(); // On s'assure que la ligne active est éteinte au démarrage
    }

    public void ActivateOneRandomSocket()
    {
        if (allSockets.Count == 0) return;

        // 1. On crée une liste temporaire avec uniquement les sockets VIDES
        List<XRSocketInteractor> socketsVides = new List<XRSocketInteractor>();
        
        foreach (var socket in allSockets)
        {
            // "hasSelection" vérifie si le socket contient un objet
            if (!socket.hasSelection) 
            {
                socketsVides.Add(socket);
            }
        }

        // S'il n'y a plus de sockets vides (le jeu est fini), on arrête
        if (socketsVides.Count == 0) return;

        // 2. On choisit un socket au hasard PARMI LES VIDES
        int randomIndex = Random.Range(0, socketsVides.Count);
        socketActif = socketsVides[randomIndex]; // Voici le nouveau gagnant

        // 3. On met à jour l'état de tous les sockets de la scène
        foreach (var socket in allSockets)
        {
            if (socket.hasSelection)
            {
                // CRUCIAL : Si le socket a déjà un objet, il DOIT rester activé pour le retenir !
                socket.enabled = true; 
                ChangerEtatLigne(socket, false); // On garde sa ligne éteinte
            }
            else
            {
                // S'il est vide, on l'active SEULEMENT si c'est le nouveau socket actif
                socket.enabled = (socket == socketActif);
                ChangerEtatLigne(socket, false); // On éteint la ligne par défaut
            }
        }
    }

    // Fonction à appeler quand tu PRENDS l'objet
    public void AllumerLigne()
    {
        ChangerEtatLigne(socketActif, true);
    }

    // Fonction à appeler quand tu LÂCHES l'objet (ou le mets dans le socket)
    public void EteindreLigne()
    {
        ChangerEtatLigne(socketActif, false);
    }

    // --- NOUVELLE FONCTION UTILITAIRE ---
    // Gère l'allumage et l'extinction de la ligne pour n'importe quel socket
    private void ChangerEtatLigne(XRSocketInteractor socket, bool etat)
    {
        if (socket != null)
        {
            // On cherche spécifiquement l'enfant qui s'appelle "Line"
            Transform childLine = socket.transform.Find("Line");

            if (childLine != null)
            {
                var visual = childLine.GetComponent<XRInteractorLineVisual>();
                if (visual != null) visual.enabled = etat;

                var line = childLine.GetComponent<LineRenderer>();
                if (line != null) line.enabled = etat;
            }
        }
    }

    // --- NOUVELLE FONCTION POUR VERROUILLER L'OBJET ---
    public void VerrouillerObjetDansSocket(SelectEnterEventArgs args)
    {
        // On récupère l'objet qui vient de se clipser dans le socket
        XRGrabInteractable objetPose = args.interactableObject.transform.GetComponent<XRGrabInteractable>();

        if (objetPose != null)
        {
            // On change son calque d'interaction pour qu'il échappe aux mains
            objetPose.interactionLayers = InteractionLayerMask.GetMask("Verrouille");
        }
    }
}