using UnityEngine;
using System.Collections;

public class GUI : MonoBehaviour {
	public TextMesh hpText;
	
	public Vector3 offscreenPosition;
	public Vector3 onscreenPosition;
	
	float movementTimeElapsed;
	float movementTimePeriod = 0.3f;
	
	bool isEnteringScreen;
	bool isLeavingScreen;
	bool isOnScreen;
	
	// Use this for initialization
	void Start () {
		Global.gui = this;
		hp = 0;
	}
	
	int hp;
	
	// Update is called once per frame
	void Update () {
		if (isEnteringScreen){
			movementTimeElapsed += Time.deltaTime;
			if (movementTimeElapsed >= movementTimePeriod){
				isEnteringScreen = false;
				movementTimeElapsed = movementTimePeriod;
			}
			transform.localPosition = Vector3.Lerp(offscreenPosition, onscreenPosition, movementTimeElapsed/movementTimePeriod);
		}
		else if (isLeavingScreen){
			movementTimeElapsed += Time.deltaTime;
			if (movementTimeElapsed >= movementTimePeriod){
				isLeavingScreen = false;
				movementTimeElapsed = movementTimePeriod;
			}
			transform.localPosition = Vector3.Lerp(onscreenPosition, offscreenPosition, movementTimeElapsed/movementTimePeriod);
		}
		if (Global.rotatingCube.getHP() != hp){
			hp = Global.rotatingCube.getHP();
			hpText.text = "HP " + hp;
		}
	}
	
	public void addToScene(){
		isOnScreen = true;
		isLeavingScreen = false;
		isEnteringScreen = true;
		movementTimeElapsed = 0;
	}
	public void removeFromScene(){
		isOnScreen = false;
		isLeavingScreen = true;
		isEnteringScreen = false;
		movementTimeElapsed = 0;
	}
	
	public bool getIsOnScreen(){
		return isOnScreen;
	}
}
