using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [SerializeField] private ItemData _itemData;
    Player player;
    private bool _isBusy;

    private void Start()
    {
        player = FindAnyObjectByType<Player>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (_isBusy) return;
        if (!other.CompareTag("Player")) return;

        if (player.isItemPickUp)
        {
            EventBus.Instance.PauseGameplay();
            DialogePrinter.Instance.PrintDialogueLine(_itemData.WorldDescription, 0.04f, OnDescriptionFinishedPrinting);
            _isBusy = true;
        }
    }

    private void OnDescriptionFinishedPrinting()
    {
        EventBus.Instance.OpenInventory(onInventoryOpenAndReady);
        EventBus.Instance.PickUpItem(_itemData);
        Destroy(gameObject);
    }

    private void onInventoryOpenAndReady()
    {
        EventBus.Instance.PrompItemPickUp(_itemData);
        DialogePrinter.Instance.PrintDialogueLine("Will you take the " + _itemData.Name + ".", 0.04f, null);
    }
}
