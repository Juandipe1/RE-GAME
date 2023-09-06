using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class InventoryViewController : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryViewObject;
    [SerializeField] private TMP_Text _itemNameText;
    [SerializeField] private TMP_Text _itemDescriptionText;
    [SerializeField] private CharacterController player;
    [SerializeField] private List<ItemSlot> _slots;

    private PlayerController playercontroller;
    private InputAction menu;

    public bool isPaused;

    private void Awake()
    {
        playercontroller = new PlayerController();
        player = FindAnyObjectByType<CharacterController>();
    }

    public void OnSlotSelected(ItemSlot selectedSlot)
    {
        if (selectedSlot.itemData == null)
        {
            _itemNameText.ClearMesh();
            _itemDescriptionText.ClearMesh();
            return;
        }

        _itemNameText.SetText(selectedSlot.itemData.Name);
        _itemDescriptionText.SetText(selectedSlot.itemData.Description[0]);
    }

    private void Update()
    {
        
    }

    private void OnEnable()
    {
        EventBus.Instance.onPickUpItem += onItemPickedUp;
        menu = playercontroller.Menu.Inventory;
        menu.Enable();

        menu.performed += Inventory;
    }

    private void OnDisable()
    {
        menu.Disable();
        EventBus.Instance.onPickUpItem -= onItemPickedUp;
    }

    private void onItemPickedUp(ItemData itemData)
    {
        foreach (var slot in _slots)
        {
            if (slot.IsEmpty())
            {
                slot.itemData = itemData;
                break;
            }
        }
    }

    void Inventory(InputAction.CallbackContext context)
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            if (_inventoryViewObject.activeSelf)
            {
                EventBus.Instance.OpenInventory();
            }
            else
            {
                EventBus.Instance.CloseInventory();
            }
            _inventoryViewObject.SetActive(true);
        }
        else
        {
            if (_inventoryViewObject.activeSelf)
            {
                EventBus.Instance.OpenInventory();
            }
            else
            {
                EventBus.Instance.CloseInventory();
            }
            _inventoryViewObject.SetActive(false);
        }
    }
}
