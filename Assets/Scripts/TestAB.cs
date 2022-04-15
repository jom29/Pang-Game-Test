using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestAB : MonoBehaviour
{
    public Text ui_gui;

    public void PrintButton(string value)
    {
        ui_gui.text = value;
    }

    
}
