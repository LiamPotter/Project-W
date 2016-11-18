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

    public enum ManipType
    {
        Scale,
        ShapeKey,
        Color
    }

    public ManipType manipulationType;

    public float manipulationVariable;

    public float maxManipulation;

    public float minManipulation;

    public Color colorValue;

    public int shapeKeyIndex;

    public CZ_Section wantedSection;

    public bool modelSpecific;
}
