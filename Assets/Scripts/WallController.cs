using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour {

    public GameObject cam;
    public GameObject wall_with_moles;
    public GameObject spawnPoints;
    public GameObject molePrefab;
    public GameObject gameOver;
    public List<GameObject> molesList;
    public int mode;
    public bool start = false;
    public int totalMolesWhacked = 0;
    public int totalMissedMoles = 0;
    public int maxMoles = 2;    //Max of moles which can appear at the same time (depending of the level)
    public int maxMissed = 5;   //Max of moles which can be missed before game over (depending of the level)
    public int redMissed = 0;

    private float timer;
    private float rndTime = 1f;
    private GameObject currentMole;
    private int indexCurrentMole;
    private float moleLifeTime = 1f;

	void Start () {
        molesList = new List<GameObject>();
        gameOver.SetActive(false);
        generateMoles(spawnPoints, molePrefab, wall_with_moles);
	}

	void Update () {

        gameObject.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y-1.85f, cam.transform.position.z + 1); //The wall follows the camera
        //   10        -    2       -    12
        if(totalMissedMoles + redMissed > maxMissed) //If too much missed -> Game over menu -> we display the results
        {
            molesBackToNormal(molesList); //We reset the moles materials to normal
            start = false;
            gameOver.transform.Find("Results").GetComponent<TextMesh>().text = " \nMoles missed :\n"+ totalMissedMoles + "\n Green moles whacked:\n"+totalMolesWhacked+"\n Red moles whacked:\n"+redMissed+"\n ";
            gameOver.SetActive(true);
        }

        if (start && mode != 0)
        {
            timer += Time.deltaTime;
            if (timer >= rndTime) //We select a new mole every random timer
            {
                currentMole = selectMole(molesList);
                timer = 0.0f;
                rndTime = generateTimer(mode);
            }
        }
	}

    private void generateMoles(GameObject spawnpoints, GameObject prefab, GameObject wall) //We generate the moles on the spawnpoints
    {
        foreach (Transform child in spawnpoints.transform) //We instantiate a mole for each spawn point
        {
            GameObject clone = Instantiate(prefab, child.transform.position, Quaternion.identity) as GameObject;
            clone.transform.SetParent(wall.transform);
            clone.transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
            molesList.Add(clone);
        }
    }

    private GameObject selectMole(List<GameObject> listMoles) //Select a random mole
    {
        int index;
        //We look for a new mole, different from the previous, who isn't active and while the number of activated moles is less than the max number
        do
        {
            index = Random.Range(0, listMoles.Count);
            
        }while(index == indexCurrentMole && listMoles[index].GetComponent<Mole>().isActive && molesActiveCount(listMoles) > maxMoles); 
        indexCurrentMole = index;
        listMoles[index].GetComponent<Mole>().isActive = true;

        return listMoles[indexCurrentMole];
    }

    private int molesActiveCount(List<GameObject> listMoles) //return the number of active mole
    {
        int count = 0;
        foreach(GameObject mole in listMoles)
        {
            if (mole.GetComponent<Mole>().isActive)
            {
                count++;
            }
        }

        return count;
    }

    private float generateTimer(int lvl) //The timer before mole's appearance depending on the level
    {
        switch (lvl)
        {
            case 1:
                maxMoles = 2;
                maxMissed = 5;
                return Random.Range(3f, 4f);
            case 2:
                maxMoles = 4;
                maxMissed = 3;
                return Random.Range(1.5f, 3f);
            case 3:
                maxMoles = 6;
                maxMissed = 2;
                return Random.Range(0.5f, 1.5f);
            case 4:
                return gradual();
            default:
                return 1f;
        }
    }

    private float gradual() //The timer changes gradually depending on the number of the whacked moles
    {
        if(totalMolesWhacked > 4 && totalMolesWhacked <= 8)
        {
            maxMoles = 4;
            maxMissed = 4;
            return Random.Range(1.5f, 3f);
        }
        else if(totalMolesWhacked > 9 && totalMolesWhacked <= 16)
        {
            maxMoles = 6;
            maxMissed = 3;
            return Random.Range(0.9f, 1.5f);
        }
        else if(totalMolesWhacked > 17)
        {
            maxMoles = 8;
            maxMissed = 2;
            return Random.Range(0.2f, 0.9f);
        }
        else
        {
            maxMoles = 2;
            maxMissed = 5;
            return Random.Range(3f, 4f);
        }
    }

    private void molesBackToNormal(List<GameObject> molesList)
    {
        foreach(GameObject mole in molesList)
        {
            if (mole.GetComponent<MeshRenderer>().materials.Length != 1)
            {
                mole.GetComponent<Mole>().addMaterials(null);
            }
        }
    }

}
