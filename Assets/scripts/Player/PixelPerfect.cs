/*How to use:
1. Make the GO that contains the SpriteRenderer a child of another GameObject which has not SpriteRenderer (either could have things like Colliders and other scripts).
2. Put the PixelPerfect script onto the child GO that contains the SpriteRenderer
3. Open the script and set the pixelsPerUnit value to whatever you want (1, 16, ...) depending on your resolution. If each tile is equal to one unity unit, then you would set to 16. If each pixel is equal to one Unity unit, then set it to 1. The variable is not public so that you can change the value inside the script and each instance of the script will be affected without having to go over each instance manually.
4. The script has the attribute [ExecuteInEditMode] so you can drag aroung the parent object and see that the child object which has the SpriteRenderer and PixelPerfect script snaps to whole pixels.
5. Note that since the Start function is not called in Edit mode, is is instead called in LateUpdate, but only if the game is not running.
*/
 
// from https://answers.unity.com/questions/1304143/round-position.html

using UnityEngine;
using System.Collections;
using System;
 
[ExecuteInEditMode]
public class PixelPerfect : MonoBehaviour {
     
	private int pixelsPerUnit = 1;
	private float snapValue = 1;
     
	// Use this for initialization
	void Start () {
		snapValue = 1f / pixelsPerUnit;
	}
     
	// Update is called once per frame
	void LateUpdate () {
		if(Application.isPlaying == false) {
			snapValue = 1f / pixelsPerUnit;
		}
		Vector3 pos = transform.parent.position;
		pos = new Vector3((float)Math.Round(pos.x / snapValue) * snapValue, (float)Math.Round(pos.y / snapValue) * snapValue, (float)Math.Round(pos.z / snapValue) * snapValue);
		transform.position = pos;
	}
}