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
    public int level = 1;

    public Stat hp;
    [SerializeField] StatusBar hpBar;
    public Stat stamina;
    [SerializeField] StatusBar staminaBar;
    public Stat xp;
    [SerializeField] StatusBar xpBar;
    public Stat mana;
    [SerializeField] StatusBar manaBar;

    public bool isDead;
    public bool isExhausted;
    public bool isNoManaLeft;

    private void Start()
    {
        //UpdateHPBar();
        //UpdateStaminaBar();
        //UpdateXPBar();
        //UpdateManaBar();
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
    private void UpdateXPBar()
    {
        xpBar.Set(xp.currVal, xp.maxVal);
    }
    private void UpdateManaBar()
    {
        manaBar.Set(mana.currVal, mana.maxVal);
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

    #region Mana

    public void UseMana(int amount)
    {
        mana.Subtract(amount);
        if(mana.currVal <= 0)
        {
            mana.currVal = 0;
            isNoManaLeft = true;
        }
    }

    public void RegenMana(int amount)
    {
        mana.Add(amount);
        if(mana.currVal >= mana.maxVal)
        {
            mana.currVal = mana.maxVal;
        }
    }

    public void FullRegenMana()
    {
        mana.SetToMax();
    }

    #endregion

    #region XP

    public void IncreaseXP(int amount)
    {
        xp.Add(amount);
        if(xp.currVal >= xp.maxVal)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        xp.currVal = 0;
        level += 1;
    }

    #endregion
}
