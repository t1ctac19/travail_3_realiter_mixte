// exit.cs
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ExitZone : MonoBehaviour
{
    [Header("Configuration")]
    public int argentRequis = 50;

    [Header("Références")]
    public ToggleMoneyOnKey moneyManager; // Source de vérité unique

    [Header("Feedback UI (optionnel)")]
    public GameObject panneauManqueArgent; // Panel "Pas assez d'argent !"
    public float dureeAffichageFeedback = 2f;

#if UNITY_EDITOR
    public SceneAsset sceneToLoad;
#endif

    private string sceneName;
    private bool enCooldown = false; // Évite les doubles triggers

    private void Awake()
    {
#if UNITY_EDITOR
        if (sceneToLoad != null)
            sceneName = sceneToLoad.name;
#endif
        if (panneauManqueArgent != null)
            panneauManqueArgent.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || enCooldown) return;

        if (moneyManager == null)
        {
            Debug.LogError("moneyManager non assigné dans ExitZone !");
            return;
        }

        // Vérifie si le joueur a assez d'argent
        if (moneyManager.money >= argentRequis)
        {
            Debug.Log($"Achat du passage ({argentRequis}$) — Argent avant : {moneyManager.money}$");

            //  Décrémente le montant exact du passage
            moneyManager.RemoveMoney(argentRequis);

            Debug.Log($"Argent après achat : {moneyManager.money}$");

            // Charge la scène suivante
            if (!string.IsNullOrEmpty(sceneName))
                SceneManager.LoadScene(sceneName);
            else
                Debug.LogWarning("Aucune scène assignée dans sceneToLoad !");
        }
        else
        {
            //  Pas assez d'argent
            int manque = argentRequis - moneyManager.money;
            Debug.Log($"Pas assez d'argent ! Il manque {manque}$ pour passer.");

            // Affiche le feedback UI si assigné
            if (panneauManqueArgent != null)
                StartCoroutine(AfficherFeedback());
        }
    }

    private System.Collections.IEnumerator AfficherFeedback()
    {
        enCooldown = true;
        panneauManqueArgent.SetActive(true);
        yield return new WaitForSeconds(dureeAffichageFeedback);
        panneauManqueArgent.SetActive(false);
        enCooldown = false;
    }
}