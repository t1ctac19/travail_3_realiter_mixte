using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MinuterieEvents : MonoBehaviour
{
    public int duration = 60;
    public bool compterLeZero = false;
    private TextMeshProUGUI _texte;
    private int _compteur;

    public UnityEvent<int> onChaqueSecond;
    public UnityEvent onTermine;

    void Start()
    {
        _texte = GetComponentInChildren<TextMeshProUGUI>();
        StartCoroutine("CommencerMinuterie");
    }

    IEnumerator CommencerMinuterie()
    {
        _compteur = duration;
        _texte.text = _compteur.ToString();

        while (_compteur >= 1)
        {
            yield return new WaitForSeconds(1f);
            _compteur -= 1;
            onChaqueSecond.Invoke(_compteur);
            _texte.text = _compteur.ToString();
        }

        if (compterLeZero == false) { 
            yield return new WaitForSeconds(1f);
        }
        onTermine.Invoke();
    }

}
