using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler
{
   public Items ItemInSlot;
   public int AmountInSlot;

   RawImage icon;
   TextMeshProUGUI tex_amount;
   
   public void SetStats()
   {
      
      for (int i = 0; i < transform.childCount; i++) {
         transform.GetChild(i).gameObject.SetActive(true);
      }

      icon = GetComponentInChildren<RawImage>();
      tex_amount = GetComponentInChildren<TextMeshProUGUI>();

      if (ItemInSlot == null) {
         for (int i = 0; i < transform.childCount; i++) {
            transform.GetChild(i).gameObject.SetActive(false);
         }
         return;
      }

      icon.texture = ItemInSlot.icon;
      tex_amount.text = $"{AmountInSlot}x";
   }

   public void OnDrop(PointerEventData eventData) {
      GameObject dropped = eventData.pointerDrag;
      

   }
}
