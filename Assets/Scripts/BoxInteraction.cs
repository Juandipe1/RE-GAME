using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxInteraction : MonoBehaviour
{
    [SerializeField] private ItemData _requiredItem;
    private Renderer _renderer;
    private SceneTransition sceneTransition;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        sceneTransition = FindAnyObjectByType<SceneTransition>();
    }

    private void OnEnable()
    {
        EventBus.Instance.onItemUsed += onItemUsed;
    }

    private void OnDisable()
    {
        EventBus.Instance.onItemUsed -= onItemUsed;
    }

    private void onItemUsed(ItemData item)
    {
        if (Vector3.Distance(Player.Instance.transform.position, transform.position) < 3)
        {
            if (item == _requiredItem)
            {
                DialogePrinter.Instance.PrintDialogueLine("Se ha abierto la puerta.", 0.06f, () => sceneTransition.onSceneChange());
            }
        }
    }
}
