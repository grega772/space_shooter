using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SOundObjectScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<AudioSource>().volume = 0.02f;	
	}
}
