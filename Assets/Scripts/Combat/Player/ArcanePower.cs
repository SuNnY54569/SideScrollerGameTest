using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArcanePower : MonoBehaviour
{
    [Header("Arcane Power Settings")]
    [SerializeField] private int maxAP = 100;
    [SerializeField] private int currentAP;
    [SerializeField] private int apPerShot = 10;
    
    [Header("Regeneration Settings")]
    [SerializeField] private bool enableRegen = true;
    [SerializeField] private float regenTickInterval = 1f;
    [SerializeField] private int regenAmountPerTick = 5;
    [SerializeField] private float regenDelay = 1f;
    
    private float lastUseTime;
    private float nextRegenTime;
    public int MaxAP => maxAP;
    public int CurrentAP => currentAP;
    
    public UnityEvent<int> OnAPChanged;
    
    private void Awake()
    {
        ResetAP();
    }

    private void Update()
    {
        if (!enableRegen || currentAP >= maxAP) return;

        if (Time.time - lastUseTime >= regenDelay && Time.time >= nextRegenTime)
        {
            RegenerateAP();
        }
    }
    
    private void RegenerateAP()
    {
        currentAP = Mathf.Min(currentAP + regenAmountPerTick, maxAP);
        OnAPChanged?.Invoke(currentAP);
        nextRegenTime = Time.time + regenTickInterval;
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
    
    public void SetRegenEnabled(bool enabled)
    {
        enableRegen = enabled;
    }
    
    public bool CanUse() => currentAP >= apPerShot;
    public int GetCurrentAP() => currentAP;
}
