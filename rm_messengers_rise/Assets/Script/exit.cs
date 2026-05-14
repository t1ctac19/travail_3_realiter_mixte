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
    public ToggleMoneyOnKey moneyManager;

    [Header("Feedback UI")]
    public GameObject panneauManqueArgent;
    public float dureeAffichageFeedback = 2f;

    [Header("Scene à charger")]
#if UNITY_EDITOR
    public SceneAsset sceneToLoad;
#endif

    [SerializeField] private string sceneName;

    private bool enCooldown = false;

    private void Awake()
    {
        if (panneauManqueArgent != null)
            panneauManqueArgent.SetActive(false);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (sceneToLoad != null)
        {
            sceneName = sceneToLoad.name;
        }
    }
#endif

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || enCooldown)
            return;

        if (moneyManager == null)
        {
            Debug.LogError("moneyManager non assigné !");
            return;
        }

        if (moneyManager.money >= argentRequis)
        {
            moneyManager.RemoveMoney(argentRequis);

            if (!string.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                Debug.LogError("Aucune scène assignée !");
            }
        }
        else
        {
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