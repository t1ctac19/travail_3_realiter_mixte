using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExemplePointage : MonoBehaviour
{
    public HighScorePersistant highScore;

    void Update()
    {
        highScore.OnChangerPointage(Mathf.FloorToInt(Time.time));
    }
}
