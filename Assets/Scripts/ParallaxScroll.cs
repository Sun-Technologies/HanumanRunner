using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Backgrounds
{
    public string name;
    public Renderer renderer;
    public float scrollSpeedX;
    public float scrollSpeedY;
}

public class ParallaxScroll : MonoBehaviour
{
    public List<Backgrounds> backgroundsList;
    [HideInInspector] public float offsetX;
    [HideInInspector] public float offsetY;

    void Update()
    {
        foreach (Backgrounds item in backgroundsList)
        {
            float bgOffsetX = offsetX * item.scrollSpeedX;
            float bgOffsetY = offsetY * item.scrollSpeedY;
            item.renderer.material.mainTextureOffset = new Vector2(bgOffsetX, bgOffsetY);
        }
    }
}
