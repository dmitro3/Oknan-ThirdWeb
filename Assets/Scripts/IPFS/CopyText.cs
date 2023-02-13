using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CopyText : MonoBehaviour
{
    public TMP_Text hashText;
    
    public void CopyAddress_ButtonCall()
    {
        GUIUtility.systemCopyBuffer = hashText.text;
    }
}
