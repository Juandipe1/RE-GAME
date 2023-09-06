using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class InventoryViewController : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryViewObject;
    [SerializeField] private GameObject _contextMenuObject;
    [SerializeField] private TMP_Text _itemNameText;
    [SerializeField] private TMP_Text _itemDescriptionText;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private List<ItemSlot> _slots;
    [SerializeField] private ScreenFader _fader;

    Player player;

    private enum State
    {
        menuClosed,
        menuOpen,
    }

    private State _state;

    private PlayerController playercontroller;
    private InputAction menu;

    public bool isPaused;

    private void Awake()
    {
        playercontroller = new PlayerController();
        characterController = FindAnyObjectByType<CharacterController>();
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
        menu = playercontroller.Menu.Inventory;
        menu.Enable();

        menu.performed += Inventory;
        EventBus.Instance.onPickUpItem += onItemPickedUp;
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
            if (_state == State.menuClosed)
            {
                FadeToMenuCallback();
                _fader.FadeToBlack(0.3f, FadeToMenuCallback);
                _state = State.menuOpen;
                characterController.enabled = false;
            }

        }
        else
        {
            if (_state == State.menuOpen)
            {
                FadeFromMenuCallback();
                _fader.FadeToBlack(0.3f, FadeFromMenuCallback);
                _state = State.menuClosed;
                characterController.enabled = true;
            }

        }

        if (player.isItemPickUp)
        {
            
        }
    }

    private void FadeToMenuCallback()
    {
        _inventoryViewObject.SetActive(true);
        _fader.FadeFromBlack(0.3f, EventBus.Instance.PauseGameplay);
    }

    private void FadeFromMenuCallback()
    {
        _inventoryViewObject.SetActive(false);
        _fader.FadeFromBlack(0.3f, EventBus.Instance.ResumeGameplay);
    }
}
