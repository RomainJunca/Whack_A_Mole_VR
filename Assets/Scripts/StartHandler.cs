using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartHandler : MonoBehaviour {

    public GameObject menu;
    public GameObject easy, medium, hard, gradual;

    private WallController wallCtrl;
    private GameObject currentMode;
    private string currentModeString;
    private Material[] original_Textures = new Material[2];
    private string nameGO;
    private float clicked = 0;
    private float clicktime = 0;
    private float clickdelay = 0.2f;
    private bool hasDoubleClicked = false;
    private string currentModeStringClicked = "";

    // Use this for initialization
    void Start () {
        original_Textures = easy.GetComponent<MeshRenderer>().materials;
        wallCtrl = gameObject.GetComponent<WallController>();
	}

    // Update is called once per frame
    void Update()
    {

        //We watch for the position of the mouse on the game scene
        //We also watch for double click
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                nameGO = hit.transform.name;
                clicked++;
                if (clicked == 1) clicktime = Time.deltaTime;
                if (clicked > 1 && Time.deltaTime - clicktime < clickdelay)
                {
                    if(currentModeString != "")
                    {
                        if(currentModeString == currentModeStringClicked)
                        {
                            hasDoubleClicked = true;
                        }
                        currentModeStringClicked = currentModeString;
                    }

                    clicked = 0;
                    clicktime = 0;
                }
                print(nameGO);
            }
            else { nameGO = ""; }
        }
        else
        {
            nameGO = "";
        }

        //We watch for the key pressed for the selected mode or the mouse button pressed on the gameobject
        if (Input.GetKeyDown("1") || Input.GetKeyDown("[1]") || nameGO == "Easy")
        {
            changeCurrentButton();
            currentMode = easy;
            currentModeString = "easy";
            buttonSelected(currentModeString);
        }
        else if (Input.GetKeyDown("2") || Input.GetKeyDown("[2]") || nameGO == "Medium")
        {
            changeCurrentButton();
            currentMode = medium;
            currentModeString = "medium";
            buttonSelected(currentModeString);
        }
        else if (Input.GetKeyDown("3") || Input.GetKeyDown("[3]") || nameGO == "Hard")
        {
            changeCurrentButton();
            currentMode = hard;
            currentModeString = "hard";
            buttonSelected(currentModeString);
        }
        else if(Input.GetKeyDown("4") || Input.GetKeyDown("[4]") || nameGO == "Gradual")
        {
            changeCurrentButton();
            currentMode = gradual;
            currentModeString = "gradual";
            buttonSelected(currentModeString);
        }

        //Launching a mode (either by pressing return or double clicking on the button)
        if ((Input.GetKeyDown("return") && currentMode != null) || (hasDoubleClicked && (currentModeString == "easy" || currentModeString == "medium" || currentModeString == "hard" || currentModeString == "gradual")))
        {
            hasDoubleClicked = false;
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
                case "gradual":
                    wallCtrl.mode = 4;
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
            case "gradual":
                buttonHighlight(gradual);
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
