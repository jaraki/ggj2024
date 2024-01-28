using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialog : MonoBehaviour {
    public TMP_Text TitleText;
    public TMP_Text LineText;

    public void SetTitle(string title) {
        TitleText.text = title;
    }

    public void SetLine(string line) {
        LineText.text = line;
    }
}
