using UnityEngine;
using System.Collections;

public class GameBinds : MonoBehaviour {
	Global.Direction currentKeyDirection;
	
	bool isPressingUp;
	bool isPressingDown;
	bool isPressingLeft;
	bool isPressingRight;

	// Use this for initialization
	void Start () {
		Screen.showCursor = false;
		isPressingUp = false;
		isPressingDown = false;
		isPressingLeft = false;
		isPressingRight = false;	
		currentKeyDirection = Global.Direction.None;
	}
	
	// Update is called once per frame
	void Update () {
		if (!isPressingUp && Input.GetKey(KeyCode.UpArrow))
			currentKeyDirection = Global.Direction.Up;
		else if (!isPressingDown && Input.GetKey(KeyCode.DownArrow))
			currentKeyDirection = Global.Direction.Down;
		else if (!isPressingLeft && Input.GetKey(KeyCode.LeftArrow))
			currentKeyDirection = Global.Direction.Left;
		else if (!isPressingRight && Input.GetKey(KeyCode.RightArrow))
			currentKeyDirection = Global.Direction.Right;
		
		isPressingUp = Input.GetKey(KeyCode.UpArrow);
		isPressingDown = Input.GetKey(KeyCode.DownArrow);
		isPressingLeft = Input.GetKey(KeyCode.LeftArrow);
		isPressingRight = Input.GetKey(KeyCode.RightArrow);
		
		if ((currentKeyDirection == Global.Direction.Down && !isPressingDown) ||
			(currentKeyDirection == Global.Direction.Up && !isPressingUp) ||
			(currentKeyDirection == Global.Direction.Left && !isPressingLeft) ||
			(currentKeyDirection == Global.Direction.Right && !isPressingRight)){
			if (isPressingLeft)
				currentKeyDirection = Global.Direction.Left;
			else if (isPressingDown)
				currentKeyDirection = Global.Direction.Down;
			else if (isPressingUp)
				currentKeyDirection = Global.Direction.Up;
			else if (isPressingRight)
				currentKeyDirection = Global.Direction.Right;
		}
		
		if (!isPressingUp && !isPressingDown && !isPressingLeft && !isPressingRight)
			currentKeyDirection = Global.Direction.None;
		else if (Global.inGame)
			Global.rotatingCube.tryMove(currentKeyDirection);
		
		if (Global.inGame || Global.isPaused){
			if (Input.GetKeyDown(KeyCode.Escape)){
				if (Global.isPaused) {	
					Global.pauseScreen.removeFromScene();
					Global.inGame = true;
					Global.isPaused = false;
				}
				else {
					Global.pauseScreen.addToScene();
					Global.inGame = false;
					Global.isPaused = true;
				}
			}
		}
	}
	
	public Global.Direction getCurrentDirection(){
		return currentKeyDirection;
	}
}
