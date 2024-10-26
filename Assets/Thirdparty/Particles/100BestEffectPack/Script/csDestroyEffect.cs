using System;
using UnityEngine;
using System.Collections;

public class csDestroyEffect : MonoBehaviour {
	//dummy script
	private int x;
	private void Start() {
		x = 5; 
	}

	private void OnDestroy() {
		x -= 5;
	}
}
