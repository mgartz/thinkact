using UnityEngine;
using System.Collections;

public class LevelBuilder{
	static int bubbleProbLeft = 50;
	static int bubbleProbRight = 0;
	
	static Tile.State[,] states;
	static BubbleModel[,] bubbles;
	static Quaternion[,] rotations;
	
	public static void buildLevel(bool startFromLeft){
		states = new Tile.State[Global.maxTileI+1,Global.maxTileJ+1];
		bubbles = new BubbleModel[Global.maxTileI+1,Global.maxTileJ+1];
		rotations = new Quaternion[Global.maxTileI+1,Global.maxTileJ+1];
		
		for (int i=0; i<=Global.maxTileI; i++)
			for (int j=0; j<=Global.maxTileJ; j++){
				states[i,j] = Tile.State.Neutral;
				bubbles[i,j] = new BubbleModel(i,j);
				rotations[i,j] = Quaternion.identity;
			}
		
		int currentTileI = 0;
		if (!startFromLeft)
			currentTileI = Global.maxTileI;
		int currentTileJ = 0;
		
		Global.rotatingCube.initParams();
		Global.Direction lastDirection = Global.Direction.Left;
		bool shouldAvoidLastDirection = false;
		int shouldAvoidLastDirectionCounter = 0;
		BubbleModel currentBubble = null;
		
		while ((currentTileI < Global.maxTileI && startFromLeft) ||
			   (currentTileI > 0 && !startFromLeft)){
			int probBubble = (int)(1.0f*bubbleProbLeft * (Global.maxTileI-currentTileI)/Global.maxTileI + 
									1.0f*bubbleProbRight * (currentTileI)/Global.maxTileI);
			if (shouldAvoidLastDirection){
				if (shouldAvoidLastDirectionCounter++ > 20)
					shouldAvoidLastDirection = false;
			}
			
			else if (currentTileI > 2 && currentTileI < Global.maxTileI-2 && Random.Range(0,100) < probBubble){
				currentBubble = bubbles[currentTileI, currentTileJ];
				if (Random.Range(0,3) == 0)
					currentBubble.bubbleType = Bubble.Type.Red;
				else if (Random.Range(0,2) == 0)
					currentBubble.bubbleType = Bubble.Type.Green;
				else
					currentBubble.bubbleType = Bubble.Type.Blue;
				shouldAvoidLastDirection = true;
				shouldAvoidLastDirectionCounter = 0;
				rotations[currentTileI, currentTileJ] = Global.rotatingCube.transform.localRotation;
				states[currentTileI, currentTileJ] = Tile.State.Neutral;
			}
			
			if (Random.Range(0,2) == 0){
				if (startFromLeft){
					currentTileI++;
					Global.rotatingCube.quickMove(Global.Direction.Right);
					lastDirection = Global.Direction.Right;
				}
				else {
					currentTileI--;
					Global.rotatingCube.quickMove(Global.Direction.Left);
					lastDirection = Global.Direction.Left;
				}
			}
			else {
				if (currentTileJ > 0 && Random.Range(0,2) == 0 && (!shouldAvoidLastDirection || lastDirection != Global.Direction.Up)){
					currentTileJ--;
					Global.rotatingCube.quickMove(Global.Direction.Down);
					lastDirection = Global.Direction.Down;
				}
				else if (currentTileJ < Global.maxTileJ && (!shouldAvoidLastDirection || lastDirection != Global.Direction.Down)){
					currentTileJ++;
					Global.rotatingCube.quickMove(Global.Direction.Up);
					lastDirection = Global.Direction.Up;
				}
				else{
					if (startFromLeft){
						currentTileI++;
						Global.rotatingCube.quickMove(Global.Direction.Right);
						lastDirection = Global.Direction.Right;
					}
					else{
						currentTileI--;
						Global.rotatingCube.quickMove(Global.Direction.Left);
						lastDirection = Global.Direction.Left;
					}
				}
			}
			
			states[currentTileI,currentTileJ] = Global.rotatingCube.getState();
			
			if (shouldAvoidLastDirection && currentBubble != null){
				currentBubble.addBlinkingTile(currentTileI,currentTileJ);
			}
			
		}
		Global.rotatingCube.initParams();
			
	}
	public static Tile.State getState(int tileI, int tileJ){
		return states[tileI,tileJ];
	}
	public static BubbleModel getBubbleModel(int tileI, int tileJ){
		return bubbles[tileI,tileJ];
	}
	public static Quaternion getRotation(int tileI, int tileJ){
		return rotations[tileI, tileJ];
	}
}

