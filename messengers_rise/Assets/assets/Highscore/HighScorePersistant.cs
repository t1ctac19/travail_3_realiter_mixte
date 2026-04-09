using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScorePersistant : MonoBehaviour
{
    public TextMeshProUGUI texteHighScore;
    public TextMeshProUGUI textePointage;

    public string highscoreClef = "HIGHSCORE";

    private int _highScoreActuel = 0;
    private int _pointageActuel = 0;

    void Start()
    {
        _pointageActuel = 0;
        _highScoreActuel = PlayerPrefs.GetInt(highscoreClef, 0);

        texteHighScore.text = _highScoreActuel.ToString();
        textePointage.text = _pointageActuel.ToString();
    }

    public void OnChangerPointage(int nouvellePointage)
    {
        _pointageActuel = nouvellePointage;

        if (_pointageActuel > _highScoreActuel)
        {
            _highScoreActuel = _pointageActuel;
            PlayerPrefs.SetInt(highscoreClef, _highScoreActuel);
        }

        textePointage.text = _pointageActuel.ToString();
        texteHighScore.text = _highScoreActuel.ToString();
    }
}
