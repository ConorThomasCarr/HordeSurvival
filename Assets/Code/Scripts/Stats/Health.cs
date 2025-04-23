using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public float health = 100f;
    public float maxHealth = 100f; 
    
    
    private Image _healthBar;
    
    public void TakeDamage(float damage)
    {
        health -= damage;
        
        _healthBar.fillAmount = health / maxHealth;
    }

    private void Start()
    {
        _healthBar = transform.parent.Find("Character UI").transform.Find("Health Bar").GetChild(1)
            .GetComponent<Image>();
    }
}
