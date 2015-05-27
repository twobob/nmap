using System;
using UnityEngine;
using System.Collections;
using Assets.Map;
using Random = UnityEngine.Random;

public class UI_Main : MonoBehaviour
{
    private int MapSeed = 1;
    const int TextureScale = 10;
	// Use this for initialization
    void Start()
    {
    }

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
        }
        Debug.Log("阶段："+index);
    }

    #endregion
}
