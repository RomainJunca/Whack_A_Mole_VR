using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public List<Mole> listMolesObjects;

    void Start()
    {
        PointingSystem.onPressTrigger += MoleWhackDetection;
        PointingSystem.isPointingAtMole += IsPointingAtMole;
    }

    void Update()
    {

    }

    // This function will be called when the user is pressing the trigger on a mole only
    void MoleWhackDetection(Collider mole)
    {
        print(mole);
        // Here you get the whacked mole collider you can access the gameobject with
        //mole.gameObject
    }

	// This functiom is called when the user is pointing at a mole
    void IsPointingAtMole(Collider mole)
    {
        // Here you get the whacked mole collider you can access the gameobject with
        //mole.gameObject
        print(mole);
    }
}
