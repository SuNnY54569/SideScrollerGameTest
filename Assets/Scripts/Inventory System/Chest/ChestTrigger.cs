using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestTrigger : MonoBehaviour
{
    [SerializeField] private Chest chest;

    private bool playerInRange;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Chest chest = GetComponent<Chest>();
        if (chest != null)
        {
            ChestManager.Instance.OpenChest(chest);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (ChestManager.Instance.IsChestOpen())
            {
                ChestManager.Instance.CloseChest();
            }
        }
    }
}
