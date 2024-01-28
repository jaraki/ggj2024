using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialog : MonoBehaviour {
    public TMP_Text TitleText;
    public TMP_Text LineText;
    public float LettersPerSecond;
    public float DeletionDelay;

    public void SetTitle(string title) {
        TitleText.text = title;
    }

    public IEnumerator SetLine(string line) {
        for(int i = 0; i < line.Length; i++) {
            LineText.text += line[i];
            yield return new WaitForSeconds(1f / LettersPerSecond);
        }
        yield return new WaitForSeconds(DeletionDelay);
        Destroy(gameObject);
    }
}
