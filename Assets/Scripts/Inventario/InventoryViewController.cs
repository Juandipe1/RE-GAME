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
    [SerializeField] private GameObject _yesNoPanel;
    [SerializeField] private GameObject _itemViewPanel;
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
        EventBus.Instance.onItemPrompt += onItemPrompted;
        EventBus.Instance.onOpenInventory += OpenMenu;
    }


    private void OnDisable()
    {
        playercontroller.Menu.Disable();
        EventBus.Instance.onPickUpItem -= onItemPickedUp;
        EventBus.Instance.onItemPrompt -= onItemPrompted;
        EventBus.Instance.onOpenInventory -= OpenMenu;
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

    private void onItemPrompted(ItemData item)
    {
        var itemView = Instantiate<Image>(item.Sprite, _itemViewPanel.transform.position, Quaternion.identity, _itemViewPanel.transform);
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

    public void onPushSlot()
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

    private PlayerController playercontroller;

    public bool isPaused;

    private void Awake()
    {
        playercontroller = new PlayerController();
        characterController = FindAnyObjectByType<CharacterController>();
        player = FindAnyObjectByType<Player>();
    }

    private void Update()
    {

        if (playercontroller.Menu.Inventory.WasPressedThisFrame())
        {
            if (_state == State.menuClosed)
            {
                OpenMenu(null);
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

    private void OpenMenu(Action finishedCallback)
    {
        EventBus.Instance.PauseGameplay();
        _fader.FadeToBlack(0.3f, () => FadeToMenuCallback(finishedCallback));
        _state = State.menuOpen;
    }

    private void FadeToMenuCallback(Action finishedCallback)
    {
        _inventoryViewObject.SetActive(true);
        characterController.enabled = false;
        _fader.FadeFromBlack(0.3f, finishedCallback);
    }

    private void FadeFromMenuCallback()
    {
        _inventoryViewObject.SetActive(false);
        characterController.enabled = true;
        _fader.FadeFromBlack(0.3f, EventBus.Instance.ResumeGameplay);
    }

    public void FadeMenuClosed()
    {
        if (_state == State.menuOpen)
        {
            _fader.FadeToBlack(0.3f, FadeFromMenuCallback);
            _state = State.menuClosed;
        }
    }
}
