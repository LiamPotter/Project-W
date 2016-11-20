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

    public CZ_Character charInstance;

    private Rect extraInfoRect;

    private bool showingExtraInfo;

    public bool makingVariable;

    public bool editingVariable;

    public CZ_Variable tempVariable;

    private GUIStyle titleStyle = new GUIStyle();

    private GUIStyle subTitleStyle = new GUIStyle();

    private GUIStyle errorStyle = new GUIStyle();

    private GUIStyle style0 = new GUIStyle();
    string[] secOptions = new string[5];
    int selected = 0;
    private string sectionName;
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

        editingVariable = false;

        showingExtraInfo = false;

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
        serializedVarObj = new SerializedObject(aquireInstance);
        aquireInstance.foldersToSearch.Clear();
        aquireInstance.folderToSearch = "Assets/Resources/Customization/Variables";
        
    }

    void OnGUI()
    {
        //Debug.Log(showingExtraInfo);
        GUILayout.Label("Variable Editor", titleStyle);
        extraInfoRect = new Rect((Screen.width / 3) + (Screen.width / 3), 52, Screen.width / 3, 20);
        if (!makingVariable&& !editingVariable)
        {
           
            //if (GUILayout.Button("Create New Variable"))
            //{
            //    tempVariable = CreateInstance<CZ_Variable>();
            //    makingVariable = !makingVariable;
            //}
            //aquireInstance.FillVariableList(aquireInstance.variables, "t:CZ_Variable", "");
            //serializedVarObj = new SerializedObject(aquireInstance); 
            //ListIterator("variables", ref listVisible, serializedVarObj, subTitleStyle, style0, "Variables", "");
            //if (!showingExtraInfo)
            //{
            //    if (GUI.Button(extraInfoRect, "Show Extra Info"))
            //    {
            //        showingExtraInfo = true;
            //    }
            //}
            //else
            //{
            //    if(GUI.Button(extraInfoRect,"Hide Extra Info"))
            //    {
            //        showingExtraInfo = false;
            //    }
            //}
        }
        //else
        //{
        if(tempVariable==null)
        {
            tempVariable = CreateInstance<CZ_Variable>();
        }
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
            
            tempVariable.objectForManipulation = (GameObject)EditorGUILayout.ObjectField("Object to Manipulate: ", tempVariable.objectForManipulation, typeof(GameObject), true);
            tempVariable.manipulationType = (CZ_Variable.ManipType)EditorGUILayout.EnumPopup("Manipulation Type: ", tempVariable.manipulationType);
            switch (tempVariable.manipulationType)
            {
                case CZ_Variable.ManipType.Scale:
                    tempVariable.minManipulation = EditorGUILayout.FloatField("Minimum Value", tempVariable.minManipulation);
                    tempVariable.maxManipulation = EditorGUILayout.FloatField("Maximum Value", tempVariable.maxManipulation);
                    break;
                case CZ_Variable.ManipType.ShapeKey:
                    tempVariable.minManipulation = EditorGUILayout.FloatField("Minimum Value", tempVariable.minManipulation);
                    tempVariable.maxManipulation = EditorGUILayout.FloatField("Maximum Value", tempVariable.maxManipulation);
                    break;
                case CZ_Variable.ManipType.Color:
                    tempVariable.colorValue = EditorGUILayout.ColorField("Color Value",tempVariable.colorValue);
                    break;
                default:
                    break;
            }
            
        }
        //tempVariable.wantedSection = (CZ_Section)EditorGUILayout.ObjectField("Wanted Section", tempVariable.wantedSection, typeof(CZ_Section), false);
        secOptions = aquireInstance.Aquire_SectionsFromCharacter(charInstance);
        if (secOptions.Length > 0)
        {
            selected = EditorGUILayout.Popup("Section", selected, secOptions);
            sectionName = secOptions[selected];
            if (tempVariable.wantedSection == null)
                SetSectionFromNull();
            if(tempVariable.wantedSection.name!=sectionName)
                SetSectionFromName();
        }
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
        if (editingVariable)
        {
            if(GUILayout.Button("Save!"))
            {
                AssetDatabase.SaveAssets();
                //makingVariable = false;
                editingVariable = false;
            }
        }
        else
        {
            if (GUILayout.Button("Create!"))
            {
                creatorInstance.Create_Variable(tempVariable,tempVariable.wantedSection);
                makingVariable = !makingVariable;
                //Close();
            }
        }
        if(GUILayout.Button("Cancel"))
        {
            tempVariable = null;
            if(editingVariable)
                editingVariable = false;
            if(makingVariable)
                makingVariable = false;
            Close();
            //editingVariable = false;
        }
        //}
  
    }
    private void SetSectionFromNull()
    {
        tempVariable.wantedSection = charInstance.sections[0];
    }
    private void SetSectionFromName()
    {
        tempVariable.wantedSection = charInstance.sections[selected];
    }

    private void ListIterator(string propertyPath, ref bool visible, SerializedObject serializedObject,GUIStyle titleStyle, GUIStyle style, string title, string type)
    {
        SerializedProperty listProperty = serializedObject.FindProperty(propertyPath);
        //visible = EditorGUILayout.Foldout(visible, title, titleStyle);
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
                GUIStyle eStyle = new GUIStyle();
                gStyle.fontSize = style.fontSize;
                gStyle.normal.textColor = style.normal.textColor;
             
                GUIContent tContent = new GUIContent();
             

                tContent.text = elementProperty.objectReferenceValue.name;
                CZ_Variable czVarToUse = (CZ_Variable)listProperty.GetArrayElementAtIndex(i).objectReferenceValue;
                GUIContent eContent0 = new GUIContent();
                GUIContent eContent1 = new GUIContent();
                eContent0.text ="Type: "+czVarToUse.variableType.ToString();
                if(czVarToUse.variableType== CZ_Variable.VarType.Creation)
                {
                    eContent1.text = "Model: " + czVarToUse.objectForCreation;
                }
                else
                {

                }
                gStyle.padding.left = 10;
                eStyle.padding.left = 25;
                eStyle.fontSize = 11;
                int widthInt = 20;
                int offsetInt = 40;
                if (showingExtraInfo)
                {
                    widthInt = 40;
                    offsetInt = 45;
                }
                else
                {
                    widthInt = 20;
                    offsetInt = 40;
                }
                Rect leftRect = new Rect(5, 80+(offsetInt * i), Screen.width-10, widthInt);
                GUI.Box(leftRect, GUIContent.none);
                //EditorGUILayout.LabelField(tContent, gStyle);
                GUI.Label(leftRect,tContent,gStyle);    
                if(showingExtraInfo)
                {
                    Rect eRect = new Rect(leftRect);
                    eRect.y += 20;
                    GUI.Label(eRect, eContent0,eStyle);
                }

                Rect leftButtonRect = leftRect;
                leftButtonRect.x = (Screen.width / 3)+(Screen.width/3);
                leftButtonRect.width = Screen.width / 3;
                GUIStyle bStyle = new GUIStyle("Button");
                if(GUI.Button(leftButtonRect, "Edit",bStyle))
                {
                    SetEdit((CZ_Variable)listProperty.GetArrayElementAtIndex(i).objectReferenceValue);
                }

            }
            EditorGUI.indentLevel--;
        }
    }

    private void SetEdit(CZ_Variable variable)
    {
        editingVariable = true;
        tempVariable = variable;
    }
    public void StartMaking()
    {
        makingVariable = true;
    }
}
