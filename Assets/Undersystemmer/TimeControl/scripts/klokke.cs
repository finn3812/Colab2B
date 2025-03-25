using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class klokke : MonoBehaviour
{
    public TextMeshProUGUI clockText; // UI-tekst til at vise tiden

    void Update()
    {
        clockText.text = DateTime.Now.ToString("HH:mm:ss"); // Viser timer, minutter og sekunder
    }
}
