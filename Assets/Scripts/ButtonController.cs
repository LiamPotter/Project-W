using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

public class ButtonController : MonoBehaviour {

    public enum ObjectType
    {
        Hair,
        Eye,
        Mouth,
        Face,
        Top,
        Value
    }

    public ObjectType thisType;

    public GameObject relatedObject;

    public Transform toSpawnPosition;

    public void OnPressed()
    {
        CustomizationMenuManager cMenuManager = FindObjectOfType<CustomizationMenuManager>();

        for (int i = 0; i < cMenuManager.placementPostions.Count; i++)
        {
            switch (thisType)
            {
                case ObjectType.Hair:
                    if (cMenuManager.placementPostions[i].name.Contains("Hair"))
                    {
                        toSpawnPosition = cMenuManager.placementPostions[i];
                        cMenuManager.currentHair= CreateRelated(cMenuManager.currentHair, toSpawnPosition);
                    }
                    break;
                case ObjectType.Eye:
                    if (cMenuManager.placementPostions[i].name.Contains("Eye"))
                    {
                        toSpawnPosition = cMenuManager.placementPostions[i];
                        cMenuManager.currentEye=CreateRelated(cMenuManager.currentEye, toSpawnPosition);
                    }
                    break;
                case ObjectType.Mouth:
                    if (cMenuManager.placementPostions[i].name.Contains("Mouth"))
                    {
                        toSpawnPosition = cMenuManager.placementPostions[i];
                        cMenuManager.currentMouth=CreateRelated(cMenuManager.currentMouth, toSpawnPosition);
                    }
                    break;
                case ObjectType.Face:
                    if (cMenuManager.placementPostions[i].name.Contains("Face"))
                    {
                        toSpawnPosition = cMenuManager.placementPostions[i];
                        cMenuManager.currentFace=CreateRelated(cMenuManager.currentFace, toSpawnPosition);
                    }
                    break;
                case ObjectType.Top:
                    if (cMenuManager.placementPostions[i].name.Contains("Top"))
                    {
                        toSpawnPosition = cMenuManager.placementPostions[i];
                        cMenuManager.currentTop=CreateRelated(cMenuManager.currentTop, toSpawnPosition);
                    }
                    break;
                case ObjectType.Value:

                    break;
                default:
                    Debug.Log("You've fucked something up.");
                    break;
            }
        }
    }
    
    private GameObject CreateRelated(GameObject managerGameObject,Transform toSpawnPos)
    {
        if (managerGameObject != null)
            Destroy(managerGameObject);
        managerGameObject = (GameObject)Instantiate(relatedObject, toSpawnPosition.position, relatedObject.transform.rotation, toSpawnPosition);
        managerGameObject.AddComponent<CustomizationObjectVariables>().prefabReference = relatedObject;
        return managerGameObject;
      
    }
}
