using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public List<Mole> listMolesObjects;

	void Start () {
		PointingSystem.onPressTrigger += MoleWhackDetection;
	}
	
	void Update () {
		
	}

	void MoleWhackDetection(bool isAMole) {
		if(isAMole) {
			// Do what you want when the user is whacking a mole
		}
	}
}
