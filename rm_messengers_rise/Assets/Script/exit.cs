using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public int argentRequis = 100;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ToggleMoneyOnKey moneySysteme = other.GetComponent<ToggleMoneyOnKey>();

            if (moneySysteme != null)
            {
                if (moneySysteme.money >= argentRequis)
                {
                    Debug.Log("Tu peux sortir !");
                    
                    // Exemple :
                    // SceneManager.LoadScene("NextLevel");
                }
                else
                {
                    Debug.Log("Pas assez d'argent !");
                }
            }
            else
            {
                Debug.Log("Script ToggleMoneyOnKey non trouvé sur le joueur !");
            }
        }
    }
}