using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Set of useful functions
public static class GlobalFunctions {
	// ------------------------------------------ Methods ------------------------------------------ //

	// Calls the passed in action after some amount of time.
	public static IEnumerator Invoke(Action func, float time) {
		yield return new WaitForSeconds(time);
		func();
	}
}
