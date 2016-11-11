using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CZ_Variable : ScriptableObject {
    public enum VarType
    {
        Creation,
        Modification
    }
    public string variableName;

    public VarType variableType;

    public GameObject objectForCreation;

    public GameObject objectForManipulation;

    public float manipulationVariable;

    public CZ_Section wantedSection;
}
