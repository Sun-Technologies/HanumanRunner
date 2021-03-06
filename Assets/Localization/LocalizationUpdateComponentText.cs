using UnityEngine;
using System.Collections;

public class LocalizationUpdateComponentText : MonoBehaviour{

    //At the start gets the actuall Language
    private string _language = LocalizationText.GetLanguage();
	
	//GameObjects with managed Text
	//Add as many of these as you have 3d Text Meshes
	private GameObject WelcomeText;
	private GameObject lblCastle;
	private GameObject CarName;
	
	void Start()
	{
		//You dont need to change anything here
		SetAllObjects();
		SetAllText();
	}
	
	// Update is called once per frame
	void Update () 
    {		
		//You dont need to change anything here
        //if the language should have been changed it will set all texts referred in SetAllText to the new one.
        if (LocalizationText.GetLanguage() != _language)
        {
            _language = LocalizationText.GetLanguage();
            SetAllText();
        }
	}
	
	private void SetAllObjects()
	{
		//This should be equal long to your GameObject list above, because here we get our reference for later operation.
		WelcomeText = GameObject.Find("Welcome");
		lblCastle = GameObject.Find("lblCastle");
		CarName = GameObject.Find("carName");
	}
    private void SetAllText()
    {
		//Here wechange the Text of all referenced 3D Text Meshes.
		
		// ALWAYS Check if the GameObject reference is null, when you dont do it and one should be null then no one will work.
		// but if you do the check. Only the one with the gameobject null wont work.
		if(WelcomeText!=null)
        	WelcomeText.GetComponent<TextMesh>().text = LocalizationText.GetText("lblDoor111");
		if(lblCastle!=null)
			lblCastle.GetComponent<TextMesh>().text = LocalizationText.GetText("lblCastle");
		if(CarName!=null)
			CarName.GetComponent<TextMesh>().text = LocalizationText.GetText("CarName");
    }
}
