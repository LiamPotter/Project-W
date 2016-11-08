using UnityEngine;
using System.Collections;
using UnityEditor;

public class WaifuCustomizationEditor : EditorWindow {

    public WaifuCustomizationVars waifuVars;

    private bool showHairList;
    private bool showEyeList;
    private bool showFaceList;
    private bool showTopList;
    private bool showMouthList;

    [MenuItem("Waifu Customization/Variables")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        GetWindow(typeof(WaifuCustomizationEditor));
        GetWindow(typeof(WaifuCustomizationEditor)).minSize = new Vector2(200, 300);

    }

    public void OnEnable()
    {
        //if there is no ASOInstance asset, create one, else simply load the one found
        if ((WaifuCustomizationVars)AssetDatabase.LoadAssetAtPath
            ("Assets/Resources/WaifuVarsInstance.asset",
            typeof(WaifuCustomizationVars)) == null)
        {
            waifuVars = CreateInstance<WaifuCustomizationVars>();
            AssetDatabase.CreateAsset(waifuVars, "Assets/Resources/WaifuVarsInstance.asset");
        }
        titleContent = new GUIContent("Waifu Vars");
        LoadData();
    }
    void LoadData()
    {
        waifuVars = (WaifuCustomizationVars)AssetDatabase.LoadAssetAtPath("Assets/Resources/WaifuVarsInstance.asset", typeof(WaifuCustomizationVars));
    }
    void SaveData()
    {
        EditorApplication.SaveAssets();
        AssetDatabase.SaveAssets();
    }
    void OnGUI()
    {
        if (waifuVars != null)
        {
            waifuVars.resourceFolderPostion = EditorGUILayout.TextField("Graphics Folder Positon:",waifuVars.resourceFolderPostion);
            if (GUILayout.Button("Grab Graphics"))
            {
                waifuVars.GrabGraphics();
                waifuVars.ApplyVariables();
                SaveData();
            }
            if(FindObjectOfType<CustomizationMenuManager>().placementPostions.Count<=0f)
            {
                if (GUILayout.Button("Find Placement Positions"))
                    FindObjectOfType<CustomizationMenuManager>().GrabPlacementPositions();
            }
        }
        else
        {
            GUILayout.Label("WaifuVars is missing!");
            if (GUILayout.Button("Try and load Waifu Vars Instance"))
            {
                LoadData();
                SaveData();
            }
        }
    }
    public void ListIterator(string propertyPath, ref bool visible, SerializedObject serializedObject, GUIStyle style, string title,string type)
    {
        SerializedProperty listProperty = serializedObject.FindProperty(propertyPath);
        visible = EditorGUILayout.Foldout(visible, title, style);
        if (visible)
        {
            EditorGUI.indentLevel++;
            //EditorGUILayout.Space();
            for (int i = 0; i < listProperty.arraySize; i++)
            {
                EditorGUILayout.Space();
                SerializedProperty elementProperty = listProperty.GetArrayElementAtIndex(i);
                //Rect drawZone = GUILayoutUtility.GetRect(0f, 16f);
                GUIContent contentP = new GUIContent();
                GUIStyle gStyle = new GUIStyle();
                gStyle.fontSize = style.fontSize;
                contentP.text = type + i + ": ";
                //contentP.tooltip = "Place any scene in this field. NOTE: Will not accept anything but SceneAssets.";
                gStyle.normal.textColor = style.normal.textColor;
                float minW;
                float maxW;
                gStyle.CalcMinMaxWidth(contentP, out minW, out maxW);
                gStyle.margin.right = 0;
                gStyle.margin.left = 0;
                gStyle.margin.top = 0;
                gStyle.margin.bottom = 2;
                //EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(contentP, gStyle);
                //EditorGUILayout.Space();
                EditorGUI.indentLevel++;

                bool showChildren = EditorGUILayout.PropertyField(elementProperty, GUIContent.none);
                EditorGUI.indentLevel--;
                //EditorGUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel--;
        }
    }
}
