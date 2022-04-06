using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    Text creditsText;


	void OnEnable()
	{
        creditsText.text = $"${GameManager.instance.creditsAmount}";
    }
}
