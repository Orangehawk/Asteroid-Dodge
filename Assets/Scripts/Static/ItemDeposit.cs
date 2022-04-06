using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDeposit : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.instance.GetItemCollected())
            {
                GameManager.instance.DepositItem();
            }
        }
    }
}
