using UnityEngine;
using System.Collections;

public class GameSummary : MonoBehaviour {
	public TextMesh winLoseText;
	public TextMesh percentageText;
	public TextMesh retryLeftText;
	public TextMesh retryRightText;
	
	public Vector3 offscreenPosition;
	public Vector3 onscreenPosition;
	
	public Transform selector;
	
	float movementTimeElapsed;
	float movementTimePeriod = 0.3f;
	
	bool isEnteringScreen;
	bool isLeavingScreen;
	bool isOnScreen;
	
	bool selectorIsLeft;
	
	// Use this for initialization
	void Start () {
		Global.gameSummary = this;
		isOnScreen = false;
	}
	
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
	}
	
	public void addToScene(){
		isOnScreen = true;
		isLeavingScreen = false;
		isEnteringScreen = true;
		movementTimeElapsed = 0;
		
		winLoseText.text = "Game Over";
		if (Global.startFromLeft)
			percentageText.text = "" + (100 * Global.rotatingCube.getCurrentTileI() / Global.maxTileI) + "%";
		else
			percentageText.text = "" + (100 * (Global.maxTileI - Global.rotatingCube.getCurrentTileI()) / Global.maxTileI) + "%";
		
		if (Global.startFromLeft)
			selector.localPosition = retryLeftText.transform.localPosition + new Vector3(0,0,0.1f);
		else
			selector.localPosition = retryRightText.transform.localPosition + new Vector3(0,0,0.1f);
		selectorIsLeft = Global.startFromLeft;
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
	
	public void moveSelectorLeft(){
		selectorIsLeft = true;
		selector.localPosition = retryLeftText.transform.localPosition + new Vector3(0,0,0.1f);
	}
	public void moveSelectorRight(){
		selectorIsLeft = false;
		selector.localPosition = retryRightText.transform.localPosition + new Vector3(0,0,0.1f);
	}
	public void chooseOption(){
		Global.game.startGame(selectorIsLeft);
		this.removeFromScene();
	}
}
