using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
    public int maxVal;
    public int currVal;

    public Stat(int curr, int max)
    {
        maxVal = max;
        currVal = curr;
    }

    internal void Subtract(int amount)
    {
        currVal -= amount;
    }

    internal void Add(int amount)
    {
        currVal += amount;

        if(currVal > maxVal) { currVal = maxVal; }
    }

    internal void SetToMax()
    {
        currVal = maxVal;
    }
}

public class Character : MonoBehaviour
{
    public Stat hp;
    [SerializeField] StatusBar hpBar;
    public Stat stamina;
    [SerializeField] StatusBar staminaBar;

    public bool isDead;
    public bool isExhausted;

    private void Start()
    {
        UpdateHPBar();
        UpdateStaminaBar();
    }

    #region Update Status Bars
    private void UpdateHPBar()
    {
        hpBar.Set(hp.currVal, hp.maxVal);
    }
    private void UpdateStaminaBar()
    {
        staminaBar.Set(stamina.currVal, stamina.maxVal);
    }

    #endregion

    #region HP

    public void TakeDamage(int amount)
    {
        hp.Subtract(amount);
        if (hp.currVal <= 0)
        {
            isDead = true;
        }
        UpdateHPBar();
    }

    public void Heal(int amount)
    {
        hp.Add(amount);
    }

    public void FullHeal()
    {
        hp.SetToMax();
    }

    #endregion

    #region Stamina

    public void GetTired(int amount)
    {
        stamina.Subtract(amount);
        if(stamina.currVal <= 0)
        {
            isExhausted = true;
        }
    }

    public void Rest(int amount)
    {
        stamina.Add(amount);
    }

    public void FullRest()
    {
        stamina.SetToMax();
    }

    #endregion
}
