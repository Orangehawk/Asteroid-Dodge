using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    Text creditsText;
    [SerializeField]
    Text livesText;


	void OnEnable()
	{
        if(creditsText != null)
            creditsText.text = $"${GameManager.instance.GetCredits()}";

        if (livesText != null)
            livesText.text = $"{PlayerShip.instance.GetLives()}";
    }
}
