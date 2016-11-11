using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CZ_Aquire : ScriptableObject {

    public List<string> foldersToSearch = new List<string>();

    public List<string> tagsToFind = new List<string>();

    private List<CZ_Variable> privateVarList = new List<CZ_Variable>();
    public CZ_Character characterInstance;

    public CZ_Manager managerInstance;

    public void Aquire_Variables()
    {
        privateVarList.Clear();
        for (int i = 0; i < foldersToSearch.Count; i++)
        {
            FillVariableList(privateVarList,"Variable",foldersToSearch[i]);
            Debug.Log(foldersToSearch[i]);
        }
        if(characterInstance!=null&&privateVarList!=null)
            characterInstance.variables = privateVarList;
    }
    private void FillVariableList(List<CZ_Variable> pList, string assetTag,string folderPosition)
    {
        pList.Clear();

        string[] lookFor = new string[] { folderPosition+"/"};
        string[] guid = UnityEditor.AssetDatabase.FindAssets(assetTag, lookFor);
        foreach (string pGuid in guid)
        {
            //Debug.Log(AssetDatabase.GUIDToAssetPath(pGuid));
            pList.Add((CZ_Variable)UnityEditor.AssetDatabase.LoadAssetAtPath(UnityEditor.AssetDatabase.GUIDToAssetPath(pGuid), typeof(CZ_Variable)));
        }

    }
}
