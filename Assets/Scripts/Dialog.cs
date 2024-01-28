using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialog : MonoBehaviour {
    public TMP_Text TitleText;
    public TMP_Text LineText;
    public float DeletionDelay;

    public void SetTitle(string title) {
        TitleText.text = title;
    }

    public IEnumerator SetLine(string line, float duration) {
        for(int i = 0; i < line.Length; i++) {
            LineText.text += line[i];
            yield return new WaitForSeconds(duration / line.Length);
        }
        yield return new WaitForSeconds(DeletionDelay);
        Destroy(gameObject);
    }
}
