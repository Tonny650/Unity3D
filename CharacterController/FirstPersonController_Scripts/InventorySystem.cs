using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{

    #region Variables
    [SerializeField] public Slot[] slots = new Slot[40];
    [SerializeField] GameObject InventoryUI;
    [SerializeField] private GameObject MinimapUI;
    [SerializeField] private GameObject _controller;
    
    
    
    #endregion
    
    #region Custom Controls

    [Header("Controls")] 
    [SerializeField] private KeyCode avtiveKey = KeyCode.Tab;

    #endregion
    

    #region Build

    void Awake() {
        
        for (int i = 0; i < slots.Length; i++) {

            if (slots[i].ItemInSlot == null) {
                
                for (int j = 0; j < slots[i].transform.childCount; j++) {
                    slots[i].transform.GetChild(j).gameObject.SetActive(false);    
                }
            }
        }
    }

    
    void Update() {
        
        if (!InventoryUI.activeInHierarchy && Input.GetKeyDown(avtiveKey)) {
            MinimapUI.SetActive(false);
            inventory();
            InventoryUI.SetActive(true);
        } else if (InventoryUI.activeInHierarchy && Input.GetKeyDown(avtiveKey)) {
            InventoryUI.SetActive(false);
            MinimapUI.SetActive(true);
            inventory();
        }

        
    }

    public void PickUpItem(ItemObject obj) {
        
        for (int i = 0; i < slots.Length; i++) {
            if (slots[i].ItemInSlot != null && slots[i].ItemInSlot.id == obj.itemStats.id && slots[i].AmountInSlot != slots[i].ItemInSlot.maxStack) {
                if (!WillHitMaxStack(i,obj.amount)) {
                    slots[i].AmountInSlot += obj.amount;
                    Destroy(obj.gameObject);
                    slots[i].SetStats();
                    return;
                } else {
                    int result = NeededToFill(i);
                    obj.amount = RemainigAmount(i, obj.amount);
                    slots[i].AmountInSlot += result;
                    slots[i].SetStats();
                    PickUpItem(obj);
                    return;
                }
            } else if (slots[i].ItemInSlot == null) {
                slots[i].ItemInSlot = obj.itemStats;
                slots[i].AmountInSlot += obj.amount;
                Destroy(obj.gameObject);
                slots[i].SetStats();
                return;
            }
            
        }
        
        
    }
    
    
    

    private void inventory() {
        _controller.GetComponent<FirstPersonController>().Inventory();
    }

    #endregion
    
    #region Custom Function

    bool WillHitMaxStack(int index, int amount) {
        if (slots[index].ItemInSlot.maxStack <= slots[index].AmountInSlot + amount) {
            return true;
        } else {
            return false;
        }
    }

    int NeededToFill(int index) {
        return slots[index].ItemInSlot.maxStack - slots[index].AmountInSlot;
    }

    int RemainigAmount(int index, int amount) {
        return (slots[index].AmountInSlot + amount) - slots[index].ItemInSlot.maxStack;
    }
    
    #endregion

    
    
}
