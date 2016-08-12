﻿using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour {
	public enum CardColor{
		Red,
		Blue,
		Green,
		Yellow,
		Black,
		None
	};
	public enum CardFunction{
		zero,
		one,
		two,
		three,
		four,
		five,
		six,
		seven,
		eight,
		nine,
		Skip,
		Reverse,
		DrawTwo,
		Wild,
		WildDrawFour,
		None,
		free
	};
	public CardColor color;
	public CardFunction function;
	public int CardID;
	//public int number;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
