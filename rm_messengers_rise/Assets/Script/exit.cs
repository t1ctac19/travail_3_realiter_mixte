using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ExitZone : MonoBehaviour
{
    public int argentRequis = 100;

    #if UNITY_EDITOR
    public SceneAsset sceneToLoad; // Drag & drop ici
    #endif

    private string sceneName;

    private void Awake()
    {
        #if UNITY_EDITOR
        if (sceneToLoad != null)
        {
            sceneName = sceneToLoad.name;
        }
        #endif
    }

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

                    if (!string.IsNullOrEmpty(sceneName))
                    {
                        SceneManager.LoadScene(sceneName);
                    }
                    else
                    {
                        Debug.Log("Aucune scène assignée !");
                    }
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