using UnityEngine;
using System.Collections;

public class TilesPool : MonoBehaviour {
	TilesColumn[] columns;
	
	static float timeForDropLeft = 2f;
	static float timeForDropRight = 0.1f;
	
	// Use this for initialization
	void Start () {
		Global.tilesPool = this;

		columns = new TilesColumn[transform.childCount];
		
		int i=0;
		foreach (Transform child in transform){
			columns[i] = child.GetComponent<TilesColumn>();
			i++;
		}
	}
	public void initTiles(){
		transform.localPosition = Vector3.zero;
		for (int i=0; i<columns.Length; i++)
			columns[i].resetColumn();

		if (Global.startFromLeft)
			for (int i=0; i<columns.Length; i++){
				columns[i].setColumnIndex(i);
			}
		else
			for (int i=0; i<columns.Length; i++){
				columns[i].setColumnIndex(Global.maxTileI-i);
			}
	}
	
	// Update is called once per frame
	void Update () {
		TilesColumn.currentTimeForDrop = timeForDropLeft * 1.0f*(Global.maxTileI-Global.rotatingCube.getCurrentTileI())/Global.maxTileI + 
										timeForDropRight * 1.0f*(Global.rotatingCube.getCurrentTileI())/Global.maxTileI;
	}
	
	public void startCountdownForDropAtNextColumnIndex(){
		int index = TilesColumn.currentMaxColumnIndex;
		if (Global.startFromLeft)
			index = TilesColumn.currentMinColumnIndex;
		for(int i=0; i<columns.Length; i++){
			if (columns[i].getColumnIndex() == index){
				columns[i].startCountdownForDrop();
			}
		}
	}
	
	public Tile getTile(int tileI, int tileJ){
		for(int i=0; i<columns.Length; i++){
			if (columns[i].getColumnIndex() == tileI){
				return columns[i].getTile(tileJ);
			}
		}
		return null;
	}
}
