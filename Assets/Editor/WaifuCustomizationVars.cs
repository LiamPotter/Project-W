using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class WaifuCustomizationVars : ScriptableObject {

    public List<GameObject> hairList = new List<GameObject>();
    public List<GameObject> eyeList=new List<GameObject>();
    public List<GameObject> faceList = new List<GameObject>();
    public List<GameObject> mouthList = new List<GameObject>();
    public List<GameObject> topList = new List<GameObject>();

    public string resourceFolderPostion;

    
    public void GrabGraphics()
    {
        FillList(hairList, "Hairs","Hair");
        FillList(eyeList, "Eyes", "Eye");
        FillList(faceList, "Faces", "Face");
        FillList(mouthList, "Mouths", "Mouth");
        FillList(topList, "Tops", "Top");
    }
    private void FillList(List<GameObject> pList, string folderName,string assetTag)
    {
        //Debug.Log("Hello I'm filling " + folderName +" from "+resourceFolderPostion+folderName);
        pList.Clear();
        //Debug.Log(AssetDatabase.GetDependencies(resourceFolderPostion+folderName).Length);
  
        string[] lookFor = new string[] { resourceFolderPostion + folderName};
        string[] guid = AssetDatabase.FindAssets(assetTag,lookFor);
        foreach (string pGuid in guid)
        {
            Debug.Log(AssetDatabase.GUIDToAssetPath(pGuid));
            pList.Add((GameObject)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(pGuid), typeof(GameObject)));
        }
        

    }
}
