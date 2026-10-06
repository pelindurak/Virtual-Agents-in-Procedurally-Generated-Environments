using System;
using UnityEngine;
using UnityEngine.UI;

public class MapGeneration : MonoBehaviour
{
    [SerializeField] private bool ShouldUpdateUI; // if we want the editor to reflect the changes immediately

    [SerializeField] private int XSize, ZSize; // map's size
    [SerializeField] private int XOffset, ZOffset; // map's size
    [SerializeField] private float Scale; // used to "zoom in/out" the perlin noise

    [SerializeField] private Renderer TextureRenderer;

    [SerializeField] private float TerrainUpdateTimer = 1f;
    private float _Timer = 0f;

    public Terrain[] terrains;

    public float[,] Map { get; set; }


    public InputField XSize_input;
    public InputField ZSize_input;
    public InputField XOffset_input;
    public InputField ZOffset_input;

    public Slider scale_input;
    public Slider Timer_input;

    public Button button;

    void Start()
    {
        GenerateNoiseMap();
    }

    private void Update()
    {
        if (TerrainTimer())
        {
            _Timer = TerrainUpdateTimer;
            CalculateColours();
        }


    }

    public void GenerateNoiseMap()
    {
        CalculateHeightMap();
        CalculateColours();


        //XSize = int.Parse(XSize_input.text);
        //ZSize = int.Parse(ZSize_input.text);
        //XOffset = int.Parse(XOffset_input.text);
        //ZOffset = int.Parse(ZOffset_input.text);
        Scale = scale_input.value;
        TerrainUpdateTimer = Timer_input.value;
    }

    public void DepleteResources(float xLoc, float zLoc, float rate)
    {
        try
        {
            int xPos = 100 - (int)xLoc;
            int zPos = 100 - (int)zLoc;
            Debug.Log(xPos + " " + zPos);
            Map[zPos, xPos] -= rate;
        }
        catch (IndexOutOfRangeException e)
        {

        }
    }

    public (int x,int z, float val) GetMapResourceLocation(int xLoc, int zLoc, int awareness)
    {
        float maxRes = 0f;
        int _locX = xLoc, _locZ = zLoc;

        for (int i = xLoc - awareness; i <= xLoc + awareness; i++)
        {
            if (i < 0 || i >= XSize) continue;

            for (int j = zLoc - awareness; j <= zLoc + awareness; j++)
            {
                if (j < 0 || j >= ZSize) continue;

                if (maxRes < Map[i, j])
                {
                    maxRes = Map[i, j];
                    _locX = i;
                    _locZ = j;
                }
            }
        }

        return (x: _locX, z: _locZ, val: maxRes);
    }

    public bool ShouldUpdate()
    {
        return ShouldUpdateUI;
    }


    // the first step in proceduraly generating a map is to calculate the perlin noise
    // this method uses the map size and scale variables to calculate a 2D perlin noise matrix
    public float[,] CalculateHeightMap()
    {
       // if (Scale <= 1) Scale = 1.01f; // to avoid the division by 0 error
        if (XSize == 0) XSize = 1; 
        if (ZSize == 0) ZSize = 1; 

        Map = new float[XSize, ZSize]; // initialize a 2D array with the given sizes
        for (int i = 0; i < XSize; i++)
        {
            for (int j = 0; j < ZSize; j++)
            {
                Map[i, j] = Mathf.PerlinNoise(XOffset + i / Scale, ZOffset + j / Scale);
            }
        }
        return Map;
    }

    public void CalculateColours()
    {
        // get a 1D array of colors to later use for texturing
        Color32[] colors = new Color32[XSize * ZSize];
        for (int i = 0; i < XSize; i++)
        {
            for (int j = 0; j < ZSize; j++)
            {
                // linearly interpolate between colours by the perlin value given,
                // which is between 0 and 1

                float val = Map[i, j];

                colors[i * ZSize + j] = Color32.Lerp(Color.red, Color.blue, val);

                //for (int k = 0; k < terrains.Length; k++)
                //{
                //    if (val >= terrains[k].minHeight && val <= terrains[k].maxHeight)
                //    {
                //        colors[i * ZSize + j] = terrains[k].colour;
                //        break;
                //    }
                //}


            }
        }

        Texture2D texture = new Texture2D(XSize, ZSize);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels32(colors);
        texture.Apply();
        TextureRenderer.sharedMaterial.mainTexture = texture;
    }

    private bool TerrainTimer()
    {
        _Timer -= Time.deltaTime;
        return _Timer < 0;
    }

}

[@System.Serializable]
public struct Terrain
{
    public string name;
    public float minHeight, maxHeight;
    public Color colour;
}
