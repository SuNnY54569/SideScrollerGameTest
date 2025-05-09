using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DragDropManager : MonoBehaviour
{
    public static DragDropManager Instance { get { return _instance; } }
    private static DragDropManager _instance; 
    
    [Header("Drag Icon")]
    public GameObject dragIconPrefab;
    private GameObject dragIconInstance;
    private Image iconImage;
    private TMP_Text countText;
    
    [HideInInspector] public int sourceSlotIndex;
    [HideInInspector] public Inventory sourceInventory;
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        if (_instance == null)
        {
            _instance = this;
        }
        
        
        dragIconInstance = Instantiate(dragIconPrefab, transform);
        iconImage = dragIconInstance.GetComponentInChildren<Image>();
        countText = dragIconInstance.GetComponentInChildren<TMP_Text>();
        dragIconInstance.SetActive(false);
    }
    
    public void StartDrag(Inventory inventory, int slotIndex, Sprite icon, string count)
    {
        sourceInventory = inventory;
        sourceSlotIndex = slotIndex;
        iconImage.sprite = icon;
        countText.text = count;
        dragIconInstance.SetActive(true);
    }
    
    public void EndDrag()
    {
        dragIconInstance.SetActive(false);
    }
    
    private void Update()
    {
        if (dragIconInstance.activeSelf)
        {
            dragIconInstance.transform.position = Input.mousePosition;
        }
    }
}
