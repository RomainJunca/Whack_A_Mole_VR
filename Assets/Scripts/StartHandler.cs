using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartHandler : MonoBehaviour {

    public GameObject menu;
    public GameObject easy, medium, hard;

    private WallController wallCtrl;
    private GameObject currentMode;
    private string currentModeString;
    private Material[] original_Textures = new Material[2];

	// Use this for initialization
	void Start () {
        original_Textures = easy.GetComponent<MeshRenderer>().materials;
        wallCtrl = gameObject.GetComponent<WallController>();
	}

    // Update is called once per frame
    void Update()
    {
        //We watch for the key pressed for the selected mode
        if (Input.GetKeyDown("1") || Input.GetKeyDown("[1]"))
        {
            changeCurrentButton();
            currentMode = easy;
            currentModeString = "easy";
            buttonSelected(currentModeString);
        }
        else if (Input.GetKeyDown("2") || Input.GetKeyDown("[2]"))
        {
            changeCurrentButton();
            currentMode = medium;
            currentModeString = "medium";
            buttonSelected(currentModeString);
        }
        else if (Input.GetKeyDown("3") || Input.GetKeyDown("[3]"))
        {
            changeCurrentButton();
            currentMode = hard;
            currentModeString = "hard";
            buttonSelected(currentModeString);
        }

        //Launching a mode
        if (Input.GetKeyDown("return") && currentMode != null)
        {
            menu.transform.position = new Vector3(menu.transform.position.x, menu.transform.position.y, menu.transform.position.z + 1);
            menu.SetActive(false);

            switch (currentModeString)
            {
                case "easy":
                    wallCtrl.mode = 1;
                    break;
                case "medium":
                    wallCtrl.mode = 2;
                    break;
                case "hard":
                    wallCtrl.mode = 3;
                    break;
                default:
                    break;
            }
            wallCtrl.start = true;
        }

    }

    //We highlight the selected button
    private void buttonSelected(string button) 
    {
        switch(button) 
        {
            case "easy":
                buttonHighlight(easy);
                break;
            case "medium":
                buttonHighlight(medium);
                break;
            case "hard":
                buttonHighlight(hard);
                break;
            default:
                break;
        }
    }

    private void buttonHighlight(GameObject button)
    {
        Material[] materials = new Material[2];

        materials = button.GetComponent<MeshRenderer>().materials;
        materials[1] = (Material)Resources.Load("Materials/selectedButtons");
        button.GetComponent<MeshRenderer>().materials = materials;
    }

    private void changeCurrentButton() //We change the previous selected button's material
    {
        if (currentMode)
        {
            currentMode.GetComponent<MeshRenderer>().materials = original_Textures;
        }
    }
}
