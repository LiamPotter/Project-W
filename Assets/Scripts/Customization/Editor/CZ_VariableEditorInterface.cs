using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class CZ_VariableEditorInterface : EditorWindow
{

    private static CZ_VariableEditorInterface VariableEditorInterface;

    public CZ_Creator creatorInstance;

    public CZ_Aquire aquireInstance;

    public List<CZ_Variable> variables = new List<CZ_Variable>();

    SerializedObject serializedVarObj;
    SerializedProperty serProperty;
    bool listVisible = true;

    public bool makingVariable;

    private CZ_Variable tempVariable;

    private GUIStyle titleStyle = new GUIStyle();

    private GUIStyle subTitleStyle = new GUIStyle();

    private GUIStyle errorStyle = new GUIStyle();

    private GUIStyle style0 = new GUIStyle();

    [MenuItem("Customization/Variable Interface")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        GetWindow(typeof(CZ_VariableEditorInterface));
        GetWindow(typeof(CZ_VariableEditorInterface)).minSize = new Vector2(200, 300);
    }

    public void OnEnable()
    {
        listVisible = true;
        #region TitleStyle
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.fontSize = 15;
        titleStyle.padding.top = 5;
        titleStyle.padding.bottom = 5;
        titleStyle.alignment = TextAnchor.UpperCenter;
        titleContent = new GUIContent("Variables");
        #endregion

        #region Style0
        style0.fontSize = 13;
        style0.padding.left = 5;
        #endregion

        #region subTitleStyle
        subTitleStyle.fontSize = 13;
        subTitleStyle.padding.left = 5;
        subTitleStyle.fontStyle = FontStyle.Bold;
        #endregion

        #region ErrorStyle
        //errorStyle = style0;
        errorStyle.normal.textColor = Color.red;
        errorStyle.active.textColor = Color.red;
        errorStyle.hover.textColor = Color.red;
        errorStyle.focused.textColor = Color.red;
        #endregion
        creatorInstance = CreateInstance<CZ_Creator>();
        aquireInstance = CreateInstance<CZ_Aquire>();
        VariableEditorInterface = this;
        if(serializedVarObj==null)
        {
            serializedVarObj = new SerializedObject(aquireInstance);
        }
        aquireInstance.foldersToSearch.Clear();
        aquireInstance.folderToSearch = "Assets/Resources/Customization/Variables";
    }

    void OnGUI()
    {

        GUILayout.Label("Variable Editor", titleStyle);

        if (!makingVariable)
        {
            if (GUILayout.Button("Create New Variable"))
            {
                tempVariable = CreateInstance<CZ_Variable>();
                makingVariable = !makingVariable;
            }
            aquireInstance.FillVariableList(aquireInstance.variables, "t:CZ_Variable", "");
            serializedVarObj = new SerializedObject(aquireInstance);
            GUILayout.Label("Variable Amount: " + aquireInstance.variables.Count);
            ListIterator("variables", ref listVisible, serializedVarObj, subTitleStyle, style0, "Variables", "");
        }
        else
        {
            tempVariable.variableName = EditorGUILayout.TextField("Variable Name: ", tempVariable.variableName);
            tempVariable.variableType = (CZ_Variable.VarType)EditorGUILayout.EnumPopup("Variable Type: ",tempVariable.variableType);
            if(tempVariable.variableType==CZ_Variable.VarType.Creation)
            {
                tempVariable.modelSpecific = EditorGUILayout.Toggle("Model Specific?: ", tempVariable.modelSpecific);
                tempVariable.objectForCreation = (GameObject)EditorGUILayout.ObjectField("Object to Create: ", tempVariable.objectForCreation, typeof(GameObject),false);
            }
            else
            {
                tempVariable.modelSpecific = true;
                tempVariable.objectForManipulation = (GameObject)EditorGUILayout.ObjectField("Object to Manipulate: ", tempVariable.objectForManipulation, typeof(GameObject), false);
                tempVariable.manipulationType = (CZ_Variable.ManipType)EditorGUILayout.EnumPopup("Manipulation Type: ", tempVariable.manipulationType);
                tempVariable.minManipulation = EditorGUILayout.FloatField("Minimum Value", tempVariable.minManipulation);
                tempVariable.maxManipulation = EditorGUILayout.FloatField("Maximum Value", tempVariable.maxManipulation);
            }
            tempVariable.wantedSection = (CZ_Section)EditorGUILayout.ObjectField("Wanted Section", tempVariable.wantedSection, typeof(CZ_Section), false);
            if (tempVariable.variableType==CZ_Variable.VarType.Modification)
            {
                if (tempVariable.minManipulation > tempVariable.maxManipulation)
                {
                    EditorGUILayout.LabelField("The Minimum value cannot be higher than the Maximum value!!!", errorStyle);
                    return;
                }
                if(tempVariable.maxManipulation<tempVariable.minManipulation)
                {
                    EditorGUILayout.LabelField("The Maximum value cannot be lower than the Minimum value!!!", errorStyle);
                    return;
                }
            }
            if(GUILayout.Button("Create!"))
            {
                creatorInstance.Create_Variable(tempVariable);
                makingVariable = !makingVariable;
            }
            if(GUILayout.Button("Cancel"))
            {
                tempVariable = null;
                makingVariable = !makingVariable;
            }
        }
  
    }

    private void CalculateStuff()
    {

    }

    private void ListIterator(string propertyPath, ref bool visible, SerializedObject serializedObject,GUIStyle titleStyle, GUIStyle style, string title, string type)
    {
        SerializedProperty listProperty = serializedObject.FindProperty(propertyPath);
        visible = EditorGUILayout.Foldout(visible, title, titleStyle);
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
