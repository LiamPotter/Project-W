using UnityEngine;
using System.Collections;

public class CZ_Creator : ScriptableObject {

    public CZ_Character characterInstance;

    public string folderPosition;

    public string variableName;

    public string sectionName;

    public bool creating;


    public void Create_Variable(CZ_Variable variable)
    {
        string path = "Assets/Resources/Customization/Variables/" + variable.variableName + ".asset";
        UnityEditor.AssetDatabase.CreateAsset(variable, path);
    }

    public void Create_Section()
    {

    }
	
}
