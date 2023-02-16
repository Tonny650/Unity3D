using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currency : MonoBehaviour {
    [SerializeField] private Wallet _wallet;
    [SerializeField] private int amount;
    [SerializeField] private int value;
    private void OnTriggerEnter(Collider other) {
        _wallet.GetComponent<Wallet>().WalletUI(amount*value);
        Destroy(gameObject);
    }
}
