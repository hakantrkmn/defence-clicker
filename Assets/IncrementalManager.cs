using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncrementalManager : MonoBehaviour
{
    public Button soldierButton;
    public Button mergeButton;
    public Button incomeButton;

    public bool canMerge;

    private void OnEnable()
    {
        EventManager.PlayerCanMerge += PlayerCanMerge;
    }

    private void OnDisable()
    {
        EventManager.PlayerCanMerge -= PlayerCanMerge;

    }

    private void PlayerCanMerge(bool merge)
    {
        canMerge = merge;
        mergeButton.interactable = canMerge;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
