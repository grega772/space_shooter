using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIElements : MonoBehaviour {

    [SerializeField] Text healthText;
    [SerializeField] Text scoreText;
    [SerializeField] GameObject lives;
    private int score = 0;



	void Start () {
		
	}
	

	void Update () {
		
	}


    public void setHealthText(int remainingHealth)
    {
        this.healthText.text = remainingHealth.ToString();
    }

    public void setScoreText(int score)
    {
        this.score += score;
        this.scoreText.text = "Score: " + this.score;
    }

    public void decrementLives() {
        foreach (var life in lives.GetComponentsInChildren<Image>())
        {
            Destroy(life.gameObject);
            break;
        }
    }

}
