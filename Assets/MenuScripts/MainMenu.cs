using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public Texture2D leftOptionUnselectedTexture;
	public Texture2D rightOptionUnselectedTexture;
	public Texture2D leftOptionSelectedTexture;
	public Texture2D rightOptionSelectedTexture;
	
	public TextMesh leftRecordText;
	public TextMesh rightRecordText;
	
	public Transform leftOption;
	public Transform rightOption;
	
	public Transform leftRecordPlaneBack;
	public Transform leftRecordPlaneFront;
	public Transform rightRecordPlaneBack;
	public Transform rightRecordPlaneFront;
	
	public Vector3 offscreenPosition;
	public Vector3 onscreenPosition;
	
	float movementTimeElapsed;
	float movementTimePeriod = 0.3f;
	
	bool isEnteringScreen;
	bool isLeavingScreen;
	bool isOnScreen;
	
	bool selectorIsLeft = false;
	bool selectorIsRight = false;
	
	// Use this for initialization
	void Start () {
		Global.mainMenu = this;
		isOnScreen = true;
		checkRecord();
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
		
		checkRecord();
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
		selectorIsRight = false;
		leftOption.renderer.material.mainTexture = leftOptionSelectedTexture;
		rightOption.renderer.material.mainTexture = rightOptionUnselectedTexture;
	}
	public void moveSelectorRight(){
		selectorIsLeft = false;
		selectorIsRight = true;
		leftOption.renderer.material.mainTexture = leftOptionUnselectedTexture;
		rightOption.renderer.material.mainTexture = rightOptionSelectedTexture;
	}
	public void chooseOption(){
		if (selectorIsLeft)
			Global.game.startGame(true);
		else if (selectorIsRight)
			Global.game.startGame(false);
		this.removeFromScene();
	}
	
	void checkRecord(){
		float leftRecordPlaneOffset = -4.7f;
		float rightRecordPlaneOffset = 4.7f;
		float recordPlaneScale = rightRecordPlaneOffset/5;
		
		int leftRecord = PlayerPrefs.GetInt("recordLeft");
		int rightRecord = PlayerPrefs.GetInt("recordRight");

		leftRecordText.text = leftRecord + "%";
		rightRecordText.text = rightRecord + "%";
		
		leftRecordPlaneFront.localScale = new Vector3(recordPlaneScale * leftRecord / 100, leftRecordPlaneFront.localScale.y, leftRecordPlaneFront.localScale.z);
		rightRecordPlaneFront.localScale = new Vector3(recordPlaneScale * rightRecord / 100, rightRecordPlaneFront.localScale.y, rightRecordPlaneFront.localScale.z);
		
		float diffScaleZ = leftRecordPlaneFront.localScale.z - 0.5f/0.6f*leftRecordPlaneFront.localScale.z;
		leftRecordPlaneBack.localScale = new Vector3(leftRecordPlaneFront.localScale.x-diffScaleZ,leftRecordPlaneFront.localScale.y-diffScaleZ,leftRecordPlaneFront.localScale.z-diffScaleZ);
		rightRecordPlaneBack.localScale = new Vector3(rightRecordPlaneFront.localScale.x-diffScaleZ,rightRecordPlaneFront.localScale.y-diffScaleZ,rightRecordPlaneFront.localScale.z-diffScaleZ);
		
		leftRecordPlaneFront.localPosition = new Vector3(leftRecordPlaneOffset+leftRecordPlaneFront.localScale.x*5.0f, leftRecordPlaneFront.localPosition.y, leftRecordPlaneFront.localPosition.z);
		rightRecordPlaneFront.localPosition = new Vector3(rightRecordPlaneOffset-rightRecordPlaneFront.localScale.x*5.0f, rightRecordPlaneFront.localPosition.y, rightRecordPlaneFront.localPosition.z);
		leftRecordPlaneBack.localPosition = leftRecordPlaneFront.localPosition + new Vector3(0,0,-0.01f);
		rightRecordPlaneBack.localPosition = rightRecordPlaneFront.localPosition + new Vector3(0,0,-0.01f);
	}
}