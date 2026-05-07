using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ToggleGrabMovementFix : MonoBehaviour
{
    [SerializeField] XRBaseInteractor m_LeftInteractor;
    [SerializeField] Transform m_ForwardSource; // Main Camera
    [SerializeField] float m_MoveSpeed = 5f;

    CharacterController m_CharacterController;
    bool m_IsGrabActive;
    readonly List<InputDevice> m_LeftDevices = new List<InputDevice>();

    void Awake() => m_CharacterController = GetComponent<CharacterController>();

    void OnEnable()
    {
        m_LeftInteractor.selectEntered.AddListener(OnSelectEntered);
        m_LeftInteractor.selectExited.AddListener(OnSelectExited);
    }

    void OnDisable()
    {
        m_LeftInteractor.selectEntered.RemoveListener(OnSelectEntered);
        m_LeftInteractor.selectExited.RemoveListener(OnSelectExited);
    }

    void OnSelectEntered(SelectEnterEventArgs args) => m_IsGrabActive = true;
    void OnSelectExited(SelectExitEventArgs args)   => m_IsGrabActive = false;

    void Update()
    {
        if (!m_IsGrabActive) return;

        // Lecture directe du hardware XR — XRIT ne peut pas l'intercepter
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, m_LeftDevices);
        if (m_LeftDevices.Count == 0) return;

        m_LeftDevices[0].TryGetFeatureValue(CommonUsages.primary2DAxis, out var input);
        if (input.sqrMagnitude < 0.01f) return;

        var forward = m_ForwardSource != null ? m_ForwardSource.forward 
                                              : Camera.main.transform.forward;
        var right   = m_ForwardSource != null ? m_ForwardSource.right   
                                              : Camera.main.transform.right;
        forward.y = 0f; forward.Normalize();
        right.y   = 0f; right.Normalize();

        var move = (forward * input.y + right * input.x) * m_MoveSpeed * Time.deltaTime;

        if (m_CharacterController != null)
            m_CharacterController.Move(move);
        else
            transform.position += move;
    }
}