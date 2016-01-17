using UnityEngine;
using System.Collections;

public class TilesColumn : MonoBehaviour {
	public static int currentMaxColumnIndex = 0;
	public static int currentMinColumnIndex = 99999999;
	
	public static float currentTimeForDrop = 5f;
	
	bool isCountingDownForDrop = false;
	float countdownForDropTimeElapsed;
	float countdownForDropTimePeriod;
	
	Tile[] tiles;
	bool hasBeenInited=false;
	int columnIndex = 0;
	
	int numTilesDropped;
	bool areTilesDropped;
	
	// Use this for initialization
	void Start () {
		init ();
	}
	void init(){
		if (!hasBeenInited){
			tiles = new Tile[Global.maxTileJ+1];
			
			int i=0;
			foreach (Transform child in transform){
				tiles[i] = child.GetComponent<Tile>();
				tiles[i].setPositionJ(i);
				i++;
			}
			hasBeenInited = true;
		}
	}
	public void resetColumn(){
		currentMaxColumnIndex = 0;
		currentMinColumnIndex = 99999999;
		for (int i=0; i<tiles.Length; i++)
			tiles[i].resetTile();
		isCountingDownForDrop = false;
	}
	
	
	// Update is called once per frame
	void Update () {
		if (Global.rotatingCube.getCurrentTileI() - 5 > columnIndex && Global.startFromLeft || 
			Global.rotatingCube.getCurrentTileI() + 5 < columnIndex && !Global.startFromLeft){
			this.dropTiles();
		}
		if (isCountingDownForDrop){
			countdownForDropTimeElapsed += Time.deltaTime;
			if (countdownForDropTimeElapsed >= countdownForDropTimePeriod){
				this.dropTiles();
				isCountingDownForDrop = false;
			}
		}
	}
	
	public Tile getTile(int tileJ){
		return tiles[tileJ];
	}
	public Tile.State getTileState(int tileJ){
		return tiles[tileJ].getTileState();
	}
	public Bubble getTileBubble(int tileJ){
		return tiles[tileJ].getTileBubble();
	}
	
	public int getColumnIndex(){
		return columnIndex;
	}
	public void setColumnIndex(int columnIndex){
		init();
		this.columnIndex = columnIndex;
		
		if (columnIndex > currentMaxColumnIndex)
			currentMaxColumnIndex = columnIndex;
		if (columnIndex < currentMinColumnIndex)
			currentMinColumnIndex = columnIndex;
		for (int i=0; i<=Global.maxTileJ; i++){
			tiles[i].setPositionI(columnIndex);
		}
		
		areTilesDropped=false;
	}
	
	public void startCountdownForDrop(){
		if (!areTilesDropped){
			isCountingDownForDrop = true;
			countdownForDropTimeElapsed = 0;
			countdownForDropTimePeriod = currentTimeForDrop;
		}
		
	}
	public void dropTiles(){
		if (!areTilesDropped){
			isCountingDownForDrop = false;
			if (Global.startFromLeft && columnIndex+1 > currentMinColumnIndex)
				currentMinColumnIndex = columnIndex+1;
			else if (!Global.startFromLeft && columnIndex-1 < currentMaxColumnIndex)
				currentMaxColumnIndex = columnIndex-1;
			
			numTilesDropped = 0;
			for (int i=0; i<=Global.maxTileJ; i++)
				tiles[i].triggerDropTile();
			areTilesDropped = true;
			
			Global.tilesPool.startCountdownForDropAtNextColumnIndex();
		}
	}
	public void tileFinishedDrop(){
		numTilesDropped++;
		if (numTilesDropped == Global.maxTileJ+1){
			if (Global.startFromLeft && currentMaxColumnIndex+1 <= Global.maxTileI)
				this.setColumnIndex(currentMaxColumnIndex+1);
			else if (!Global.startFromLeft && currentMinColumnIndex-1 >= 0)
				this.setColumnIndex(currentMinColumnIndex-1);
			for (int i=0; i<=Global.maxTileJ; i++)
				tiles[i].triggerLandTile();
		}
	}
}
