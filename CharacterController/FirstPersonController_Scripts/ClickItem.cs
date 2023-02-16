using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClickItem : MonoBehaviour
{
    public Slot clickedSlot;
    public Color[] rarityColors;
    
    [SerializeField] RawImage _image;

    [SerializeField] private TextMeshProUGUI txt_name;
    [SerializeField] private TextMeshProUGUI txt_Rarity;
    [SerializeField] private TextMeshProUGUI txt_Waight;
    [SerializeField] private TextMeshProUGUI txt_Value;
    [SerializeField] private TextMeshProUGUI txt_Type;
    [SerializeField] private TextMeshProUGUI txt_Stack;
    [SerializeField] private TextMeshProUGUI txt_Description;
    
    void OnEnable() {
        SetUp();
    }

    void SetUp()
    {
        txt_name.text = clickedSlot.ItemInSlot.name;
        txt_Waight.text = $"{clickedSlot.ItemInSlot.weight * clickedSlot.AmountInSlot}kg";
        txt_Stack.text = $"{clickedSlot.AmountInSlot}/{clickedSlot.ItemInSlot.maxStack}";
        txt_Value.text = $"{clickedSlot.AmountInSlot * clickedSlot.ItemInSlot.baseValue}";
        txt_Description.text = clickedSlot.ItemInSlot.descripcion;
        
        switch (clickedSlot.ItemInSlot.rarity) {
            
            case Items.Rarity.common:
                _image.color = rarityColors[0];
                txt_Rarity.text = "Common";
                break;
            case Items.Rarity.uncommon:
                _image.color = rarityColors[0];
                txt_Rarity.text = "Uncommon";
                break;
            case Items.Rarity.rare:
                _image.color = rarityColors[0];
                txt_Rarity.text = "Rare";
                break;
            case Items.Rarity.epic:
                _image.color = rarityColors[0];
                txt_Rarity.text = "Epic";
                break;
            case Items.Rarity.legandary:
                _image.color = rarityColors[0];
                txt_Rarity.text = "Legendary";
                break;
            default:
                break;
        }

        switch (clickedSlot.ItemInSlot.type) {
            case Items.Types.misecellaneous:
                txt_Type.text = "Misecellaneous";
                break;
            case Items.Types.craftingMaterial:
                txt_Type.text = "Crafting Material";
                break;
            case Items.Types.equipament:
                txt_Type.text = "Equipament";
                break;
            default:
                break;
        }
    }
}
