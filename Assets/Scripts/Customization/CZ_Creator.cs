using UnityEngine;
using System.Collections;

public class CZ_Creator : ScriptableObject {

    public CZ_Character characterInstance;

    public string folderPosition;

    public string variableName;

    public string sectionName;

    public bool creating;


    public void Create_Variable(CZ_Variable variable,CZ_Section section)
    {
        //string path = "Assets/Resources/Customization/Variables/" + variable.variableName + ".asset";
        //UnityEditor.AssetDatabase.CreateAsset(variable, path);
        
        section.variablesUsingThisSection.Add(variable);
        section.character.variables.Add(variable);
        //variable.name = variable.variableName;

        //string path = UnityEditor.AssetDatabase.GetAssetPath(section);
        //UnityEditor.AssetDatabase.AddObjectToAsset(variable, path);
        //variable.name = variable.variableName;
        //UnityEditor.AssetDatabase.ImportAsset(UnityEditor.AssetDatabase.GetAssetPath(variable));
        //UnityEditor.AssetDatabase.Refresh();
    }

    public void Create_Section(CZ_Section section,CZ_Character character)
    {
        string path = UnityEditor.AssetDatabase.GetAssetPath(character);
        CZ_Section tSec = CreateInstance<CZ_Section>();
        tSec = section;
        tSec.character = character;
    
        tSec.name = "z" + tSec.sectionName;
        UnityEditor.AssetDatabase.AddObjectToAsset(tSec, path);
        UnityEditor.AssetDatabase.ImportAsset(UnityEditor.AssetDatabase.GetAssetPath(tSec));
        tSec.hideFlags = HideFlags.HideInInspector;
        character.sections.Add(tSec);
        //UnityEditor.AssetDatabase.Refresh();
        //Debug.Log(tempSection.name);
    }
	
}
