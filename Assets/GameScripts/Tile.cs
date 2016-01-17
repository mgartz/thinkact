using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	public enum State {Neutral, Red, Green, Blue};
	
	static int fillNeutralProbabilityLeft = 100;
	static int fillNeutralProbabilityRight = 0;
	static int skipColoredProbabilityLeft = 0;
	static int skipColoredProbabilityRight = 80;
	
	int tileI = 0;
	int tileJ = 0;
	public State state;
	
	bool isWaitingForDrop;
	float waitingForDropTimeElapsed;
	float waitingForDropTimePeriod;
	
	bool isWaitingForLand;
	float waitingForLandTimeElapsed;
	float waitingForLandTimePeriod;
	
	bool isDroppingTile = false;
	float droppingTileTimeElapsed;
	float droppingTileTimePeriod = 1;
	Vector3 droppingTilePositionStart;
	Vector3 droppingTilePositionEnd;
	
	bool isLandingTile = false;
	float landingTileTimeElapsed;
	float landingTileTimePeriod = 1;
	Vector3 landingTilePositionStart;
	Vector3 landingTilePositionEnd;
	
	bool isBlinking = false;
	public float blinkingTimeElapsed;
	float blinkingTimePeriod = 0.5f;

	Texture2D currentTexture;
	public Texture2D redTexture;
	public Texture2D greenTexture;
	public Texture2D blueTexture;
	public Texture2D grayTexture;
	
	TilesColumn parentTilesColumn;
	
	Bubble currentBubble;
	
	// Use this for initialization
	void Start () {
		parentTilesColumn = transform.parent.GetComponent<TilesColumn>();
	}
	public void resetTile(){
		isDroppingTile = false;
		isWaitingForDrop = false;
		isLandingTile = false;
		isWaitingForLand = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isWaitingForDrop){
			waitingForDropTimeElapsed += Time.deltaTime;
			if (waitingForDropTimeElapsed >= waitingForDropTimePeriod){
				isWaitingForDrop = false;
				this.dropTile();
			}
		}
		if (isWaitingForLand){
			waitingForLandTimeElapsed += Time.deltaTime;
			if (waitingForLandTimeElapsed >= waitingForLandTimePeriod){
				isWaitingForLand = false;
				this.landTile();
			}
		}
		if (isDroppingTile){
			droppingTileTimeElapsed += Time.deltaTime;
			if (droppingTileTimeElapsed >= droppingTileTimePeriod){
				isDroppingTile = false;
				droppingTileTimeElapsed = droppingTileTimePeriod;
				parentTilesColumn.tileFinishedDrop();
			}
			else
				transform.localPosition = Vector3.Lerp(droppingTilePositionStart, droppingTilePositionEnd, droppingTileTimeElapsed/droppingTileTimePeriod);
		}
		if (isLandingTile){
			landingTileTimeElapsed += Time.deltaTime;
			if (landingTileTimeElapsed >= landingTileTimePeriod){
				isLandingTile = false;
				landingTileTimeElapsed = landingTileTimePeriod;
			}
			transform.localPosition = Vector3.Lerp(landingTilePositionStart, landingTilePositionEnd, landingTileTimeElapsed/landingTileTimePeriod);
		}
		
		if (isBlinking){
			blinkingTimeElapsed += Time.deltaTime;
			renderer.material.color = Color.Lerp(Color.black, Color.white, (1+Mathf.Sin(Mathf.PI*2*blinkingTimeElapsed/blinkingTimePeriod))/2.0f);
		}
	}
	
	
	public Tile.State getTileState(){
		return state;
	}
	public Bubble getTileBubble(){
		return currentBubble;
	}
	
	public void setPositionI(int tileI){
		this.tileI = tileI;
		transform.localPosition = Global.TilesOrigin + new Vector3(tileI*Global.TileWidth,tileJ*Global.TileHeight,10);
		state = LevelBuilder.getState(tileI,tileJ);
		
		stopBlinking();
		if (LevelBuilder.getBubbleModel(tileI,tileJ).bubbleType != Bubble.Type.None){
			currentBubble = Global.bubblesPool.addBubbleToScene(LevelBuilder.getBubbleModel(tileI,tileJ));
		}
		else {
			currentBubble = null;
			int fillProbability = (int)(1.0f*fillNeutralProbabilityLeft * (Global.maxTileI-tileI)/Global.maxTileI + 
									1.0f*fillNeutralProbabilityRight * (tileI)/Global.maxTileI);
			int skipProbability = (int)(1.0f*skipColoredProbabilityLeft * (Global.maxTileI-tileI)/Global.maxTileI + 
									1.0f*skipColoredProbabilityRight * (tileI)/Global.maxTileI);
			if (state == Tile.State.Neutral){
				if (Random.Range(0,100) < fillProbability){
					if (Random.Range(0,3) == 0)
						state = Tile.State.Red;
					else if (Random.Range(0,2) == 0)
						state = Tile.State.Green;
					else
						state = Tile.State.Blue;
				}
			}
			else {
				if (Random.Range(0,100) < skipProbability)
					state = Tile.State.Neutral;
			}
		}
		if ((tileI < 3 && tileJ < 3) || (tileI > Global.maxTileI-2 && tileJ < 3))
			state = State.Neutral;
			
		if (state == State.Neutral)
			currentTexture = grayTexture;
		else if (state == State.Red)
			currentTexture = redTexture;
		else if (state == State.Green)
			currentTexture = greenTexture;
		else if (state == State.Blue)
			currentTexture = blueTexture;
		
		renderer.material.mainTexture = currentTexture;
	}
	public int getPositionI(){
		return tileI;
	}
	public void setPositionJ(int tileJ){
		this.tileJ = tileJ;
		transform.localPosition = Global.TilesOrigin + new Vector3(tileI*Global.TileWidth,tileJ*Global.TileHeight,30);
	}
	public int getPositionJ(){
		return tileJ;
	}
	
	public void triggerDropTile(){
		isWaitingForDrop = true;
		waitingForDropTimeElapsed = 0;
		waitingForDropTimePeriod = Random.Range(0.0f,0.5f);
	}
	public void triggerLandTile(){
		isWaitingForLand = true;
		waitingForLandTimeElapsed = 0;
		waitingForLandTimePeriod = Random.Range(0.0f,0.5f);
		transform.localPosition = transform.localPosition + new Vector3(0,0,-100);
	}
	
	public void startBlinking(float blinkingTimeElapsed){
		isBlinking = true;
		this.blinkingTimeElapsed = blinkingTimeElapsed;
	}
	public void stopBlinking(){
		isBlinking = false;
		renderer.material.color = Color.white;
	}
	
	void dropTile(){
		isDroppingTile = true;
		droppingTileTimeElapsed = 0;
		
		droppingTilePositionStart = transform.localPosition;
		droppingTilePositionEnd = droppingTilePositionStart + new Vector3(0,0,200);
		
		if (Global.rotatingCube.getCurrentTileI() == tileI &&
			Global.rotatingCube.getCurrentTileJ() == tileJ)
			Global.rotatingCube.drop();
	}
	void landTile(){
		isLandingTile = true;
		landingTileTimeElapsed = 0;
		landingTilePositionStart = transform.localPosition;
		landingTilePositionEnd = transform.localPosition - new Vector3(0,0,-100);
	}
}