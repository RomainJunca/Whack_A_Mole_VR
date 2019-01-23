using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour {

    public GameObject cam;

	void Start () {
		
	}
	
	void Update () {

        gameObject.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y-1.5f, cam.transform.position.z + 1);

	}
}
