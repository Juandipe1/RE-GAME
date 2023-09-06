using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ItemSlot : MonoBehaviour, ISelectHandler
{
    public ItemData _itemData;

    public ItemData itemData;
    private InventoryViewController _viewController;
    private Image _spawnedItemSprite;

    public void OnSelect(BaseEventData eventData)
    {
        _viewController.OnSlotSelected(this);
    }

    public bool IsEmpty()
    {
        return itemData == null;
    }

    private void OnEnable()
    {
        _viewController = FindObjectOfType<InventoryViewController>();

        if (itemData == null) return;

        _spawnedItemSprite = Instantiate<Image>(itemData.Sprite, transform.position, Quaternion.identity, transform);
    }

    private void OnDisable()
    {
        if (_spawnedItemSprite != null)
        {
            Destroy(_spawnedItemSprite);
        }
    }
}
