using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryViewController : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryViewObject;
    [SerializeField] private GameObject _contextMenuObject;
    [SerializeField] private GameObject _firstContextMenuOption;
    [SerializeField] private TMP_Text _itemNameText;
    [SerializeField] private TMP_Text _itemDescriptionText;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private List<ItemSlot> _slots;
    [SerializeField] private ItemSlot _currentSlot;
    [SerializeField] private ScreenFader _fader;
    [SerializeField] private List<Button> _contextMenuIgnore;

    Player player;

    private enum State
    {
        menuClosed,
        menuOpen,
        contextMenu,
    };

    private State _state;

    public void UseItem()
    {
        _fader.FadeToBlack(1f, FadeToUseItemCallback);
    }

    public void FadeToUseItemCallback()
    {
        _contextMenuObject.SetActive(false);
        _inventoryViewObject.SetActive(false);
        characterController.enabled = true;
        _fader.FadeFromBlack(1f, () => EventBus.Instance.UseItem(_currentSlot.itemData));
        EventBus.Instance.UseItem(_currentSlot.itemData);
        foreach (var button in _contextMenuIgnore)
        {
            button.interactable = true;
        }
        _state = State.menuClosed;
    }

    private void OnEnable()
    {
        playercontroller.Menu.Enable();
        EventBus.Instance.onPickUpItem += onItemPickedUp;
    }

    private void OnDisable()
    {
        playercontroller.Menu.Disable();
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

    public void OnSlotSelected(ItemSlot selectedSlot)
    {
        _currentSlot = selectedSlot;
        if (selectedSlot.itemData == null)
        {
            _itemNameText.ClearMesh();
            _itemDescriptionText.ClearMesh();
            return;
        }

        _itemNameText.SetText(selectedSlot.itemData.Name);
        _itemDescriptionText.SetText(selectedSlot.itemData.Description[0]);
    }

    private PlayerController playercontroller;
    private InputAction menu;

    public bool isPaused;

    private void Awake()
    {
        playercontroller = new PlayerController();
        characterController = FindAnyObjectByType<CharacterController>();
        player = FindAnyObjectByType<Player>();
    }

    private void Update()
    {
        //Open context menu
        if (player.isItemPickUp)
        {
            if (_state == State.menuOpen)
            {
                if (EventSystem.current.currentSelectedGameObject.TryGetComponent<ItemSlot>(out var slot))
                {
                    _state = State.contextMenu;
                    _contextMenuObject.SetActive(true);
                    EventSystem.current.SetSelectedGameObject(_firstContextMenuOption);
                    foreach (var button in _contextMenuIgnore)
                    {
                        button.interactable = false;
                    }
                }
            }
        }

        if (playercontroller.Menu.Inventory.WasPressedThisFrame())
        {
            if (_state == State.menuClosed)
            {
                EventBus.Instance.PauseGameplay();
                _fader.FadeToBlack(0.3f, FadeToMenuCallback);
                _state = State.menuOpen;
            }
            else if (_state == State.menuOpen)
            {
                _fader.FadeToBlack(0.3f, FadeFromMenuCallback);
                _state = State.menuClosed;
            }
            else if (_state == State.contextMenu)
            {
                _contextMenuObject.SetActive(false);
                foreach (var button in _contextMenuIgnore)
                {
                    button.interactable = true;
                }
                EventSystem.current.SetSelectedGameObject(_currentSlot.gameObject);
                _state = State.menuOpen;
            }
        }
    }

    private void FadeToMenuCallback()
    {
        _inventoryViewObject.SetActive(true);
        characterController.enabled = false;
        _fader.FadeFromBlack(0.3f, null);
    }

    private void FadeFromMenuCallback()
    {
        _inventoryViewObject.SetActive(false);
        characterController.enabled = true;
        _fader.FadeFromBlack(0.3f, EventBus.Instance.ResumeGameplay);
    }
}
