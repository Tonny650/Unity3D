using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ThisName_System/Scripts/Items")]
public class Items : ScriptableObject
{
    public int id;
    public string itemName;

    [TextArea(3, 3)] public string descripcion;
    
    public enum Types {
        craftingMaterial,
        equipament,
        misecellaneous
    }
    
    public enum Rarity {
        common,
        uncommon,
        rare,
        epic,
        legandary
    }

    public GameObject prefab;
    public Texture icon;

    public Types type;
    public Rarity rarity;
    public int maxStack;
    public float weight;
    public int baseValue;
}
