using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHP = 100;
    private int currentHP;
    
    public UnityEvent<int> OnHealthChanged;
    
    private void Awake()
    {
        currentHP = maxHP;
        OnHealthChanged?.Invoke(currentHP);
    }
    
    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        currentHP = Mathf.Max(0, currentHP);
        OnHealthChanged?.Invoke(currentHP);

        if (currentHP <= 0)
        {
            Debug.Log("Player died");
        }
    }
    
    public void ResetHealth()
    {
        currentHP = maxHP;
        OnHealthChanged?.Invoke(currentHP);
    }
}
