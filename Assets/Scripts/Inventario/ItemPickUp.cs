using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    [SerializeField] private ItemData _itemData;
    Player player;

    private void Start()
    {
        player = FindAnyObjectByType<Player>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(!other.CompareTag("Player")) return;

        if (player.isItemPickUp)
        {
            EventBus.Instance.PickUpItem(_itemData);
            Destroy(gameObject);
        }
    }
}
