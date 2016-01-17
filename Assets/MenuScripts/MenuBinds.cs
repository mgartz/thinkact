using UnityEngine;
using System.Collections;

public class MenuBinds : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI(){
		if (!Global.inGame){
			if (Global.gameSummary.getIsOnScreen()){
				if (Event.current.type == EventType.keyDown){
					if (Event.current.keyCode == KeyCode.Return)
						Global.gameSummary.chooseOption();
					else if (Event.current.keyCode == KeyCode.LeftArrow)
						Global.gameSummary.moveSelectorLeft();
					else if (Event.current.keyCode == KeyCode.RightArrow)
						Global.gameSummary.moveSelectorRight();
					else if (Input.GetKeyDown(KeyCode.Escape)){
						Global.gameSummary.removeFromScene();
						Global.mainMenu.addToScene();
					}
				}				
			}
			if (Global.mainMenu.getIsOnScreen()){
				if (Event.current.type == EventType.keyDown){
					if (Event.current.keyCode == KeyCode.Return)
						Global.mainMenu.chooseOption();
					else if (Event.current.keyCode == KeyCode.LeftArrow)
						Global.mainMenu.moveSelectorLeft();
					else if (Event.current.keyCode == KeyCode.RightArrow)
						Global.mainMenu.moveSelectorRight();
				}				
			}
			if (Global.pauseScreen.getIsOnScreen()){
				if (Event.current.type == EventType.keyDown){
					if (Event.current.keyCode == KeyCode.Return)
						Global.pauseScreen.chooseOption();
					else if (Event.current.keyCode == KeyCode.LeftArrow)
						Global.pauseScreen.moveSelectorLeft();
					else if (Event.current.keyCode == KeyCode.RightArrow)
						Global.pauseScreen.moveSelectorRight();
				}	
			}
		}
	}
}
