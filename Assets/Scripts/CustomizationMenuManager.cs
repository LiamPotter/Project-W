using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CustomizationMenuManager : MonoBehaviour {

    public GameObject customizationPanel;

    private List<Button> tabs = new List<Button>();

    public Button activeButton;

    public Button buttonPrefab;

    public float panelMaxWidth;

    public float panelMaxHeight;

    public float heightPadding;

    public float widthPadding;

    public float amountOfRows;

    public float amountPerRow;

    public bool doneStart;
	// Use this for initialization
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

        panelMaxWidth = customizationPanel.GetComponent<RectTransform>().rect.width;
        panelMaxHeight = customizationPanel.GetComponent<RectTransform>().rect.height;
        amountPerRow = Mathf.Floor(panelMaxWidth / (buttonPrefab.targetGraphic.rectTransform.rect.width ));
        amountOfRows = Mathf.Ceil(panelMaxHeight / (buttonPrefab.targetGraphic.rectTransform.rect.height + heightPadding)-1);


        doneStart = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        ApplyActiveButton(activeButton);
        
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
}
