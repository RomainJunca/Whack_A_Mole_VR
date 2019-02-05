using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public List<Mole> listMolesObjects;
    private Material[] moleMaterial = new Material[2];


    void Start()
    {
        PointingSystem.onPressTrigger += MoleWhackDetection;
        PointingSystem.isPointingAtMole += IsPointingAtMole;
        PointingSystem.isExitingMole += IsExitingMole;
    }

    // This function will be called when the user is pressing the trigger on a mole only
    void MoleWhackDetection(Collider mole)
    {
        Mole moleCollided = mole.gameObject.GetComponent<Mole>();
        if (moleCollided)
        {
            moleCollided.isActive = false;
            moleCollided.addMaterials(null);
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

    // This function is called when the user is exiting the  mole with the controller pointer
    void IsExitingMole(Collider mole)
    {
        Mole moleCollided = mole.gameObject.GetComponent<Mole>();
        if (moleCollided)
        {
            if(moleCollided.isActive){
                moleCollided.addMaterials(moleCollided.currentColor);
            }
            else
            {
                moleCollided.addMaterials(null);
            }
        }    
    }
}
