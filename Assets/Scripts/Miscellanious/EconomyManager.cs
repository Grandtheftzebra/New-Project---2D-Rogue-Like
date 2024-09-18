using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EconomyManager : Singleton<EconomyManager>
{
    private TMP_Text _goldText;
    private int _currentGold = 0;
    
    const string COIN_AMOUNT_TEXT = "Gold_Amount_Text";

    public void UpdateCurrentGold(int amount)
    {
        _currentGold += amount; 
        
        if (_goldText == null)
            _goldText = GameObject.Find(COIN_AMOUNT_TEXT).GetComponent<TMP_Text>();
        
        _goldText.text = _currentGold.ToString("D3"); // D3 means 3 digits
    }
}
