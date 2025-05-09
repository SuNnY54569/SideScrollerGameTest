using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArcanePower : MonoBehaviour
{
    [SerializeField] private int maxAP = 100;
    [SerializeField] private int currentAP;
    [SerializeField] private int apPerShot = 10;
    [SerializeField] private bool enableRegen = true;
    [SerializeField] private float regenTickInterval = 1f;
    [SerializeField] private int regenAmountPerTick = 5;
    [SerializeField] private float regenDelay = 1f;
    
    public int MaxAP => maxAP;
    private float lastUseTime;
    private float nextRegenTime;
    
    public UnityEvent<int> OnAPChanged;
    
    private void Awake()
    {
        ResetAP();
    }

    private void Update()
    {
        if (!enableRegen || currentAP >= maxAP) return;

        if (Time.time - lastUseTime >= regenDelay)
        {
            if (Time.time >= nextRegenTime)
            {
                currentAP += regenAmountPerTick;
                currentAP = Mathf.Min(currentAP, maxAP);
                OnAPChanged?.Invoke(currentAP);

                nextRegenTime = Time.time + regenTickInterval;
            }
        }
    }

    public bool CanUse()
    {
        return currentAP >= apPerShot;
    }
    
    public void UseAP()
    {
        if (!CanUse()) return;

        currentAP -= apPerShot;
        lastUseTime = Time.time;
        nextRegenTime = Time.time + regenDelay;
        OnAPChanged?.Invoke(currentAP);
    }
    
    public void ResetAP()
    {
        currentAP = maxAP;
        OnAPChanged?.Invoke(currentAP);
    }
    
    public int GetCurrentAP()
    {
        return currentAP;
    }
    
    public void SetRegenEnabled(bool enabled)
    {
        enableRegen = enabled;
    }
}
