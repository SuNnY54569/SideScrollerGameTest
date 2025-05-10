using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestTrigger : MonoBehaviour
{
    private bool playerInRange = false;
    private Chest chest;

    private void Awake()
    {
        chest = GetComponent<Chest>();
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (ChestUI.Instance.gameObject.activeSelf)
                ChestUI.Instance.Close();
            else
                ChestUI.Instance.Open(chest);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && ChestUI.Instance.gameObject.activeSelf)
        {
            ChestUI.Instance.Close();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
