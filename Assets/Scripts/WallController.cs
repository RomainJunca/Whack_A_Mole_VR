using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour {

    public GameObject cam;
    public GameObject wall_with_moles;
    public GameObject spawnPoints;
    public GameObject molePrefab;
    public List<GameObject> molesList;
    public int mode;
    public bool start = false;
    public int totalMolesWhacked = 0;
    public int maxMoles = 2;

    private float timer;
    private float rndTime = 1f;
    private GameObject currentMole;
    private int indexCurrentMole;
    private float moleLifeTime = 1f;

	void Start () {
        molesList = new List<GameObject>();
        generateMoles(spawnPoints, molePrefab, wall_with_moles);
	}

	void Update () {

        gameObject.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y-0.8f, cam.transform.position.z + 1); //The wall follows the camera

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
                return Random.Range(3f, 4f);
            case 2:
                maxMoles = 4;
                return Random.Range(1.5f, 3f);
            case 3:
                maxMoles = 6;
                return Random.Range(0.5f, 1.5f);
            case 4:
                return gradual();
            default:
                return 1f;
        }
    }

    private float gradual() //The timer changes gradually depending on the number of the whacked moles
    {
        if(totalMolesWhacked > 5)
        {
            maxMoles = 4;
            return Random.Range(1.5f, 3f);
        }
        else if(totalMolesWhacked > 10)
        {
            maxMoles = 6;
            return Random.Range(0.5f, 1.5f);
        }
        else
        {
            maxMoles = 2;
            return Random.Range(3f, 4f);
        }
    }

}
