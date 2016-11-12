using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TabVariables : MonoBehaviour {


    private CustomizationMenuManager cMenuManager;

    public GameObject holderObject;

    private GameObject buttonHolder;

    public List<GameObject> possibleSelections = new List<GameObject>();

    public List<float> possibleValues = new List<float>();

    private List<Button> relatedButtons = new List<Button>();

    private bool doneStart =false;
    
    private int amountOfLines;

    //public enum ObjectType
    //{
    //    Hair,
    //    Eye,
    //    Mouth,
    //    Face,
    //    Top
    //}

    public ButtonController.ObjectType thisType;

    void Start ()
    {
        if (name.Contains("Hair"))
            thisType = ButtonController.ObjectType.Hair;
        if (name.Contains("Eye"))
            thisType = ButtonController.ObjectType.Eye;
        if (name.Contains("Mouth"))
            thisType = ButtonController.ObjectType.Mouth;
        if (name.Contains("Face"))
            thisType = ButtonController.ObjectType.Face;
        if (name.Contains("Top"))
            thisType = ButtonController.ObjectType.Top;
        if (name.Contains("Slider"))
            thisType = ButtonController.ObjectType.Value;
        cMenuManager = FindObjectOfType<CustomizationMenuManager>();
        buttonHolder = holderObject.transform.FindChild("ButtonHolder").gameObject;
    }
	void Update()
    {
        if (cMenuManager.doneStart)
            if (!doneStart)
                RunStart();
    }
  
    public void TurnOff()
    {
        holderObject.SetActive(false);
    }
    public void TurnOn()
    {
        holderObject.SetActive(true);
    }
    private void RunStart()
    {
      
        //amountOfLines = possibleSelections.Count / (int)cMenuManager.amountPerRow;
        //if (possibleSelections.Count % cMenuManager.amountPerRow > 0)
        //    amountOfLines++;
        buttonHolder.GetComponent<GridLayoutGroup>().cellSize = new Vector2(cMenuManager.buttonPrefab.targetGraphic.rectTransform.rect.width, cMenuManager.buttonPrefab.targetGraphic.rectTransform.rect.height);
        for (int i = 0; i < possibleSelections.Count; i++)
        {
            relatedButtons.Add((Button)Instantiate(cMenuManager.buttonPrefab, new Vector3(
                cMenuManager.customizationPanel.transform.position.x,
                cMenuManager.customizationPanel.transform.position.y,
                cMenuManager.customizationPanel.transform.position.z),
                cMenuManager.customizationPanel.transform.rotation,
                buttonHolder.transform));
            relatedButtons[i].GetComponent<ButtonController>().thisType = thisType;
            relatedButtons[i].GetComponent<ButtonController>().relatedObject = possibleSelections[i];
        }
        doneStart = true;
    }
}
