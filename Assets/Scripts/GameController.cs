using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public List<Mole> listMolesObjects;

	void Start () {
		PointingSystem.onPressTrigger += MoleWhackDetection;
		PointingSystem.isPointingAtMole += IsPointingAtMole;
	}
	
	void Update () {
		
	}

	// This function will be called everytime the user is pressing the trigger
	// If it hits a mole, then isAMole is true.
	void MoleWhackDetection(bool isAMole) {
		if(isAMole) {
			// Do what you want when the user is whacking a mole
		}
		else {
			//
		}
	}

	void IsPointingAtMole(bool isPointingAtMole) {
		if(isPointingAtMole) {
			// User is pointing at thte  mole with the controller
		}
	}
}
