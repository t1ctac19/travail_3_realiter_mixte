using UnityEngine;
using TMPro;
using System.Collections;

public class ToggleMoneyOnKey : MonoBehaviour
{
    [Header("UI References")]
    public GameObject CanvasMoney;
    public TextMeshProUGUI moneyText;

    [Header("Money Settings")]
    public int money = 0;

    [Header("Pop Animation")]
    public float popScale = 1.4f;
    public float popDuration = 0.2f;

    // INITIALIZATION MONEY
    void Start()
    {
        CanvasMoney.SetActive(false);
        UpdateMoneyText();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
            CanvasMoney.SetActive(!CanvasMoney.activeSelf);

        if (Input.GetKeyDown(KeyCode.Space))
            AddMoney(10);
    }

    public void AddMoney(int amount)
    {
        money += amount;
        UpdateMoneyText();
        StartCoroutine(PopAnimation());
    }

    public void RemoveMoney(int amount)
    {
        money -= amount;
        UpdateMoneyText();
        StartCoroutine(PopAnimation());
    }

    IEnumerator PopAnimation()
    {
        float elapsed = 0f;

        // Grossit
        while (elapsed < popDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / popDuration;
            float scale = Mathf.Lerp(1f, popScale, t);
            moneyText.transform.localScale = Vector3.one * scale;
            yield return null;
        }

        elapsed = 0f;

        // Rétrécit
        while (elapsed < popDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / popDuration;
            float scale = Mathf.Lerp(popScale, 1f, t);
            moneyText.transform.localScale = Vector3.one * scale;
            yield return null;
        }

        moneyText.transform.localScale = Vector3.one;
    }

    // UPDATE MONEY TEXT
    void UpdateMoneyText()
    {
        if (moneyText != null)
            moneyText.text = money + "<color=#118C4F>$</color>";
        else
            Debug.LogWarning("Money TextMeshProUGUI reference is missing!");
    }
}
