using UnityEngine;
using System.Collections;

public class Global {
	
	public enum Direction {Up, Down, Left, Right, None};
	
	public static bool inGame = false;
	public static bool isPaused = false;
	
	public static bool startFromLeft = false;
	
	public static float TileWidth = 10;
	public static float TileHeight = 10;
	
	public static int maxTileI = 500;
	public static int maxTileJ = 5;
	
	public static Vector3 TilesOrigin = new Vector3(-TileWidth*3,-maxTileJ*TileHeight/2.0f, 0);
	
	public static RotatingCube rotatingCube;
	public static TilesPool tilesPool;
	public static BubblesPool bubblesPool;
	
	public static Vector3 CameraOffset = Vector3.zero;
	
	public static GameSummary gameSummary;
	public static PauseScreen pauseScreen;
	public static GUI gui;
	public static MainMenu mainMenu;
	public static Game game;

}
