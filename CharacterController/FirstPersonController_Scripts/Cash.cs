using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cash : MonoBehaviour
{
    public int AmountMoney = 100;
    
    TextMeshProUGUI tex_amount;

    public void SetStats()
    {

        tex_amount = GetComponentInChildren<TextMeshProUGUI>();
        tex_amount.text = $"${AmountMoney}";

    }
}
