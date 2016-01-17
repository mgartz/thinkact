using UnityEngine;
using System.Collections;

public class PauseScreen : MonoBehaviour {
	public TextMesh percentageText;
	public TextMesh retryLeftText;
	public TextMesh retryRightText;
	
	public Vector3 offscreenPosition;
	public Vector3 onscreenPosition;
	
	public Transform selector;
	
	bool isOnScreen;
	bool selectorIsLeft;
	
	// Use this for initialization
	void Start () {
		Global.pauseScreen = this;
		isOnScreen = false;
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void addToScene(){
		Time.timeScale = 0;
		isOnScreen = true;

		if (Global.startFromLeft)
			percentageText.text = "" + (100 * Global.rotatingCube.getCurrentTileI() / Global.maxTileI) + "%";
		else
			percentageText.text = "" + (100 * (Global.maxTileI - Global.rotatingCube.getCurrentTileI()) / Global.maxTileI) + "%";
		
		if (Global.startFromLeft)
			selector.localPosition = retryLeftText.transform.localPosition + new Vector3(0,0,0.1f);
		else
			selector.localPosition = retryRightText.transform.localPosition + new Vector3(0,0,0.1f);
		selectorIsLeft = Global.startFromLeft;
		transform.localPosition = onscreenPosition;
	}
	public void removeFromScene(){
		isOnScreen = false;
		transform.localPosition = offscreenPosition;
		Time.timeScale = 1;
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
