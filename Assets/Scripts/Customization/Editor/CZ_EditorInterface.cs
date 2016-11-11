﻿using UnityEngine;
using System.Collections;
using UnityEditor;

public class CZ_EditorInterface : EditorWindow {

    public CZ_Creator creatorInstance;

    public CZ_Aquire aquireInstance;

    public CZ_Character characterInstance;

    private string characterName;
    private bool creatingCharacter;


    private GUIStyle titleStyle = new GUIStyle();


    [MenuItem("Customization/Interface")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        GetWindow(typeof(CZ_EditorInterface));
        GetWindow(typeof(CZ_EditorInterface)).minSize = new Vector2(200, 300);

    }

    public void OnEnable()
    {

        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.fontSize = 15;
        titleStyle.padding.top = 5;
        titleStyle.padding.bottom = 5;
        titleStyle.alignment = TextAnchor.UpperCenter;
        titleContent = new GUIContent("Customization");
        creatorInstance = CreateInstance<CZ_Creator>();
        aquireInstance = CreateInstance<CZ_Aquire>();
        //Find_Character();
    }
    void OnGUI()
    {
       
        if (characterInstance == null)
        {
            if (GUILayout.Button("Find Character"))
            {
                Find_Character();
            }
            if (!creatingCharacter)
            { 
                if (GUILayout.Button("Create New Character"))
                {
                    creatingCharacter = true;
                    //UnityEditor.EditorUtility.SaveFolderPanel();
                }              
            }
            if(creatingCharacter)
            {
                Name_Character();
                if (characterName != null)
                    if (characterName.Length>0f)
                        if(GUILayout.Button("Save Character"))
                        {
                            Save_Character();
                        }
                if (GUILayout.Button("Cancel"))
                    creatingCharacter = false;
            }
        }
        else
        {
        
            GUILayout.Label("Currently viewing " + characterInstance.characterName, titleStyle);
            EditorGUILayout.Separator();
            characterInstance.baseModel = (GameObject)EditorGUILayout.ObjectField("Base Model",characterInstance.baseModel, typeof(GameObject),false);
            EditorGUILayout.Space();
            #region Variables
            Rect leftRect = new Rect(0, 60, Screen.width / 2-5, Screen.height);
            GUIStyle leftStyle = new GUIStyle();
            Texture2D leftTexture = new Texture2D((int)leftRect.width, (int)leftRect.height);
            Color[] leftColors = new Color[1];
            leftColors[0] = Color.grey;
            //leftTexture.SetPixels(leftColors);
            leftStyle.normal.background =leftTexture ;
            GUI.Box(leftRect, "",leftStyle);
            GUILayout.BeginArea(leftRect);
            GUILayout.Label("Variables", titleStyle);
            if(GUILayout.Button("Find Variables"))
            {
                string entirePath = EditorUtility.OpenFolderPanel("Variables Folder", "/Assets/Resources/Customization/Variables/", "");
                string relativepath = "Assets" + entirePath.Substring(Application.dataPath.Length);
                aquireInstance.foldersToSearch.Add(relativepath);
                aquireInstance.Aquire_Variables();
            }
            if(GUILayout.Button("Create New Variable"))
            {
                creatorInstance.Create_Variable();
            }
            if(creatorInstance.creating)
            {

            }

            GUILayout.EndArea();
            #endregion
            #region
            Rect rightRect = new Rect(Screen.width / 2 + 5, 60, Screen.width / 2, Screen.height);
            GUIStyle rightStyle = new GUIStyle();
            Texture2D rightTexture = new Texture2D((int)rightRect.width, (int)rightRect.height);
            Color[] rightColors = new Color[1];
            rightColors[0] = Color.grey;
            //rightTexture.SetPixels(rightColors);
            rightStyle.normal.background = rightTexture;
            GUI.Box(rightRect, "", rightStyle);
            GUILayout.BeginArea(rightRect);
            GUILayout.Label("Sections", titleStyle);
            GUILayout.EndArea();
            #endregion
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
        AssetDatabase.CreateAsset(characterInstance, path);

    }
    private void Find_Character()
    {
        string path = "Assets/Resources/Customization/Characters/";
        string[] filters = new string[2];
        filters[0] = "Asset Files";
        filters[0] = "asset";
        string fileLocation = EditorUtility.OpenFilePanelWithFilters("Load Character", path,filters);
        string relativepath = "Assets" + fileLocation.Substring(Application.dataPath.Length);
        //Debug.Log(relativepath);
        characterInstance = (CZ_Character)AssetDatabase.LoadAssetAtPath(relativepath, typeof(CZ_Character));
        aquireInstance.characterInstance = characterInstance;
        creatorInstance.characterInstance = characterInstance;
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
