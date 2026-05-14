using UnityEngine;

public class FootstepManager : MonoBehaviour
{
    public AudioSource audioSource;
    public float vitesseSeuil = 0.1f; // Sensibilité du mouvement
    public float intervallePas = 0.5f; // Temps entre deux bruits de pas

    private float timer;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // On vérifie si le joueur se déplace (vitesse horizontale)
        Vector3 vitesseHorizontale = new Vector3(controller.velocity.x, 0, controller.velocity.z);

        if (vitesseHorizontale.magnitude > vitesseSeuil)
        {
            timer += Time.deltaTime;

            if (timer >= intervallePas)
            {
                JouerSonDePas();
                timer = 0;
            }
        }
        else
        {
            timer = 0; // Réinitialise si on s'arrête
        }
    }

    void JouerSonDePas()
    {
        // Change légèrement le pitch pour que ce ne soit pas répétitif
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(audioSource.clip);
    }
}