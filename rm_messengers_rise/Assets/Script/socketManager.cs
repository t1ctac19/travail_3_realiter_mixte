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
        
        // Correction : On utilise la fonction utilitaire pour éteindre au démarrage
        if (socketActif != null)
        {
            ChangerEtatLigne(socketActif, false);
        }
    }

    public void ActivateOneRandomSocket()
    {
        if (allSockets.Count == 0) return;

        // 1. On crée une liste temporaire avec uniquement les sockets VIDES
        List<XRSocketInteractor> socketsVides = new List<XRSocketInteractor>();
        
        foreach (var socket in allSockets)
        {
            if (!socket.hasSelection) 
            {
                socketsVides.Add(socket);
            }
        }

        // S'il n'y a plus de sockets vides (le jeu est fini), on arrête
        if (socketsVides.Count == 0) return;

        // 2. On choisit un socket au hasard PARMI LES VIDES
        int randomIndex = Random.Range(0, socketsVides.Count);
        socketActif = socketsVides[randomIndex]; 

        // 3. On met à jour l'état de tous les sockets de la scène
        foreach (var socket in allSockets)
        {
            if (socket.hasSelection)
            {
                // Si le socket a déjà un objet, il DOIT rester activé pour le retenir !
                socket.enabled = true; 
                ChangerEtatLigne(socket, false); 
            }
            else
            {
                // S'il est vide, on l'active SEULEMENT si c'est le nouveau socket actif
                socket.enabled = (socket == socketActif);
                ChangerEtatLigne(socket, false); 
            }
        }
    }

    // --- GESTION DE L'AFFICHAGE DE LA LIGNE ---

    // Fonction à appeler quand l'objet est PRIS (Select Entered sur l'Objet)
    public void AllumerLigneSiJoueur(SelectEnterEventArgs args)
    {
        if (args.interactorObject is XRSocketInteractor) return;
        ChangerEtatLigne(socketActif, true);
    }

    // Fonction à appeler quand l'objet est LÂCHÉ (Select Exited sur l'Objet)
    public void EteindreLigneSiJoueur(SelectExitEventArgs args)
    {
        if (args.interactorObject is XRSocketInteractor) return;
        ChangerEtatLigne(socketActif, false);
    }

    private void ChangerEtatLigne(XRSocketInteractor socket, bool etat)
    {
        if (socket != null)
        {
            Transform childLine = socket.transform.Find("Line");
            if (childLine != null)
            {
                // On désactive le GameObject entier, c'est plus stable en VR !
                childLine.gameObject.SetActive(etat);
            }
        }
    }

    // --- GESTION DE L'ACTION DU SOCKET ---

    // Fonction PRINCIPALE à appeler dans l'événement "Select Entered" DU SOCKET
    public void TraiterObjetRecu(SelectEnterEventArgs args)
    {
        // 1. On utilise la méthode de verrouillage par calque (Interaction Layer)
        VerrouillerObjetDansSocket(args);

        // (Note : Si tu préfères l'autre méthode qui fige complètement la physique, 
        // mets la ligne au-dessus en commentaire et décommente la ligne du dessous)
        // FigerObjetDansSocket(args);

        // 2. On éteint la ligne de ce socket par sécurité
        XRSocketInteractor socketRempli = args.interactorObject as XRSocketInteractor;
        if (socketRempli != null)
        {
            ChangerEtatLigne(socketRempli, false);
        }

        // 3. On tire au sort le prochain socket !
        ActivateOneRandomSocket();
    }

    private void VerrouillerObjetDansSocket(SelectEnterEventArgs args)
    {
        XRGrabInteractable objetPose = args.interactableObject.transform.GetComponent<XRGrabInteractable>();
        if (objetPose != null)
        {
            objetPose.interactionLayers = InteractionLayerMask.GetMask("Verrouille");
        }
    }

    private void FigerObjetDansSocket(SelectEnterEventArgs args)
    {
        GameObject objetPose = args.interactableObject.transform.gameObject;
        Transform pointAttache = args.interactorObject.GetAttachTransform(args.interactableObject);

        XRGrabInteractable grabComp = objetPose.GetComponent<XRGrabInteractable>();
        if (grabComp != null) grabComp.enabled = false;

        Rigidbody rb = objetPose.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true; 
            rb.useGravity = false;
            rb.velocity = Vector3.zero; 
            rb.angularVelocity = Vector3.zero;
        }

        Transform parentTransform = (pointAttache != null) ? pointAttache : args.interactorObject.transform;
        objetPose.transform.SetParent(parentTransform);
        objetPose.transform.localPosition = Vector3.zero;
        objetPose.transform.localRotation = Quaternion.identity;
    }
}