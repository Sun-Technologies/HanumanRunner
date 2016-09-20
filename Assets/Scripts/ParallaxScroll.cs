﻿using UnityEngine;
using System.Collections;

public class ParallaxScroll : MonoBehaviour {

	public Renderer background;
	public Renderer foreground;
    public Renderer midground;

    public float backgroundSpeed = 0.02f;
	public float foregroundSpeed = 0.06f;
    public float midgroundSpeed = 0.04f;

    public float offset = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float backgroundOffset = offset * backgroundSpeed;
		float foregroundOffset = offset * foregroundSpeed;
        float midgroundOffset = offset * midgroundSpeed;

        background.material.mainTextureOffset = new Vector2(backgroundOffset, 0);
		foreground.material.mainTextureOffset = new Vector2(foregroundOffset, 0);
        midground.material.mainTextureOffset = new Vector2(midgroundOffset, 0);

    }
}
