using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour {
    
    [Header("Minimap Parameters")]
    [SerializeField] private Transform player;


    private void LateUpdate() {
        Vector3 newPosition = player.position;

        newPosition.y = transform.position.y;

        transform.position = newPosition;
        
    }
    
    
}
