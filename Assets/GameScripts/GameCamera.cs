using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		transform.localPosition = Global.CameraOffset + new Vector3(Global.rotatingCube.transform.localPosition.x,0,0);
	}
}
