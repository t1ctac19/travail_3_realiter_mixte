using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class ToggleMoneyOnKey : MonoBehaviour
{
    [Header("UI References")]
    public GameObject CanvasMoney;
    public TextMeshProUGUI moneyText;

    [Header("Popup Gain Argent")]
    public GameObject popupPrefab;

    [Header("Money Settings")]
    public int money = 0;

    [Header("Pop Animation")]
    public float popScale = 1.4f;
    public float popDuration = 0.2f;

    [Header("Input Action")]
    public InputActionReference toggleMenuAction;

    void Start()
    {
        CanvasMoney.SetActive(false);
        UpdateMoneyText();
        toggleMenuAction.action.Enable();
        toggleMenuAction.action.performed += OnToggleMenu;
    }

    void OnDestroy()
    {
        toggleMenuAction.action.performed -= OnToggleMenu;
    }

    void OnToggleMenu(InputAction.CallbackContext ctx)
    {
        CanvasMoney.SetActive(!CanvasMoney.activeSelf);
    }

    void Update()
    {
        // PC
        if (Input.GetKeyDown(KeyCode.Space))
            CanvasMoney.SetActive(!CanvasMoney.activeSelf);

        if (Input.GetKeyDown(KeyCode.M))
            AddMoney(15);
    }

    public void AddMoney(int amount)
    {
        money += amount;
        UpdateMoneyText();
        StartCoroutine(PopAnimation());
        ShowMoneyPopup(amount);
    }

    public void RemoveMoney(int amount)
    {
        money -= amount;
        UpdateMoneyText();
        StartCoroutine(PopAnimation());
    }

    public void ShowMoneyPopup(int amount)
    {
        if (popupPrefab == null)
        {
            Debug.LogWarning("PopupPrefab manquant !");
            return;
        }

        GameObject popup = Instantiate(popupPrefab, moneyText.transform.parent);
        TextMeshProUGUI text = popup.GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
        {
            text.text = "+" + amount + "<color=#118C4F>$</color>";
        }
        Destroy(popup, 1.5f);
    }

    IEnumerator PopAnimation()
    {
        float elapsed = 0f;
        while (elapsed < popDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / popDuration;
            moneyText.transform.localScale = Vector3.one * Mathf.Lerp(1f, popScale, t);
            yield return null;
        }
        elapsed = 0f;
        while (elapsed < popDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / popDuration;
            moneyText.transform.localScale = Vector3.one * Mathf.Lerp(popScale, 1f, t);
            yield return null;
        }
        moneyText.transform.localScale = Vector3.one;
    }

    void UpdateMoneyText()
    {
        if (moneyText != null)
            moneyText.text = money + "<color=#118C4F>$</color>";
        else
            Debug.LogWarning("Money TextMeshProUGUI reference is missing!");
    }
}