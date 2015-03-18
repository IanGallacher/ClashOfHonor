using UnityEngine;
using System.Collections;

public class NZZ_MouseLock : MonoBehaviour {
	void DidLockCursor() {
		guiTexture.enabled = false;
	}
	void DidUnlockCursor() {
		guiTexture.enabled = true;
	}
	void OnMouseDown() {
		Screen.lockCursor = true;
	}
	private bool wasLocked = false;
	void Update() {
		if (Input.GetKeyDown("escape"))
			Screen.lockCursor = false;
		
		if (!Screen.lockCursor && wasLocked) {
			wasLocked = false;
			DidUnlockCursor();
		} else
		if (Screen.lockCursor && !wasLocked) {
			wasLocked = true;
			DidLockCursor();
		}
	}
}