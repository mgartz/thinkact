using UnityEngine;
using System.Collections;

public class BubbleModel {
	public Bubble.Type bubbleType;
	
	public int tileI;
	public int tileJ;
	
	public int[] blinkingTilesI;
	public int[] blinkingTilesJ;
	
	int currentBlinkingTile;
	
	public BubbleModel(int tileI, int tileJ){
		this.tileI = tileI;
		this.tileJ = tileJ;
		blinkingTilesI = new int[10];
		blinkingTilesJ = new int[10];
		currentBlinkingTile = 0;
		bubbleType = Bubble.Type.None;
	}
	
	public void addBlinkingTile(int tileI, int tileJ){
		if (currentBlinkingTile < blinkingTilesI.Length){
			blinkingTilesI[currentBlinkingTile] = tileI;
			blinkingTilesJ[currentBlinkingTile++] = tileJ;
		}
	}
}

