using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : MonoBehaviour{

    public bool isActive = false;
    public int redOdds = 8;
    public AudioSource wii;
    public string currentColor;

    private GameController gmCtrl;
    private float lifeTime = 5f;
    private float timer = 0f;
    private bool startShining = true;
    private bool startNormal = true;
    private Material[] moleMaterial = new Material[2];

    void Start()
    {
        wii = gameObject.GetComponent<AudioSource>();
        gmCtrl = GameObject.Find("GameController").GetComponent<GameController>();
    }

    void Update()
    {
        if (isActive)
        {
            timer += Time.deltaTime;
            if (timer > lifeTime) //During the lifeTime of the mall, we make it shine
            {
                if (startNormal)
                {
                    addMaterials(null); //Make the mole going back to normal
                    startNormal = false;
                }
                timer = 0;
                startShining = true;
                isActive = false;
            }
            else
            {
                if (startShining)
                {
                    makeItShine();
                    startShining = false;
                }
                startNormal = true;
            }
        }
    }

    public void makeItShine() //Make the mole shine in green or red
    {
        //Material currentMaterial;
        int odds = Random.Range(0, redOdds);
        if (odds != redOdds / 2)
        {
            //currentMaterial = (Material)Resources.Load("Materials/green");
            addMaterials("green");
            currentColor = "green";
        }
        else
        {
            //currentMaterial = (Material)Resources.Load("Materials/red");
            addMaterials("red");
            currentColor = "red";
        }

        /*moleMaterial[0] = (Material)Resources.Load("Materials/mole");
        moleMaterial[1] = currentMaterial;
        gameObject.GetComponent<MeshRenderer>().materials = moleMaterial;*/
        
        wii.time = 0.52f; //Play directly at the beginning of the sound
        wii.Play();
    }

   /* public void makeItNormal() 
    {
        addMaterials(null);
    }*/

    public void glow()
    {
        if(isActive){
            if(moleMaterial[1].name == "green")
            {
                addMaterials("green_Glow");

            }
            else if(moleMaterial[1].name == "red")
            {
                addMaterials("red_Glow");
            }
        }
    }

    public void addMaterials(string secondMaterial)
    {
        moleMaterial[0] = (Material)Resources.Load("Materials/mole");
        switch (secondMaterial)
        {
            case "green":
                moleMaterial[1] = (Material)Resources.Load("Materials/green");
                break;
            case "red":
                moleMaterial[1] = (Material)Resources.Load("Materials/red");
                break;
            case "green_Glow":
                moleMaterial[1] = (Material)Resources.Load("Materials/green_Glow");
                moleMaterial[1].shader = (Shader)Resources.Load("Shaders/Glow");
                break;
            case "red_Glow":
                moleMaterial[1] = (Material)Resources.Load("Materials/red_Glow");
                moleMaterial[1].shader = (Shader)Resources.Load("Shaders/Glow");
                break;
            default:
                moleMaterial[1] = null;
                break;
        }
        gameObject.GetComponent<MeshRenderer>().materials = moleMaterial;
    }
}
