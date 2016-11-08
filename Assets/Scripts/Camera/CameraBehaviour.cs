using UnityEngine;
using System.Collections;
using Rewired;
using TriTools;
public class CameraBehaviour : MonoBehaviour {
    private CustomizationMenuManager customizationMenuManager;
    public enum ControlState
    {
        NoControl,
        Control
    }
    ControlState currControlState;
    public enum CameraState
    {
        Normal
    }
    CameraState currCameraState;

    public float rotationSpeed;

    public GameObject playerGameObject;

    private Player player;

    public float offsetPosition;

	void Start ()
    {
        customizationMenuManager = FindObjectOfType<CustomizationMenuManager>();
        playerGameObject = GameObject.FindGameObjectWithTag("Player");
        player = ReInput.players.GetPlayer(0);
	}
	

	void FixedUpdate ()
    {
        if (customizationMenuManager.currentState == CustomizationMenuManager.State.Closed)
            currControlState = ControlState.Control;
        if(currControlState == ControlState.Control)
        {
            if(currCameraState == CameraState.Normal)
            {
                CameraRotation();
                CameraPosition();
            }
        }
	}
    void CameraRotation()
    {
        float hAxis = player.GetAxis("Horizontal Alt");
        float vAxis = player.GetAxis("Vertical Alt");
        //TriToolHub.Rotate(gameObject, new Vector3(vAxis * rotationSpeed, hAxis*rotationSpeed, 0), true, Space.World);
  
        
        transform.RotateAround(playerGameObject.transform.position, transform.up,hAxis*rotationSpeed*Time.deltaTime);
        transform.RotateAround(playerGameObject.transform.position, transform.right, vAxis * rotationSpeed * Time.deltaTime);
        TriToolHub.SetRotation(gameObject, TriToolHub.XYZ.Z, 0, Space.World);
    }
    void CameraPosition()
    {
        //Vector3 toPosition = new Vector3(playerGameObject.transform.position.x+offsetPosition, playerGameObject.transform.position.y + offsetPosition, playerGameObject.transform.position.z + offsetPosition);
        //transform.position = Vector3.Lerp(transform.position, toPosition, Time.deltaTime);
    }
}
