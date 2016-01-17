using UnityEngine;
using System.Collections;

public class Bubble : MonoBehaviour{
	
	public enum Type {Red, Green, Blue, None};
	
	BubbleModel model;
	Vector3 baseBubbleOffset = new Vector3(0,5,-11);
	Vector3 bubbleOffset;
	
	bool isIdle = false;
	bool isBursting = false;
	bool isShowingPath = false;
	float animationTimeElapsed;
	float animationTimePerFrame;
	int animationFrame;

	float showPathTimeElapsed;
	float showPathTimePerTile = 0.15f;
	int showPathTileIndex = 0;
	
	public Texture2D[] idleTextures;
	public Texture2D[] burstTextures;
	
	Tile mountedTile;
	
	
	// Use this for initialization
	void Start (){
		model = new BubbleModel(-1,0);
		bubbleOffset = baseBubbleOffset;
		renderer.enabled = true;
		mountedTile = null;
	}
	
	// Update is called once per frame
	void Update (){
		if (isIdle){
			animationTimeElapsed += Time.deltaTime;
			if (animationTimeElapsed > animationTimePerFrame){
				animationTimeElapsed -= animationTimePerFrame;
				animationFrame = (animationFrame + 1) % idleTextures.Length;
				renderer.material.mainTexture = idleTextures[animationFrame];
			}
		}
		else if (isBursting){
			animationTimeElapsed += Time.deltaTime;
			if (animationTimeElapsed > animationTimePerFrame){
				animationTimeElapsed -= animationTimePerFrame;
				animationFrame++;
				if (animationFrame == burstTextures.Length){
					isBursting = false;
					renderer.enabled = false;
				}
				else
					renderer.material.mainTexture = burstTextures[animationFrame];
			}
		}

		if (isShowingPath) {
			showPathTimeElapsed += Time.deltaTime;
			if (showPathTimeElapsed > showPathTimePerTile) {
				showPathTimeElapsed -= showPathTimePerTile;
				int i = model.blinkingTilesI[showPathTileIndex];
				int j = model.blinkingTilesJ[showPathTileIndex];
				float blinkingTimeElapsed = 0;
				if (showPathTileIndex > 0) {
					int lastI = model.blinkingTilesI[showPathTileIndex-1];
					int lastJ = model.blinkingTilesJ[showPathTileIndex-1];
					blinkingTimeElapsed = Global.tilesPool.getTile(lastI, lastJ).blinkingTimeElapsed;
				}
				Global.tilesPool.getTile(i,j).startBlinking(blinkingTimeElapsed);
				showPathTileIndex++;
				if (showPathTileIndex == model.blinkingTilesI.Length)
					isShowingPath = false;
			}
		}

		if (renderer.enabled && mountedTile != null && model.tileI == mountedTile.getPositionI() && model.tileJ == mountedTile.getPositionJ())
			transform.localPosition = mountedTile.transform.localPosition + bubbleOffset;
	}
	
	public void addToScene(BubbleModel model){
		mountedTile = Global.tilesPool.getTile(model.tileI, model.tileJ);
		
		renderer.enabled = true;
		bubbleOffset = baseBubbleOffset;
		this.setIdle();
		
		this.model = model;
	}
	public void removeFromScene(){
		renderer.enabled = false;
		isIdle = false;
		isBursting = false;
		isShowingPath = false;
	}
	
	void setIdle(){
		isIdle = true;
		isBursting = false;
		isShowingPath = false;
		animationTimeElapsed = 0;
		animationFrame = 0;
		animationTimePerFrame = 0.1f;
	}
	public void burstBubble(){
		if (isIdle){
			isIdle = false;
			isBursting = true;
			animationTimeElapsed = 0;
			animationFrame = 0;
			animationTimePerFrame = 0.06f;

			bubbleOffset = baseBubbleOffset + new Vector3(0,0,-10);
			showPath();
		}
	}
	public void showPath(){
		isShowingPath = true;
		showPathTimeElapsed = 0;
		showPathTileIndex = 0;
	}
	
	public Type getBubbleType(){
		return model.bubbleType;
	}
	
	public bool isBubbleAtPos(int tileI, int tileJ){
		return (model.tileI == tileI && model.tileJ == tileJ);
	}
}