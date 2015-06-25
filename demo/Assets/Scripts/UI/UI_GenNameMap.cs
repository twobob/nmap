using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_GenNameMap : MonoBehaviour
{

    private InputField _inputName;
    private Button _btnGen;
	// Use this for initialization
    private Font _dFont;
    private RawImage _image;
	void Start ()
	{
        _inputName = transform.FindChild("inputName").GetComponent<InputField>();
        _btnGen = transform.FindChild("btnGen").GetComponent<Button>();
        _image = transform.FindChild("RawImage").GetComponent<RawImage>();
	    _dFont = _inputName.textComponent.font;

	    _btnGen.onClick.AddListener(GenMap);
	}

    private void GenMap()
    {
        Texture2D output = new Texture2D(200, 200);
        RenderTexture renderTexture = new RenderTexture(200, 200, 24);
        RenderTexture.active = renderTexture;
        GameObject tempObject = new GameObject("Temporary");
        Camera myCamera = Camera.main;
        myCamera.orthographic = true;
        myCamera.orthographicSize = 100;
        myCamera.targetTexture = renderTexture;
        GUIText guiText = tempObject.AddComponent<GUIText>();
        guiText.text = _inputName.text;
        guiText.anchor = TextAnchor.LowerLeft;
        guiText.alignment = TextAlignment.Left;
        //guiText.lineSpacing = 1;
        //guiText.pixelOffset = new Vector2(50, 50);
        guiText.font = _dFont;
        guiText.fontSize = 50;
        myCamera.Render();
        output.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        output.Apply();
        RenderTexture.active = null;

        _image.texture = output;
        myCamera.targetTexture = null;
        myCamera.orthographic = false;
        myCamera.Render();

        //Destroy(tempObject);
    }
}
