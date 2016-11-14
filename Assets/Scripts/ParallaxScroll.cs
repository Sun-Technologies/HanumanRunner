using UnityEngine;

public class ParallaxScroll : MonoBehaviour
{

    public Renderer foreground;
    public Renderer midground;
    public Renderer midgroundMountains;

    public float backgroundSpeed = 0.02f;
    public float foregroundSpeed = 0.06f;
    public float midgroundSpeed = 0.04f;
    public float midgroundMountainsSpeed = 0.04f;

    public float offset = 0;

    void Update()
    {
        float backgroundOffset = offset * backgroundSpeed;
        float foregroundOffset = offset * foregroundSpeed;
        float midgroundOffset = offset * midgroundSpeed;
        float midgroundMountainsOffset = offset * midgroundMountainsSpeed;

        foreground.material.mainTextureOffset = new Vector2(foregroundOffset, 0);
        midground.material.mainTextureOffset = new Vector2(midgroundOffset, 0);
        midgroundMountains.material.mainTextureOffset = new Vector2(midgroundMountainsOffset, 0);

    }
}
