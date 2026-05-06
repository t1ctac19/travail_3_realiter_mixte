using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ExitZone : MonoBehaviour
{
    [Header("Configuration")]
    public int argentRequis = 100;
    
    // La variable porte maintenant le bon nom
    public SocketRandomizer socketManager; 

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
        Debug.Log("Quelque chose est entré dans la zone : " + other.name);
        
        if (other.CompareTag("Player"))
        {
            Debug.Log("C'est le joueur !");

            if (socketManager != null)
            {
                // On vérifie directement l'argent dans ton SocketManager
                if (socketManager.argentTotal >= argentRequis)
                {
                    Debug.Log("Tu as assez d'argent, tu peux sortir !");

                    if (!string.IsNullOrEmpty(sceneName))
                    {
                        SceneManager.LoadScene(sceneName);
                    }
                    else
                    {
                        Debug.LogWarning("Aucune scène assignée dans sceneToLoad !");
                    }
                }
                else
                {
                    Debug.Log("Pas assez d'argent ! Il te manque " + (argentRequis - socketManager.argentTotal) + "$");
                }
            }
            else
            {
                Debug.LogError("Attention : Le script SocketManager n'est pas assigné dans l'inspecteur !");
            }
        }
    }
}