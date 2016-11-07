using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TabVariables : MonoBehaviour {

    private CustomizationMenuManager cMenuManager;

    public GameObject holderObject;

    private GameObject buttonHolder;

    public List<GameObject> possibleSelections = new List<GameObject>();

    private List<Button> relatedButtons = new List<Button>();

    private bool doneStart =false;
    
    private int amountOfLines;

	void Start ()
    {
        cMenuManager = FindObjectOfType<CustomizationMenuManager>();
        buttonHolder = holderObject.transform.FindChild("ButtonHolder").gameObject;
    }
	void Update()
    {
        if (cMenuManager.doneStart)
            if (!doneStart)
                RunStart();
    }
    private float IfOverForY(int i,int greaterThan, float returnFloat)
    {
        if (i > greaterThan)
        {
            return returnFloat;
        }
        else return 0f;
    }
    private float IfOverForX(int i,int greaterThan,float returnFloat)
    {
        if (i > greaterThan)
        {
            return returnFloat;
        }
        else return 0f;
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
      
        amountOfLines = possibleSelections.Count / (int)cMenuManager.amountPerRow;
        if (possibleSelections.Count % cMenuManager.amountPerRow > 0)
            amountOfLines++;
        buttonHolder.GetComponent<GridLayoutGroup>().cellSize = new Vector2(cMenuManager.buttonPrefab.targetGraphic.rectTransform.rect.width, cMenuManager.buttonPrefab.targetGraphic.rectTransform.rect.height);
        for (int i = 0; i < possibleSelections.Count; i++)
        {
          
               // if (relatedButtons.Count < possibleSelections.Count)
            relatedButtons.Add((Button)Instantiate(cMenuManager.buttonPrefab, new Vector3(
                cMenuManager.customizationPanel.transform.position.x,
                cMenuManager.customizationPanel.transform.position.y,
                cMenuManager.customizationPanel.transform.position.z),
                cMenuManager.customizationPanel.transform.rotation,
                buttonHolder.transform));
        }
        doneStart = true;
    }
}
