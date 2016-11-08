using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CustomizationMenuManager : MonoBehaviour {

    public enum State
    {
        Customizing,
        Saving,
        Closed
    }

    public State currentState;

    public GameObject customizationPanel;

    private List<Button> tabs = new List<Button>();

    private PlayerCustomizationVariables playerCVariables;

    public Button activeButton;

    public Button buttonPrefab;

    public List<Transform> placementPostions;

    public GameObject currentHair;

    public GameObject currentEye;

    public GameObject currentMouth;

    public GameObject currentFace;

    public GameObject currentTop;

    public bool doneStart;

	void Start ()
    {
        foreach (Transform tab in customizationPanel.transform.GetComponentInChildren<Transform>())
        {
            if (tab.tag.Contains("Tab"))
                tabs.Add(tab.GetComponent<Button>());
        }
        activeButton = tabs[0];
        foreach (Button button in tabs)
        {
            if (button != activeButton)
                button.GetComponent<TabVariables>().TurnOff();
        }     
        doneStart = true;
        Load(true);
    }
	
	void Update ()
    {
        switch (currentState)
        {
            case State.Customizing:
                if(!customizationPanel.activeInHierarchy)
                    customizationPanel.SetActive(true);
                ApplyActiveButton(activeButton);
                break;
            case State.Saving:
                Save();
                break;
            case State.Closed:
                if (customizationPanel.activeInHierarchy)
                    customizationPanel.SetActive(false);
                break;
            default:
                break;
        }
     
    }
    private void SetActiveButton(Button toUse)
    {
        activeButton.GetComponent<TabVariables>().TurnOff();
        activeButton.interactable = true;
        activeButton = toUse;
        activeButton.GetComponent<TabVariables>().TurnOn();
    }
    public void ApplyActiveButton(Button toUse)
    {
        SetActiveButton(toUse);
        toUse.interactable = false;
    }
    public void GrabPlacementPositions()
    {
        foreach (GameObject gameObj in GameObject.FindGameObjectsWithTag("PlacementPosition"))
        {
            placementPostions.Add(gameObj.transform);
        }
    }
    public void CloseCustomization()
    {       
        currentState = State.Saving;
    }
    private void Save()
    {
        if ((PlayerCustomizationVariables)UnityEditor.AssetDatabase.LoadAssetAtPath
           ("Assets/Resources/PlayerCustomization.asset",
           typeof(PlayerCustomizationVariables)) == null)
            {
            playerCVariables = ScriptableObject.CreateInstance<PlayerCustomizationVariables>();
            UnityEditor.AssetDatabase.CreateAsset(playerCVariables, "Assets/Resources/PlayerCustomization.asset");
            }
        if (currentEye != null)
            playerCVariables.eyes = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(UnityEditor.AssetDatabase.GetAssetPath(currentEye.GetComponent<CustomizationObjectVariables>().prefabReference));
        if (currentHair != null)
            playerCVariables.hair = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(UnityEditor.AssetDatabase.GetAssetPath(currentHair.GetComponent<CustomizationObjectVariables>().prefabReference));
        if (currentFace != null)
            playerCVariables.face = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(UnityEditor.AssetDatabase.GetAssetPath(currentFace.GetComponent<CustomizationObjectVariables>().prefabReference));
        if (currentMouth != null)
            playerCVariables.mouth = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(UnityEditor.AssetDatabase.GetAssetPath(currentMouth.GetComponent<CustomizationObjectVariables>().prefabReference));
        if (currentTop != null)
            playerCVariables.top = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(UnityEditor.AssetDatabase.GetAssetPath(currentTop.GetComponent<CustomizationObjectVariables>().prefabReference));
        UnityEditor.AssetDatabase.SaveAssets();
        currentState = State.Closed;
    }
    private void Load(bool alsoCreate)
    {
        if ((PlayerCustomizationVariables)UnityEditor.AssetDatabase.LoadAssetAtPath
          ("Assets/Resources/PlayerCustomization.asset",
          typeof(PlayerCustomizationVariables)) != null)
        {
            playerCVariables = (PlayerCustomizationVariables)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Resources/PlayerCustomization.asset", typeof(PlayerCustomizationVariables));
        }
        if(alsoCreate)
        {
            foreach (Transform pos in placementPostions)
            {
                if (pos.name.Contains("Hair"))
                {
                    currentHair = (GameObject)Instantiate(playerCVariables.hair, pos.transform.position, playerCVariables.hair.transform.rotation, pos.transform);
                    currentHair.AddComponent<CustomizationObjectVariables>().prefabReference = playerCVariables.hair;
                }
                if (pos.name.Contains("Eye"))
                {
                    currentEye = (GameObject)Instantiate(playerCVariables.eyes, pos.transform.position, playerCVariables.eyes.transform.rotation, pos.transform);
                    currentEye.AddComponent<CustomizationObjectVariables>().prefabReference = playerCVariables.eyes;
                }
                if (pos.name.Contains("Face"))
                {
                    currentFace = (GameObject)Instantiate(playerCVariables.face, pos.transform.position, playerCVariables.face.transform.rotation, pos.transform);
                    currentFace.AddComponent<CustomizationObjectVariables>().prefabReference = playerCVariables.face;
                }
                if (pos.name.Contains("Mouth"))
                {
                    currentMouth = (GameObject)Instantiate(playerCVariables.mouth, pos.transform.position, playerCVariables.mouth.transform.rotation, pos.transform);
                    currentMouth.AddComponent<CustomizationObjectVariables>().prefabReference = playerCVariables.mouth;
                }
                if (pos.name.Contains("Top"))
                {
                    currentTop = (GameObject)Instantiate(playerCVariables.top, pos.transform.position, playerCVariables.top.transform.rotation, pos.transform);
                    currentTop.AddComponent<CustomizationObjectVariables>().prefabReference = playerCVariables.top;
                }
            }
        }
    }
}
