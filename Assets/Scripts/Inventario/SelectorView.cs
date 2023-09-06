using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectorView : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private GameObject _firstSelection;

    private RectTransform _rectTransform;
    private GameObject _selected;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(_firstSelection);
    }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        var selectedGameObject = EventSystem.current.currentSelectedGameObject;
        _selected = (selectedGameObject == null) ? _selected : selectedGameObject;
        var selected = EventSystem.current.currentSelectedGameObject;

        if (selected == null) return;

        //transform.position = selected.transform.position;
        transform.position = Vector3.Lerp(transform.position, selected.transform.position, _speed * Time.deltaTime);

        var otherRect = selected.GetComponent<RectTransform>();

        var horizontalLerp = Mathf.Lerp(_rectTransform.rect.size.x, otherRect.rect.size.x, _speed * Time.deltaTime);
        var verticalLerp = Mathf.Lerp(_rectTransform.rect.size.y, otherRect.rect.size.y, _speed * Time.deltaTime);

        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, otherRect.rect.size.x);
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, otherRect.rect.size.y);
    }
}
