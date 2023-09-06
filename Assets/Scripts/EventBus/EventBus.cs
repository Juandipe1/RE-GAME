using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBus : MonoBehaviour
{
    public static EventBus Instance { get; private set; }

    public event Action onOpenInventory;
    public event Action onCloseInventory;
    public event Action<ItemData> onPickUpItem;

    public void OpenInventory()
    {
        onOpenInventory?.Invoke();
    }
    
    public void CloseInventory()
    {
        onCloseInventory?.Invoke();
    }

    public void PickUpItem(ItemData itemData)
    {
        onPickUpItem?.Invoke(itemData);
    }

    private void Awake()
    {
        Instance = this;
    }

}
