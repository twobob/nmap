using System;
using UnityEngine;
using System.Collections;
using Assets.Map;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UI_GenNameMap : MonoBehaviour
{

    private InputField _inputName;
    private Button _btnGen;
	// Use this for initialization
    private Font _dFont;
    private RawImage _image;
    private GameObject _showMap;
	void Start ()
	{
        _inputName = transform.FindChild("inputName").GetComponent<InputField>();
        _btnGen = transform.FindChild("btnGen").GetComponent<Button>();
        _image = transform.FindChild("RawImage").GetComponent<RawImage>();
	    _dFont = _inputName.textComponent.font;

        _btnGen.onClick.AddListener(GenMap);

	    _showMap = GameObject.Find("Map");
	}

    private static Texture2D _txtTexture;
    const int TextureScale = 20;
    private const int Width = 100;
    private const int Height = 50;
    private void GenMap()
    {
        //Random.seed = 1;

        _txtTexture = GetTextTexture();

        Map.Width = Width;
        Map.Height = Height;
        Map map = new Map();
        map.SetPointNum(2500);
        map.Init(CheckIsland());
        //扰乱边缘
        NoisyEdges noisyEdge = new NoisyEdges();
        noisyEdge.BuildNoisyEdges(map);

        new MapTexture(TextureScale).AttachTexture(_showMap, map, noisyEdge);
    }
    public static System.Func<Vector2, bool> CheckIsland()
    {
        System.Func<Vector2, bool> inside = q =>
        {
            int x = Convert.ToInt32(q.x / Width * _txtWidth);
            int y = Convert.ToInt32(q.y / Height * _txtHeight);
            Color tColor = _txtTexture.GetPixel(x,y);
            return tColor == Color.white;
        };
        return inside;
    }

    private static int _txtWidth = 400;
    private static int _txtHeight = 200;
    private Texture2D GetTextTexture()
    {
        Texture2D output = new Texture2D(_txtWidth, _txtHeight);
        RenderTexture renderTexture = new RenderTexture(_txtWidth, _txtHeight, 24);
        RenderTexture.active = renderTexture;
        GameObject tempObject = new GameObject("Temporary");
        tempObject.transform.position = new Vector3(0.5f,0.5f,0);
        Camera myCamera = Camera.main;
        myCamera.orthographic = true;
        myCamera.orthographicSize = 100;
        myCamera.targetTexture = renderTexture;
        GUIText gText = tempObject.AddComponent<GUIText>();
        gText.text = _inputName.text;
        gText.anchor = TextAnchor.MiddleCenter;
        gText.alignment = TextAlignment.Center;
        gText.font = _dFont;
        gText.fontSize = 125;

        _showMap.SetActive(false);
        myCamera.Render();
        _showMap.SetActive(true);

        output.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        output.Apply();
        RenderTexture.active = null;

        //_image.texture = renderTexture;
        myCamera.targetTexture = null;
        myCamera.orthographic = false;
        myCamera.Render();

        Destroy(tempObject);

        return output;
    }
}
