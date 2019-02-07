using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{

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
    public int redWhacked = 0;

    private float timer;
    private float rndTime = 1f;
    private GameObject currentMole;
    private int indexCurrentMole;
    private int doOnce = 0;

    private int totalMissedMolesSave = 0, redWhackedSave = 0;

    private bool stop = false;

    void Start()
    {
        molesList = new List<GameObject>();
        gameOver.SetActive(false);
        generateMoles(spawnPoints, molePrefab, wall_with_moles);
    }

    void Update()
    {

        gameObject.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y - 1.85f, cam.transform.position.z + 1); //The wall follows the camera

        if (totalMissedMoles + redWhacked > maxMissed && !stop) //If too much missed -> Game over menu -> we display the results
        {
            molesBackToNormal(molesList); //We reset the moles materials to normal
            start = false;
            stop = true;

            if (mode == 4)
            {
                totalMissedMoles += totalMissedMolesSave;
                redWhacked += redWhackedSave;
            }
            gameOver.transform.Find("Results").GetComponent<TextMesh>().text = " \nMoles missed :\n" + totalMissedMoles + "\n Green moles whacked:\n" + totalMolesWhacked + "\n Red moles whacked:\n" + redWhacked + "\n ";
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

        } while (index == indexCurrentMole && listMoles[index].GetComponent<Mole>().isActive && molesActiveCount(listMoles) > maxMoles);
        indexCurrentMole = index;
        listMoles[index].GetComponent<Mole>().isActive = true;
        listMoles[index].GetComponent<Mole>().timer = 0;
        return listMoles[indexCurrentMole];
    }

    private int molesActiveCount(List<GameObject> listMoles) //return the number of active mole
    {
        int count = 0;
        foreach (GameObject mole in listMoles)
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
        //For each step in this mode, we change/reste values such as : max moles that can be missed, nbr of moles (green or red) whacked, and new range and time of appearances.
        if (totalMolesWhacked > 4 && totalMolesWhacked <= 8)
        {
            return changeModeValues(4, 4, 2f, 2.75f, 1, 4);
        }
        else if (totalMolesWhacked > 9 && totalMolesWhacked <= 16)
        {
            return changeModeValues(6, 3, 1.25f, 2f, 2, 3);
        }
        else if (totalMolesWhacked > 17 && totalMolesWhacked <= 24)
        {
            return changeModeValues(8, 2, 0.5f, 1.25f, 3, 2);
        }
        else if(totalMolesWhacked  > 24)
        {           
            return changeModeValues(10, 2, 0.1f, 0.8f, 3, 2);
        }
        else
        {
            return changeModeValues(2, 5, 2.75f, 3.5f, 0, 5);
        }
    }

    private void molesBackToNormal(List<GameObject> molesList)
    {
        foreach (GameObject mole in molesList)
        {
            if (mole.GetComponent<MeshRenderer>().materials.Length != 1)
            {
                mole.GetComponent<Mole>().addMaterials(null);
            }
        }
    }

    private float changeModeValues(int molesMax, int missedMax, float minRange, float maxRange, int step, int life)
    {
        maxMoles = molesMax;
        maxMissed = missedMax;
        currentMole.GetComponent<Mole>().lifeTime = life;

        if (doOnce == step) //We have to do it only once per step/mode
        {
            doOnce++;
            totalMissedMolesSave += totalMissedMoles;
            redWhackedSave += redWhacked;
            totalMissedMoles = 0;
            redWhacked = 0;
        }
        return Random.Range(minRange, maxRange);
    }
}
