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

        gameObject.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y-1.2f, cam.transform.position.z + 1); //The wall follows the camera

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
        do
        {
            index = Random.Range(0, listMoles.Count);
            
        }while(index == indexCurrentMole && listMoles[index].GetComponent<Mole>().isActive);
        indexCurrentMole = index;
        listMoles[index].GetComponent<Mole>().isActive = true;

        return listMoles[indexCurrentMole];
    }

    private float generateTimer(int lvl) //The timer before mole's appearance depending on the level
    {
        if(lvl == 1)
        {
            return Random.Range(1f, 3f);
        }
        else if(lvl == 2)
        {
            return Random.Range(0.5f, 2f);
        }
        else if(lvl == 3)
        {
            return Random.Range(0.1f, 1f);
        }
        else
        {
            return 1f;
        }
    }

}
