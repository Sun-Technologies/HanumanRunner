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
        //float backgroundOffset = offset * backgroundSpeed;
        //float foregroundOffset = offset * foregroundSpeed;
        //float midgroundOffset = offset * midgroundSpeed;
        //float midgroundMountainsOffset = offset * midgroundMountainsSpeed;

        //foreground.material.mainTextureOffset = new Vector2(foregroundOffset, 0);
        //midground.material.mainTextureOffset = new Vector2(midgroundOffset, 0);
        //midgroundMountains.material.mainTextureOffset = new Vector2(midgroundMountainsOffset, 0);
    }
}
