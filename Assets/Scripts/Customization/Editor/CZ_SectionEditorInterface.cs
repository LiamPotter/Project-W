using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class CZ_SectionEditorInterface : EditorWindow {

    private GUIStyle titleStyle = new GUIStyle();

    private GUIStyle style0 = new GUIStyle();

    public CZ_Creator creatorInstance;

    public CZ_Aquire aquireInstance;

    public CZ_Character characterInstance;

    public CZ_Section tempSection;

    public bool editingSection;

    public string characterPath;

    //[MenuItem("Customization/Section Interface")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        GetWindow(typeof(CZ_SectionEditorInterface));
        GetWindow(typeof(CZ_SectionEditorInterface)).minSize = new Vector2(200, 300);
    }
    public void OnEnable()
    {

        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.fontSize = 15;
        titleStyle.padding.top = 5;
        titleStyle.padding.bottom = 5;
        titleStyle.alignment = TextAnchor.UpperCenter;
        titleContent = new GUIContent("Sections");
        #region Style0
        style0.fontSize = 13;
        style0.padding.left = 5;
        #endregion
        creatorInstance = CreateInstance<CZ_Creator>();
        aquireInstance = CreateInstance<CZ_Aquire>();
        //Find_Character();
        if(tempSection==null)
            tempSection = CreateInstance<CZ_Section>(); 
    }
    public void OnGUI()
    {
        if(!editingSection)
            tempSection.sectionName = EditorGUILayout.TextField("Section Name", tempSection.sectionName);
        tempSection.sectionType = (CZ_Section.SecType)EditorGUILayout.EnumPopup("Section Type", tempSection.sectionType);
        tempSection.objectToUse = (GameObject)EditorGUILayout.ObjectField("Object to use", tempSection.objectToUse, typeof(GameObject), true);
        if (editingSection)
        {
            if (GUILayout.Button("Save Changes"))
            {
                AssetDatabase.SaveAssets();
                Close();
            }
        }
        else
        {
            if (GUILayout.Button("Save Section"))
            {
                tempSection.name = tempSection.sectionName;
                creatorInstance.Create_Section(tempSection,characterInstance);
                //Debug.Log(AssetDatabase.GetAssetPath(tempSection));
                Close();
            }
        }   
        if(GUILayout.Button("Cancel"))
        {
            Close();
        }
    }
}
