using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CZ_Section : ScriptableObject{

    public CZ_Character character;

    public enum SecType
    {
        Creation,
        Modification
    }
    public string sectionName;

    public SecType sectionType;

    public GameObject objectToUse;

    public List<CZ_Variable> variablesUsingThisSection = new List<CZ_Variable>();
}
