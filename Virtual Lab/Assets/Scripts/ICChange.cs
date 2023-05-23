using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ICChange : MonoBehaviour
{
    [SerializeField]
    private IC ic;

    private Button changeButton;
    [SerializeField]
    private GameObject ICSelection;
    private void Awake()
    {
        changeButton = GetComponent<Button>();
    }
    private void Start()
    {
        changeButton.onClick.AddListener(changeIc);
    }

    private void changeIc()
    {
        ICBase IcBase = SimulatorManager.Instance.IcBase;
        IcBase.ICSprite.sprite = ic.IcSprite;
        ICSelection.SetActive(false);
    }
}
