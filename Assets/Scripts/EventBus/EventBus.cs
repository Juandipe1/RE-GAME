using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBus : MonoBehaviour
{
    public static EventBus Instance { get; private set; }

    public event Action<Action> onOpenInventory;
    public event Action onCloseInventory;
    public event Action<ItemData> onPickUpItem;
    public event Action onGameplayPaused;
    public event Action onGameplayResumed;
    public event Action<ItemData> onItemUsed;
    public event Action<ItemData> onItemPrompt;

    public void OpenInventory(Action finishedCallback)
    {
        onOpenInventory?.Invoke(finishedCallback);
    }
    
    public void CloseInventory()
    {
        onCloseInventory?.Invoke();
    }

    public void PickUpItem(ItemData itemData)
    {
        onPickUpItem?.Invoke(itemData);
    }

    public void PauseGameplay()
    {
        onGameplayPaused?.Invoke();
    }

    public void ResumeGameplay()
    {
        onGameplayResumed?.Invoke();
    }

    private void Awake()
    {
        Instance = this;
    }

    public void UseItem(ItemData item)
    {
        onItemUsed?.Invoke(item);
    }

    public void PrompItemPickUp(ItemData item)
    {
        onItemPrompt?.Invoke(item);
    }
}
