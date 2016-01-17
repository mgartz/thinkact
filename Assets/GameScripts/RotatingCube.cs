using UnityEngine;
using System.Collections;

public class RotatingCube : MonoBehaviour {
	int currentTileI;
	int currentTileJ;
	
	public float speed;
	
	bool isRotating;
	float rotationTimeElapsed;
	float rotationTimePeriod;
	Vector3 rotationPositionStart;
	Vector3 rotationPositionEnd;
	Quaternion rotationStart;
	Quaternion rotationEnd;
	
	bool isDropping;
	float droppingTimeElapsed;
	float droppingTimePeriod;
	Vector3 droppingPositionStart;
	Vector3 droppingPositionEnd;
	
	bool isLosingLife;
	float losingLifeTimeElapsed;
	float losingLifeTimePeriod = 0.2f;
	float losingLifeNumBlinks = 5;
	float losingLifeNumBlinksLeft;
	bool isBlinkOn;
	
	public Renderer rendererPlaneA;
	public Renderer rendererPlaneB;
	public Renderer rendererPlaneC;
	public Renderer rendererPlaneD;
	public Renderer rendererPlaneE;
	public Renderer rendererPlaneF;
	
	public Texture2D redTexture;
	public Texture2D greenTexture;
	public Texture2D blueTexture;
	
	Texture2D planeAOrigTexture;
	Texture2D planeBOrigTexture;
	Texture2D planeCOrigTexture;
	Texture2D planeDOrigTexture;
	Texture2D planeEOrigTexture;
	Texture2D planeFOrigTexture;
	
	int numLives;
	int numLivesStart = 5;
	
	// Use this for initialization
	void Start () {
		Global.rotatingCube = this;
		isRotating = false;
		isDropping = false;
		isLosingLife = false;
		rotationTimePeriod = 0.15f;
		droppingTimePeriod = 1;
		
		planeAOrigTexture = redTexture;
		planeCOrigTexture = redTexture;
		planeBOrigTexture = greenTexture;
		planeDOrigTexture = greenTexture;
		planeEOrigTexture = blueTexture;
		planeFOrigTexture = blueTexture;
	}
	public void initParams(){
		if (Global.startFromLeft)
			currentTileI = 1;
		else 
			currentTileI = Global.maxTileI-1;
		numLives = numLivesStart;
		currentTileJ = 1;
		transform.localPosition = Global.TilesOrigin + new Vector3(currentTileI*Global.TileWidth,currentTileJ*Global.TileHeight,0);
		transform.localRotation = Quaternion.Euler(0,0,0);
		
		rendererPlaneA.material.mainTexture = planeAOrigTexture;
		rendererPlaneB.material.mainTexture = planeBOrigTexture;
		rendererPlaneC.material.mainTexture = planeCOrigTexture;
		rendererPlaneD.material.mainTexture = planeDOrigTexture;
		rendererPlaneE.material.mainTexture = planeEOrigTexture;
		rendererPlaneF.material.mainTexture = planeFOrigTexture;
		
		isRotating = false;
		isDropping = false;
		isLosingLife = false;
		
		transform.localScale = new Vector3(0.95f,0.95f,0.95f);
	}
	
	// Update is called once per frame
	void Update () {
		if (isRotating){
			rotationTimeElapsed += Time.deltaTime;
			if (rotationTimeElapsed >= rotationTimePeriod){
				isRotating = false;
				rotationTimeElapsed = rotationTimePeriod;
				
				transform.localPosition = Vector3.Lerp(rotationPositionStart, rotationPositionEnd, rotationTimeElapsed/rotationTimePeriod);
				transform.localRotation = Quaternion.Lerp(rotationStart, rotationEnd, rotationTimeElapsed/rotationTimePeriod);
				this.checkIsMovementValid();
			}
			else {
				transform.localPosition = Vector3.Lerp(rotationPositionStart, rotationPositionEnd, rotationTimeElapsed/rotationTimePeriod);
				transform.localRotation = Quaternion.Lerp(rotationStart, rotationEnd, rotationTimeElapsed/rotationTimePeriod);
			}
		}
		else if (isDropping){
			droppingTimeElapsed += Time.deltaTime;
			if (droppingTimeElapsed >= droppingTimePeriod){
				isDropping = false;
				droppingTimeElapsed = droppingTimePeriod;
			}
			transform.localPosition = Vector3.Lerp(droppingPositionStart, droppingPositionEnd, droppingTimeElapsed/droppingTimePeriod);
		}
		
		if (isLosingLife){
			losingLifeTimeElapsed += Time.deltaTime;
			if (losingLifeTimeElapsed >= losingLifeTimePeriod){
				if (isBlinkOn)
					blinkOff();
				else
					blinkOn();
				losingLifeTimeElapsed -= losingLifeTimePeriod;
			}
				
		}
	}
	
	void move(Global.Direction direction){
		rotationPositionStart = transform.localPosition;
		rotationStart = transform.localRotation;
		
		Vector3 rotationAxis = Vector3.zero;
		
		if (direction == Global.Direction.Down){
			currentTileJ--;
			rotationAxis = Vector3.left;
		}
		else if (direction == Global.Direction.Up){
			currentTileJ++;
			rotationAxis = Vector3.right;
		}
		else if (direction == Global.Direction.Left){
			currentTileI--;
			rotationAxis = Vector3.up;
		}
		else if (direction == Global.Direction.Right){
			currentTileI++;
			rotationAxis = Vector3.down;
		}
		
		transform.RotateAround(rotationAxis, Mathf.PI/2);
		rotationEnd = transform.localRotation;
		
		Vector3 unfilteredEulerVector = transform.localRotation.eulerAngles;
		rotationEnd = Quaternion.Euler((int)unfilteredEulerVector.x,(int)unfilteredEulerVector.y,(int)unfilteredEulerVector.z);
		transform.localRotation = rotationStart;
		
		isRotating = true;
		rotationTimeElapsed = 0;
		rotationPositionEnd = Global.TilesOrigin + new Vector3(currentTileI*Global.TileWidth,currentTileJ*Global.TileHeight,0);
	}
	
	void checkIsMovementValid(){
		Tile.State tileState = Global.tilesPool.getTile(currentTileI, currentTileJ).getTileState();
		Tile.State cubeState = getState();

		if (!(tileState == Tile.State.Neutral || tileState == cubeState))
			this.loseLife();
		
		if (Global.tilesPool.getTile(currentTileI, currentTileJ).getTileBubble() != null){
			Global.bubblesPool.burstBubbleAtPos(currentTileI, currentTileJ);
			transform.localRotation = LevelBuilder.getRotation(currentTileI, currentTileJ);
		}

		Global.tilesPool.getTile (currentTileI, currentTileJ).stopBlinking ();
	}
	void loseLife(){
		if (!isLosingLife){
			blinkOff();
			isLosingLife = true;
			losingLifeTimeElapsed = 0;
			losingLifeNumBlinksLeft = losingLifeNumBlinks;
			
			numLives--;
			if (numLives <= 0)
				Global.game.endGame();
		}
	}
	
	void blinkOn(){
		isBlinkOn = true;
		losingLifeNumBlinksLeft--;
		if (losingLifeNumBlinksLeft == 0)
			isLosingLife = false;
		transform.localScale = new Vector3(0.95f,0.95f,0.95f);
	}
	void blinkOff(){
		isBlinkOn = false;
		transform.localScale = Vector3.zero;
	}
	
	public int getCurrentTileI(){
		return currentTileI;
	}
	public int getCurrentTileJ(){
		return currentTileJ;
	}
	
	public void tryMove(Global.Direction direction){
		if (!isRotating && !isDropping){
			if (direction == Global.Direction.Down && currentTileJ > 0)
				move(direction);
			else if (direction == Global.Direction.Left && currentTileI > TilesColumn.currentMinColumnIndex)
				move(direction);
			else if (direction == Global.Direction.Up && currentTileJ < Global.maxTileJ)
				move(direction);
			else if (direction == Global.Direction.Right && currentTileI < TilesColumn.currentMaxColumnIndex)
				move(direction);
		}
	}
	public void quickMove(Global.Direction direction){
		rotationStart = transform.localRotation;
		
		Vector3 rotationAxis = Vector3.zero;
		
		if (direction == Global.Direction.Down){
			currentTileJ--;
			rotationAxis = Vector3.left;
		}
		else if (direction == Global.Direction.Up){
			currentTileJ++;
			rotationAxis = Vector3.right;
		}
		else if (direction == Global.Direction.Left){
			currentTileI--;
			rotationAxis = Vector3.up;
		}
		else if (direction == Global.Direction.Right){
			currentTileI++;
			rotationAxis = Vector3.down;
		}
		
		transform.RotateAround(rotationAxis, Mathf.PI/2);
		rotationEnd = transform.localRotation;		
		Vector3 unfilteredEulerVector = transform.localRotation.eulerAngles;
		rotationEnd = Quaternion.Euler((int)unfilteredEulerVector.x,(int)unfilteredEulerVector.y,(int)unfilteredEulerVector.z);
		transform.localRotation = rotationEnd;
		rotationPositionEnd = Global.TilesOrigin + new Vector3(currentTileI*Global.TileWidth,currentTileJ*Global.TileHeight,0);
	}
	public void drop(){
		isDropping = true;
		isRotating = false;
		droppingTimeElapsed = 0;
		droppingPositionStart = transform.localPosition;
		droppingPositionEnd = droppingPositionStart + new Vector3(0,0,200);
		Global.game.endGame();
	}

	public Tile.State getState(){
		if (rendererPlaneA.transform.position.z > 4 || rendererPlaneC.transform.position.z > 4)
			return Tile.State.Red;
		else if (rendererPlaneB.transform.position.z > 4 || rendererPlaneD.transform.position.z > 4)
			return Tile.State.Green;
		else
			return Tile.State.Blue;
	}
	
	public int getHP(){
		return numLives;
	}
}