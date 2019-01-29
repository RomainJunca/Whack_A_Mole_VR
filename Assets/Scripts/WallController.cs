using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour {

    public GameObject cam;
    public GameObject wall_with_moles;
    public int redOdds = 4;

    private List<GameObject> molesList;
    private float timer = 1f;
    private GameObject currentMole;
    private Material[] moleMaterial = new Material[2];
    private int indexCurrentMole;

	void Start () {
        molesList = new List<GameObject>();
        putMolesInList(wall_with_moles);
	}
	
	void Update () {

        gameObject.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y-1.5f, cam.transform.position.z + 1); //The wall follows the camera

        timer += Time.deltaTime;
        if(timer >= 1.0f) //We select a new mole every second
        {
            if(currentMole != null)
            {
                makeItNormal(currentMole);
            }
            currentMole = selectMole(molesList);
            makeItShine(currentMole);
            timer = 0.0f;
        }

	}

    private void putMolesInList(GameObject listMoles)  //Put the moles in list
    {
        foreach (Transform child in listMoles.transform)
        {
            molesList.Add(child.gameObject);
            print(child.name);
        }

        print(molesList.Count);
    }

    private GameObject selectMole(List<GameObject> listMoles) //Select a random mole
    {
        int index;
        do
        {
            index = Random.Range(0, listMoles.Count);
        } while (index == indexCurrentMole);
        indexCurrentMole = index;

        return listMoles[indexCurrentMole];
    }

    private void makeItShine(GameObject mole) //Make the mole shine in green or red
    {
        Material currentMaterial;
        int odds = Random.Range(0, redOdds);
        if(odds != redOdds / 2)
        {
            currentMaterial = (Material)Resources.Load("Materials/green");
        }
        else
        {
            currentMaterial = (Material)Resources.Load("Materials/red");
        }

        moleMaterial[0] = (Material)Resources.Load("Materials/mole");
        moleMaterial[1] = currentMaterial;
        currentMole.GetComponent<MeshRenderer>().materials = moleMaterial;
    }

    private void makeItNormal(GameObject mole) //Make the mole going back to normal
    {
        moleMaterial[0] = (Material)Resources.Load("Materials/mole");
        moleMaterial[1] = null;
        currentMole.GetComponent<MeshRenderer>().materials = moleMaterial;
    }
}
