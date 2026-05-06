using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

public class SocketRandomizer : MonoBehaviour
{
    [Header("Connexion UI Argent")]
    // Ajout de la référence vers ton nouveau script
    public ToggleMoneyOnKey gestionnaireArgent; 
    public int recompenseParScroll = 15;

    [Header("Sockets")]
    public List<XRSocketInteractor> allSockets;

    private XRSocketInteractor socketActif;

    void Start()
    {
        ActivateOneRandomSocket();
        
        if (socketActif != null)
        {
            ChangerEtatLigne(socketActif, false);
        }
    }

    public void ActivateOneRandomSocket()
    {
        if (allSockets.Count == 0) return;

        List<XRSocketInteractor> socketsVides = new List<XRSocketInteractor>();
        
        foreach (var socket in allSockets)
        {
            if (!socket.hasSelection) 
            {
                socketsVides.Add(socket);
            }
        }

        if (socketsVides.Count == 0) return;

        int randomIndex = Random.Range(0, socketsVides.Count);
        socketActif = socketsVides[randomIndex]; 

        foreach (var socket in allSockets)
        {
            if (socket.hasSelection)
            {
                socket.enabled = true; 
                ChangerEtatLigne(socket, false); 
            }
            else
            {
                socket.enabled = (socket == socketActif);
                ChangerEtatLigne(socket, false); 
            }
        }
    }

    // --- GESTION DE L'AFFICHAGE DE LA LIGNE ---

    public void AllumerLigneSiJoueur(SelectEnterEventArgs args)
    {
        if (args.interactorObject is XRSocketInteractor) return;
        ChangerEtatLigne(socketActif, true);
    }

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
                childLine.gameObject.SetActive(etat);
            }
        }
    }

    // --- GESTION DE L'ACTION DU SOCKET ---

    public void TraiterObjetRecu(SelectEnterEventArgs args)
    {
        // --- NOUVEAUTÉ : On appelle la fonction de ton script d'UI ---
        if (gestionnaireArgent != null)
        {
            gestionnaireArgent.AddMoney(recompenseParScroll);
        }
        else
        {
            Debug.LogWarning("Attention: Le script ToggleMoneyOnKey n'est pas assigné !");
        }

        // 1. Verrouillage
        VerrouillerObjetDansSocket(args);

        // 2. Extinction de la ligne
        XRSocketInteractor socketRempli = args.interactorObject as XRSocketInteractor;
        if (socketRempli != null)
        {
            ChangerEtatLigne(socketRempli, false);
        }

        // 3. Prochain socket
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