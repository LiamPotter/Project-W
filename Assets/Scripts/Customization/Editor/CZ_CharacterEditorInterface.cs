﻿using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.UI;


public class CZ_CharacterEditorInterface : EditorWindow {

    public CZ_Creator creatorInstance;

    public CZ_Aquire aquireInstance;

    public CZ_Character characterInstance;

    private string characterName;
    private bool creatingCharacter;
    private bool modifyingCharacter;

    private GUIStyle titleStyle = new GUIStyle();
    string[] charOptions = new string[5];
    int selected = 0;
    private GUIStyle style0 = new GUIStyle();
    CZ_SectionEditorInterface sectionEditor;
    CZ_VariableEditorInterface variableEditor;
    [MenuItem("Customization/Character Interface")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        GetWindow(typeof(CZ_CharacterEditorInterface));
        GetWindow(typeof(CZ_CharacterEditorInterface)).minSize = new Vector2(200, 300);

    }

    public void OnEnable()
    {

        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.fontSize = 15;
        titleStyle.padding.top = 5;
        titleStyle.padding.bottom = 5;
        titleStyle.alignment = TextAnchor.UpperCenter;
        titleContent = new GUIContent("Characters");
        creatorInstance = CreateInstance<CZ_Creator>();
        aquireInstance = CreateInstance<CZ_Aquire>();

        #region Style0
        style0.fontSize = 13;
        style0.padding.left = 5;
        style0.padding.bottom = 5;
        #endregion

        //Find_Character();
    }
    public void OnDisable()
    {

        AssetDatabase.SaveAssets();
        //Find_Character();
    }
    void OnGUI()
    {
        GUILayout.Label("Character Editor", titleStyle);
        if (characterInstance == null)
        {
            charOptions = aquireInstance.FindCharacterReturnString();
            if (charOptions.Length > 0)
            {
                selected = EditorGUILayout.Popup("Switch Character", selected, charOptions);
                if (selected > charOptions.Length - 1)
                    selected = charOptions.Length - 1;
                characterName = charOptions[selected];
                if (characterInstance == null)
                    SetCharacterFromName();
                if (characterInstance.characterName != characterName)
                    SetCharacterFromName();
            }
            else creatingCharacter = true;
        }
        GUILayout.Label("Currently viewing " + characterInstance.characterName, titleStyle);
        if (!creatingCharacter&&!modifyingCharacter)
        {
            if (GUILayout.Button("Create New Character"))
            {
                creatingCharacter = true;
            }
            if (GUILayout.Button("Edit Character"))
            {
                modifyingCharacter = true;             
            }

            charOptions = aquireInstance.FindCharacterReturnString();
            selected = EditorGUILayout.Popup("Switch Character", selected, charOptions);
            if (selected > charOptions.Length - 1)
                selected = charOptions.Length-1;
            characterName = charOptions[selected];
            if (characterInstance.characterName != characterName)
                SetCharacterFromName();
            
            characterInstance.prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", characterInstance.prefab, typeof(GameObject),false);


            #region Variables

            #region Left Rect (Sections)
            Rect leftRect = new Rect(0, 150, Screen.width / 2 - 5, Screen.height);
            GUIStyle leftStyle = new GUIStyle();
            Texture2D leftTexture = new Texture2D((int)leftRect.width, (int)leftRect.height);
            Color[] leftColors = new Color[1];
            leftColors[0] = Color.grey;
            //leftTexture.SetPixels(leftColors);
            leftStyle.normal.background = leftTexture;
            GUI.Box(leftRect, "", leftStyle);
            GUILayout.BeginArea(leftRect);
            GUILayout.Label("Sections", titleStyle);
            //if(!CharacterSectionsMatch())
            //{
            //    AssignSections();
            //}
            if(characterInstance.sections.Count>0)
                for (int i = 0; i < characterInstance.sections.Count; i++)
                {
                    if(GUILayout.Button(characterInstance.sections[i].sectionName))
                    {
                        if (sectionEditor == null)
                            sectionEditor = CreateInstance<CZ_SectionEditorInterface>();
                        sectionEditor.tempSection = characterInstance.sections[i];
                        sectionEditor.editingSection = true;
                        sectionEditor.Show();
                    }
                }
            if(GUILayout.Button("Create New Section"))
            {
                if(sectionEditor==null)
                    sectionEditor = CreateInstance<CZ_SectionEditorInterface>();
                sectionEditor.characterInstance = characterInstance;
                sectionEditor.Show();
            }
            GUILayout.EndArea();
            #endregion

            #region Right Rect (Variables)
            Rect rightRect = new Rect(Screen.width / 2 + 5, 150, (Screen.width / 2) - 10, Screen.height);
            GUIStyle rightStyle = new GUIStyle();
            Texture2D rightTexture = new Texture2D((int)rightRect.width, (int)rightRect.height);
            Color[] rightColors = new Color[1];
            rightColors[0] = Color.grey;
            //rightTexture.SetPixels(rightColors);
            rightStyle.normal.background = rightTexture;
            GUI.Box(rightRect, "", rightStyle);
            GUILayout.BeginArea(rightRect);
            GUILayout.Label("Variables", titleStyle);
            if(characterInstance.variables.Count>0)
                for (int i = 0; i < characterInstance.variables.Count; i++)
                {
                    if(GUILayout.Button(characterInstance.variables[i].variableName))
                    {
                        if (variableEditor == null)
                            variableEditor = CreateInstance<CZ_VariableEditorInterface>();
                        variableEditor.charInstance = characterInstance;
                        variableEditor.editingVariable = true;
                        variableEditor.tempVariable = characterInstance.variables[i];
                        variableEditor.Show();
                    }
                }
            if (GUILayout.Button("Create New Variable"))
            {
                if (variableEditor == null)
                    variableEditor = CreateInstance<CZ_VariableEditorInterface>();
                variableEditor.charInstance = characterInstance;
                variableEditor.editingVariable = false;
                variableEditor.Show();
            }
            GUILayout.EndArea();
            #endregion

            if (creatorInstance.creating)
            {

            }
            #endregion
        }
        if (creatingCharacter)
        {
            characterInstance = CreateInstance<CZ_Character>();
            Name_Character();
            if (characterName != null)
                if (characterName.Length > 0f)
                    if (GUILayout.Button("Save Character"))
                    {
                        Save_Character();
                        creatingCharacter = false;
                    }
            if (GUILayout.Button("Cancel"))
                creatingCharacter = false;
        }
        if(modifyingCharacter)
        {
           
            if (GUILayout.Button("Save Character"))
            {
                AssetDatabase.SaveAssets();
                modifyingCharacter = false;
            }
            if (GUILayout.Button("Cancel"))
                modifyingCharacter = false;
        }
        
    }

    private void Name_Character()
    {
        characterName=EditorGUILayout.TextField("Character Name", characterName);
    }
    private void Save_Character()
    {
        string path = "Assets/Resources/Customization/Characters/"+characterName+".asset";
        characterInstance = CreateInstance<CZ_Character>();
        characterInstance.characterName = characterName;     
        AssetDatabase.CreateAsset(characterInstance, AssetDatabase.GenerateUniqueAssetPath(path));

    }
    private void Find_Character()
    {
        aquireInstance.Find_Character(creatorInstance, characterInstance);
        characterInstance = aquireInstance.characterInstance;
    }
    private void SetCharacterFromName()
    {
        string path = "Assets/Resources/Customization/Characters/" + characterName + ".asset";
        characterInstance = (CZ_Character)AssetDatabase.LoadAssetAtPath(path, typeof(CZ_Character));
    }
    private bool CharacterSectionsMatch()
    {
        //Debug.Log(AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(characterInstance)).Length);
        if (AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(characterInstance)).Length-1 > characterInstance.sections.Count)
        {
            return false;
        }
        else return true;
    }
    public void AssignSections()
    {
        foreach (object section in AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(characterInstance)))
        {
            if (section.GetType() == typeof(CZ_Section))
            {
                if(!characterInstance.sections.Contains((CZ_Section)section))
                    characterInstance.sections.Add((CZ_Section)section);
            }
        }
    }

    public void ListIterator(string propertyPath, ref bool visible, SerializedObject serializedObject, GUIStyle style, string title, string type)
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
