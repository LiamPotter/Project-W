using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CZ_Aquire : ScriptableObject {

    public List<string> foldersToSearch = new List<string>();

    public string folderToSearch;

    public List<string> tagsToFind = new List<string>();

    public List<CZ_Variable> variables = new List<CZ_Variable>();
    public CZ_Character characterInstance;

    public CZ_Manager managerInstance;

    public void Aquire_Variables()
    {
        variables.Clear();
        for (int i = 0; i < foldersToSearch.Count; i++)
        {
            FillVariableList(variables,"t:CZ_Variable",foldersToSearch[i]);
            Debug.Log(foldersToSearch[i]);
        }
        if (folderToSearch != null)
            FillVariableList(variables, "t:CZ_Variable", folderToSearch);
        if (characterInstance != null && variables != null)
            characterInstance.variables = variables;
    }
    public void FillVariableList(List<CZ_Variable> pList, string assetTag,string folderPosition)
    {
        pList.Clear();

        string[] lookFor = new string[] { folderPosition+"/"};
        string[] guid = UnityEditor.AssetDatabase.FindAssets(assetTag);
        foreach (string pGuid in guid)
        {
            //Debug.Log(UnityEditor.AssetDatabase.GUIDToAssetPath(pGuid));
            pList.Add((CZ_Variable)UnityEditor.AssetDatabase.LoadAssetAtPath(UnityEditor.AssetDatabase.GUIDToAssetPath(pGuid), typeof(CZ_Variable)));
        }

    }
    public void Find_Character(CZ_Creator czCreatorInstance, CZ_Character returnCharacter)
    {
        string path = "Assets/Resources/Customization/Characters/";
        string[] filters = new string[2];
        filters[0] = "Asset Files";
        filters[1] = "asset";
        string fileLocation = UnityEditor.EditorUtility.OpenFilePanelWithFilters("Load Character", path, filters);
        string relativepath = "Assets" + fileLocation.Substring(Application.dataPath.Length);
        //Debug.Log(relativepath);
        characterInstance = (CZ_Character)UnityEditor.AssetDatabase.LoadAssetAtPath(relativepath, typeof(CZ_Character));
        czCreatorInstance.characterInstance = characterInstance;
        returnCharacter = characterInstance;
    }
}
