using System;
using UnityEngine;
using System.Collections;
using Assets.Map;
using Random = UnityEngine.Random;

public class UI_Main : MonoBehaviour
{
    private int MapSeed = 1;
    const int TextureScale = 20;
	// Use this for initialization
    void Start()
    {
    }

    #region Tutorial
    void GenMap()
    {
        Map map = new Map();
        //扰乱边缘
        NoisyEdges noisyEdge = new NoisyEdges();
        noisyEdge.BuildNoisyEdges(map);

        new MapTexture(TextureScale).AttachTexture(GameObject.Find("Map"), map, noisyEdge);
    }

    private void GenMap1()
    {
        Map1 map = new Map1();

        new MapTexture1(TextureScale).AttachTexture(GameObject.Find("Map"), map);
    }

    private void GenMap2()
    {
        Map1 map = new Map1(true);

        new MapTexture1(TextureScale).AttachTexture(GameObject.Find("Map"), map);
    }

    private void GenMap3()
    {
        Map1 map = new Map1(true);

        new MapTexture1(TextureScale).DrawTwoGraph(GameObject.Find("Map"), map);
    }

    private void GenMap4()
    {
        Map2 map = new Map2();

        new MapTexture2(TextureScale).AttachTexture(GameObject.Find("Map"), map);
    }

    private void GenMap5()
    {
        Map2 map = new Map2();

        new MapTexture2(TextureScale).ShowElevation(GameObject.Find("Map"), map);
    }
    #endregion

    #region UI
    public void ResetSeed()
    {
        MapSeed = (int)DateTime.Now.Ticks;
    }
    public void ClickGenMap(int index)
    {
        Random.seed = MapSeed;
        switch (index)
        {
            case 0:
                GenMap();
                break;
            case 1:
                GenMap1();
                break;
            case 2:
                GenMap2();
                break;
            case 3:
                GenMap3();
                break;
            case 4:
                GenMap4();
                break;
            case 5:
                GenMap5();
                break;
        }
        Debug.Log("阶段："+index);
    }

    #endregion
}
