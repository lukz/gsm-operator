using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerSet {
	[Range(0, 3)]
	public int sphereCount = 0;
	[Range(0, 3)]
	public int coneCount = 0;
	[Range(0, 3)]
	public int rayCount = 0;

	public int this[int i]
	{
		get { 
			switch(i) {
				case 0: return sphereCount;
				case 1: return coneCount;
				case 2: return rayCount;
			}
			throw new IndexOutOfRangeException("Invalid tower id " + i);
		 }
		set { 
			switch(i) {
				case 0: {
					sphereCount = value;
					return;
				}
				case 1: {
					coneCount = value;
					return;
				}
				case 2: {
					rayCount = value;
					return;
				}
			}
			throw new IndexOutOfRangeException("Invalid tower id " + i);
		}
	}
}
