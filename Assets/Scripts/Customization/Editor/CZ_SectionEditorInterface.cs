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

    [MenuItem("Customization/Section Interface")]
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
        titleContent = new GUIContent("Characters");
        #region Style0
        style0.fontSize = 13;
        style0.padding.left = 5;
        #endregion
        creatorInstance = CreateInstance<CZ_Creator>();
        aquireInstance = CreateInstance<CZ_Aquire>();
        //Find_Character();
    }
    public void OnGUI()
    {

    }
}
