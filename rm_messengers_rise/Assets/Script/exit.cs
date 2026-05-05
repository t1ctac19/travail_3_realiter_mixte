using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitZone : MonoBehaviour
{
    public int argentRequis = 100;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Quelque chose est entré dans la zone");
        if (other.CompareTag("Player"))
        {
            ToggleMoneyOnKey moneySysteme = other.GetComponent<ToggleMoneyOnKey>();
            Debug.Log("C'est le joueur !");
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