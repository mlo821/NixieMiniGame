using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TubeArray : MonoBehaviour
{
    public List<TubeControl> tubeControls;
    public List<string> mychars;
    public InputField mainInputField;
    public int currentCharacterCount;


    void Start()
    {
        foreach (Transform child in gameObject.transform)
        {
            if (child.tag == "tube")
            {
                tubeControls.Add(child.gameObject.GetComponent<TubeControl>());
            }
        }

        mainInputField.onValueChanged.AddListener(delegate { ValueChangeCheck(mainInputField.text); });
    }


    public void StringtoTube(string inputString)
    {
        mychars.Clear();

        for (int i = 0; i < inputString.Length; i++)
        {
            mychars.Add(System.Convert.ToString(inputString[i]));
        }
;
        //add blanks characters to array to bring total up to the maximum character count
        if (mychars.Count <= tubeControls.Count)
        {
            int charactersNeeded = (tubeControls.Count - 1) - mychars.Count;
            for (int i = 0 - 1; i < charactersNeeded; i++)
            {
                mychars.Add(" ");
            }
        }

        for (int i = 0; i < tubeControls.Count; i++)        
        {
            tubeControls[i].LightNumber(mychars[i]);
        }
    }


    private void ValueChangeCheck(string newValue)
    {
        currentCharacterCount++;
        StringtoTube(newValue);

    }

    public void OnClickInputButton()
    {

        mainInputField.ActivateInputField();
        StringtoTube(mainInputField.text);
    }
}
