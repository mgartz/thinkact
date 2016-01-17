using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
	public int recordLeft;
	public int recordRight;
	
	// Use this for initialization
	void Start () {
		Global.game = this;
		
		recordLeft = PlayerPrefs.GetInt("recordLeft",0);
		recordRight = PlayerPrefs.GetInt("recordRight",0);
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void startGame(bool startFromLeft){
		Global.inGame = true;
		Global.startFromLeft = startFromLeft;
		if (Global.startFromLeft)
			Global.CameraOffset = new Vector3(40,-50,-80);
		else
			Global.CameraOffset = new Vector3(-40,-50,-80);
		Global.bubblesPool.initBubbles();
		Global.rotatingCube.initParams();
		LevelBuilder.buildLevel(Global.startFromLeft);
		Global.tilesPool.initTiles();
		Global.gui.addToScene();
	}
	public void endGame(){
		if (Global.inGame){
			Global.inGame = false;
			Global.gameSummary.addToScene();
			Global.gui.removeFromScene();

			int record;
			if (Global.startFromLeft){
				record = (100 * Global.rotatingCube.getCurrentTileI() / Global.maxTileI);
				if (record > recordLeft){
					PlayerPrefs.SetInt("recordLeft", record);
					recordLeft = record;
				}
			}
			else{
				record = (100 * (Global.maxTileI - Global.rotatingCube.getCurrentTileI()) / Global.maxTileI);
				if (record > recordRight){
					PlayerPrefs.SetInt("recordRight", record);
					recordRight = record;
				}
			}
			
		}
	}
}
