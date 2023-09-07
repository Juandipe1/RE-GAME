using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogePrinter : MonoBehaviour
{
    public static DialogePrinter Instance { get; private set; }
    PlayerController player;
    CharacterController characterController;

    [SerializeField] private TMP_Text _dialogueTextMesh;
    private bool _isBusy;

    public void PrintDialogueLine(string LineToPrint, float charSpeed, Action finishedCallback)
    {
        if (_isBusy)
        {
            throw new Exception("Dialogue printer already printing, but recived new print request." + "'" + LineToPrint + "'");
        }
        StartCoroutine(CO_PrintDialogueLine(LineToPrint, charSpeed, finishedCallback));
        _isBusy = true;
    }

    private IEnumerator CO_PrintDialogueLine(string LineToPrint, float charSpeed, Action finishedCallback)
    {
        _dialogueTextMesh.SetText(string.Empty);
        characterController.enabled = false;

        for (int i = 0; i < LineToPrint.Length; i++)
        {
            var character = LineToPrint[i];
            _dialogueTextMesh.SetText(_dialogueTextMesh.text + character);

            yield return new WaitForSeconds(charSpeed);
        }
        yield return new WaitUntil(() => player.CharacterControl.Pick.WasPressedThisFrame());

        _dialogueTextMesh.SetText(string.Empty);

        finishedCallback?.Invoke();
        characterController.enabled = true;
        _isBusy = false;
        yield return null;
    }

    private void Awake()
    {
        Instance = this;
        player = new PlayerController();
        characterController = FindAnyObjectByType<CharacterController>();
    }

    private void OnEnable()
    {
        player.CharacterControl.Enable();
    }

    private void OnDisable()
    {
        player.CharacterControl.Disable();
    }
}
