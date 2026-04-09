using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitterScript : MonoBehaviour
{
    public void quitterPartie()
    {
        Debug.Log("Quitter");
        Application.Quit();
    }
}
