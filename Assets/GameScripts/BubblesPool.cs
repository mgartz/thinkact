using UnityEngine;
using System.Collections;

public class BubblesPool : MonoBehaviour{
	Bubble[] bubbles;
	int currentBubbleIndex;
	
	// Use this for initialization
	void Start (){
		Global.bubblesPool = this;
			
		bubbles = new Bubble[transform.childCount];
		
		int i=0;
		foreach (Transform child in transform){
			bubbles[i] = child.GetComponent<Bubble>();
			i++;
		}
		
		currentBubbleIndex = 0;
	}
	public void initBubbles(){
		for (int i=0; i<bubbles.Length; i++)
			bubbles[i].removeFromScene();
	}

	public Bubble getNextBubble(){
		Bubble bubble = bubbles[currentBubbleIndex];
		currentBubbleIndex = (currentBubbleIndex+1) % (bubbles.Length);
		return bubble;
	}
	public Bubble addBubbleToScene(BubbleModel model){
		Bubble bubble = getNextBubble();
		bubble.addToScene(model);
		return bubble;
	}
	public void burstBubbleAtPos(int tileX, int tileY){
		for (int i=0; i<bubbles.Length; i++){
			if (bubbles[i].isBubbleAtPos(tileX,tileY)){
				bubbles[i].burstBubble();
				break;
			}
		}
	}
}

