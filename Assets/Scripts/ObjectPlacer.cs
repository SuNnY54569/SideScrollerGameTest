using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField] private QuickBar quickBar;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            PlaceSelectedItem();
            Debug.Log("Place");
        }
    }
    
    private void PlaceSelectedItem()
    {
        var slot = quickBar.GetSelectedSlot();
        if (slot == null || slot.IsEmpty) return;

        var item = slot.item;
        if (item.isPlaceable && item.placeablePrefab!= null)
        {
            Debug.Log("work");
            Vector3 placePosition = transform.position;
            Instantiate(item.placeablePrefab, placePosition, Quaternion.identity);

            slot.RemoveItem(1);
        }
    }
}
