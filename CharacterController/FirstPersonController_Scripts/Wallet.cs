using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour {

    #region Variables
    [SerializeField] public Cash _Cash = new Cash();
    //private int money;
    

    #endregion

    public void WalletUI(int money) {
        _Cash.AmountMoney += money;
        _Cash.SetStats();
    }
    
    
}
