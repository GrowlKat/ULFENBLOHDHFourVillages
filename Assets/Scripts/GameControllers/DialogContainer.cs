using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogContainer : MonoBehaviour
{
    public GameObject charNameObject;
    public GameObject textObject;

    public List<string> dialogs = new();

    private int dialogID;
    private Text charName;
    private Text text;

    private void Start()
    {
        charName = charNameObject.GetComponent<Text>();
        text = textObject.GetComponent<Text>();
    }

    private void OnGUI()
    {
        Event current = Event.current;

        if (current != null && current.button.Equals(0) && current.isMouse)
        {
            if (dialogID < dialogs.Count)
            {
                NextText(null, "");
                dialogID++;
            }
            else EndDialog();
        }
    }

    private IEnumerator ShowText(string name, string dialog)
    {
        int i = 0;

        charName.text = name is null ? name : "";

        do
        {
            text.text += dialog[i];
            yield return new WaitForSeconds(0.2f);
            i++;
        }
        while (i < dialog.Length && !Input.GetKeyDown(KeyCode.Mouse0));
    }

    private void NextText(string name, string dialog)
    {
        text.text = "";
        StartCoroutine(ShowText(name, dialog));
    }

    private void EndDialog()
    {
        text.text = "";
        Destroy(gameObject);
    }
}