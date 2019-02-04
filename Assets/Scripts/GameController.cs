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
        Mole moleCollided = mole.gameObject.GetComponent<Mole>();
        if (moleCollided)
        {
            moleCollided.isActive = false;
        }
    }

	// This functiom is called when the user is pointing at a mole
    void IsPointingAtMole(Collider mole)
    {
        Mole moleCollided = mole.gameObject.GetComponent<Mole>();
        if (moleCollided)
        {
            moleCollided.glow();
        }
    }
}
