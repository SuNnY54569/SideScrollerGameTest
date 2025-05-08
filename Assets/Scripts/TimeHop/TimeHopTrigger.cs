using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeHopTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TimeManager timeManager = FindObjectOfType<TimeManager>();
            if (timeManager != null)
            {
                timeManager.AdvanceTime();
            }
        }
    }
}
