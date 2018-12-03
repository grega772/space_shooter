using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    [SerializeField] AudioClip menuMusic;

    // Use this for initialization
    void Start () {
        gameObject.GetComponent<AudioSource>().PlayOneShot(menuMusic);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
