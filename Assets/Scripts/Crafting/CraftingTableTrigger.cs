using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTableTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CraftingManager>()?.SetNearWorkbench(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CraftingManager>()?.SetNearWorkbench(false);
        }
    }
}
