using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public void ButtonClicked(int index)
    {
        EventManager.UpgradeButtonClicked(index);
    }
}
