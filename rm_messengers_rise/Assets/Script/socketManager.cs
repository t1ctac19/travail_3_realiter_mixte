using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

public class SocketRandomizer : MonoBehaviour
{
    [Header("Système d'Argent")]
    public int argentTotal = 0;
    public int recompenseParScroll = 15;
    public ToggleMoneyOnKey moneyManager; // Référence au script UI

    [Header("Sockets")]
    public List<XRSocketInteractor> allSockets;

    private XRSocketInteractor socketActif;

    void Start()
    {
        ActivateOneRandomSocket();
        if (socketActif != null)
            ChangerEtatLigne(socketActif, false);
    }

    public void ActivateOneRandomSocket()
    {
        if (allSockets.Count == 0) return;

        List<XRSocketInteractor> socketsVides = new List<XRSocketInteractor>();
        foreach (var socket in allSockets)
        {
            if (!socket.hasSelection)
                socketsVides.Add(socket);
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
                childLine.gameObject.SetActive(etat);
        }
    }

    public void TraiterObjetRecu(SelectEnterEventArgs args)
    {
        // Ajout de l'argent via ToggleMoneyOnKey
        if (moneyManager != null)
            moneyManager.AddMoney(recompenseParScroll);
        else
            Debug.LogWarning("moneyManager non assigné dans SocketRandomizer !");

        argentTotal += recompenseParScroll; // Garde le total local en sync
        Debug.Log("Scroll déposé ! Argent total : " + argentTotal);

        FigerObjetDansSocket(args);

        XRSocketInteractor socketRempli = args.interactorObject as XRSocketInteractor;
        if (socketRempli != null)
            ChangerEtatLigne(socketRempli, false);

        ActivateOneRandomSocket();
    }

<<<<<<< Updated upstream
    private void VerrouillerObjetDansSocket(SelectEnterEventArgs args)
    {
        XRGrabInteractable objetPose = args.interactableObject.transform.GetComponent<XRGrabInteractable>();
        if (objetPose != null)
        {
            objetPose.interactionLayers = InteractionLayerMask.GetMask("Verrouille");
        }
    }

    public void FigerObjetDansSocket(SelectEnterEventArgs args)
=======
    private void FigerObjetDansSocket(SelectEnterEventArgs args)
>>>>>>> Stashed changes
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