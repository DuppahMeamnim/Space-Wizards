using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomStart : MonoBehaviour
{
    public Tilemap[] doorsTilemaps;
    private bool isActive = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Set this room's tilemaps to active order
            SetLayerOrder();
            isActive = true;
        }
    }

    private void SetLayerOrder()
    {
        foreach (Tilemap tilemap in doorsTilemaps)
        {
            if (tilemap != null)
            {
                tilemap.GetComponent<TilemapRenderer>().sortingOrder = 10;
                tilemap.GetComponent<TilemapCollider2D>().isTrigger = false;
            }
        }
    }
}