using UnityEngine;
using TMPro;

public class ToggleMoneyOnKey : MonoBehaviour
{
    [Header("UI References")]
    public GameObject CanvasMoney;
    public TextMeshProUGUI moneyText;

    [Header("Money Settings")]
    public int money = 0;

    // INITIALIZATION MONEY
    void Start()
    {
        CanvasMoney.SetActive(false);
        UpdateMoneyText();
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.X))
        {
            CanvasMoney.SetActive(!CanvasMoney.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            money += 10;
            UpdateMoneyText();
        }
    }

    // UPDATE MONEY TEXT
    void UpdateMoneyText()
    {
        if (moneyText != null)
        {
            moneyText.text = money + "<color=#118C4F>$</color>";
        }
        else
        {
            Debug.LogWarning("Money TextMeshProUGUI reference is missing!");
        }
    }
}
