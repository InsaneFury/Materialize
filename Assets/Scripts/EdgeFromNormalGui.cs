﻿using System.Collections;
using System.ComponentModel;
using UnityEngine;

public class EdgeSettings
{
    [DefaultValue(1.0)] public float Blur0Contrast;

    [DefaultValue("1")] public string Blur0ContrastText;

    [DefaultValue(1.0f)] public float Blur0Weight;

    [DefaultValue(0.5f)] public float Blur1Weight;

    [DefaultValue(0.3f)] public float Blur2Weight;

    [DefaultValue(0.5f)] public float Blur3Weight;

    [DefaultValue(0.7f)] public float Blur4Weight;

    [DefaultValue(0.7f)] public float Blur5Weight;

    [DefaultValue(0.3f)] public float Blur6Weight;

    [DefaultValue(1.0f)] public float CreviceAmount;

    [DefaultValue("1")] public string CreviceAmountText;

    [DefaultValue(1.0f)] public float EdgeAmount;

    [DefaultValue("1")] public string EdgeAmountText;

    [DefaultValue(0.0f)] public float FinalBias;

    [DefaultValue("0")] public string FinalBiasText;

    [DefaultValue(1.0f)] public float FinalContrast;

    [DefaultValue("1")] public string FinalContrastText;

    [DefaultValue(1.0f)] public float Pillow;

    [DefaultValue("1")] public string PillowText;

    [DefaultValue(1.0f)] public float Pinch;

    [DefaultValue("1")] public string PinchText;

    public EdgeSettings()
    {
        Blur0Contrast = 1.0f;
        Blur0ContrastText = "1";

        Blur0Weight = 1.0f;
        Blur1Weight = 0.5f;
        Blur2Weight = 0.3f;
        Blur3Weight = 0.5f;
        Blur4Weight = 0.7f;
        Blur5Weight = 0.7f;
        Blur6Weight = 0.3f;

        FinalContrast = 1.0f;
        FinalContrastText = "1";

        FinalBias = 0.0f;
        FinalBiasText = "0";

        EdgeAmount = 1.0f;
        EdgeAmountText = "1";

        CreviceAmount = 1.0f;
        CreviceAmountText = "1";

        Pinch = 1.0f;
        PinchText = "1";

        Pillow = 1.0f;
        PillowText = "1";
    }
}

public class EdgeFromNormalGui : MonoBehaviour
{
    private RenderTexture _BlurMap0;
    private RenderTexture _BlurMap1;
    private RenderTexture _BlurMap2;
    private RenderTexture _BlurMap3;
    private RenderTexture _BlurMap4;
    private RenderTexture _BlurMap5;
    private RenderTexture _BlurMap6;

    private Texture2D _EdgeMap;
    private Texture2D _NormalMap;
    private RenderTexture _TempBlurMap;
    private RenderTexture _TempEdgeMap;
    private Material blitMaterial;

    public bool busy;
    private bool doStuff;

    private EdgeSettings ES;

    private int imageSizeX = 1024;
    private int imageSizeY = 1024;

    public MainGui MainGuiScript;
    private bool newTexture;

    private bool settingsInitialized;

    public GameObject testObject;

    public Material thisMaterial;

    private Rect windowRect = new Rect(30, 300, 300, 600);

    public void GetValues(ProjectObject projectObject)
    {
        InitializeSettings();
        projectObject.EdgeSettings = ES;
    }

    public void SetValues(ProjectObject projectObject)
    {
        InitializeSettings();
        if (projectObject.EdgeSettings != null)
        {
            ES = projectObject.EdgeSettings;
        }
        else
        {
            settingsInitialized = false;
            InitializeSettings();
        }

        doStuff = true;
    }

    private void InitializeSettings()
    {
        if (settingsInitialized == false)
        {
            Debug.Log("Initializing Edge Settings");
            ES = new EdgeSettings();
            settingsInitialized = true;
        }
    }

    // Use this for initialization
    private void Start()
    {
        testObject.GetComponent<Renderer>().sharedMaterial = thisMaterial;

        blitMaterial = new Material(Shader.Find("Hidden/Blit_Shader"));

        InitializeSettings();
    }

    public void DoStuff()
    {
        doStuff = true;
    }

    public void NewTexture()
    {
        newTexture = true;
    }


    // Update is called once per frame
    private void Update()
    {
        if (newTexture)
        {
            InitializeTextures();
            newTexture = false;
        }

        if (doStuff)
        {
            StartCoroutine(ProcessNormal());
            doStuff = false;
        }

        thisMaterial.SetFloat("_Blur0Weight", ES.Blur0Weight * ES.Blur0Weight * ES.Blur0Weight);
        thisMaterial.SetFloat("_Blur1Weight", ES.Blur1Weight * ES.Blur1Weight * ES.Blur1Weight);
        thisMaterial.SetFloat("_Blur2Weight", ES.Blur2Weight * ES.Blur2Weight * ES.Blur2Weight);
        thisMaterial.SetFloat("_Blur3Weight", ES.Blur3Weight * ES.Blur3Weight * ES.Blur3Weight);
        thisMaterial.SetFloat("_Blur4Weight", ES.Blur4Weight * ES.Blur4Weight * ES.Blur4Weight);
        thisMaterial.SetFloat("_Blur5Weight", ES.Blur5Weight * ES.Blur5Weight * ES.Blur5Weight);
        thisMaterial.SetFloat("_Blur6Weight", ES.Blur6Weight * ES.Blur6Weight * ES.Blur6Weight);
        thisMaterial.SetFloat("_EdgeAmount", ES.EdgeAmount);
        thisMaterial.SetFloat("_CreviceAmount", ES.CreviceAmount);
        thisMaterial.SetFloat("_Pinch", ES.Pinch);
        thisMaterial.SetFloat("_Pillow", ES.Pillow);
        thisMaterial.SetFloat("_FinalContrast", ES.FinalContrast);
        thisMaterial.SetFloat("_FinalBias", ES.FinalBias);
    }

    private void SetDefaultSliderValues()
    {
        ES.Blur0Contrast = 1.0f;
        ES.Blur0ContrastText = "1";

        ES.EdgeAmount = 1.0f;
        ES.EdgeAmountText = "1";

        ES.CreviceAmount = 1.0f;
        ES.CreviceAmountText = "1";

        ES.Pinch = 1.0f;
        ES.PinchText = "1";

        ES.Pillow = 1.0f;
        ES.PillowText = "1";

        ES.FinalContrast = 2.0f;
        ES.FinalContrastText = "2";

        ES.FinalBias = 0.0f;
        ES.FinalBiasText = "0";
    }

    private void SetWeightEQDefault()
    {
        ES.Blur0Weight = 1.0f;
        ES.Blur1Weight = 0.5f;
        ES.Blur2Weight = 0.3f;
        ES.Blur3Weight = 0.5f;
        ES.Blur4Weight = 0.7f;
        ES.Blur5Weight = 0.7f;
        ES.Blur6Weight = 0.3f;

        SetDefaultSliderValues();

        doStuff = true;
    }

    private void SetWeightEQDisplace()
    {
        ES.Blur0Weight = 0.1f;
        ES.Blur1Weight = 0.15f;
        ES.Blur2Weight = 0.25f;
        ES.Blur3Weight = 0.45f;
        ES.Blur4Weight = 0.75f;
        ES.Blur5Weight = 0.95f;
        ES.Blur6Weight = 1.0f;

        SetDefaultSliderValues();

        ES.Blur0Contrast = 3.0f;
        ES.Blur0ContrastText = "3";

        ES.FinalContrast = 5.0f;
        ES.FinalContrastText = "5";

        ES.FinalBias = -0.2f;
        ES.FinalBiasText = "-0.2";

        doStuff = true;
    }

    private void SetWeightEQSoft()
    {
        ES.Blur0Weight = 0.15f;
        ES.Blur1Weight = 0.4f;
        ES.Blur2Weight = 0.7f;
        ES.Blur3Weight = 0.9f;
        ES.Blur4Weight = 1.0f;
        ES.Blur5Weight = 0.9f;
        ES.Blur6Weight = 0.7f;

        SetDefaultSliderValues();

        ES.FinalContrast = 4.0f;
        ES.FinalContrastText = "4";

        doStuff = true;
    }

    private void SetWeightEQTight()
    {
        ES.Blur0Weight = 1.0f;
        ES.Blur1Weight = 0.45f;
        ES.Blur2Weight = 0.25f;
        ES.Blur3Weight = 0.18f;
        ES.Blur4Weight = 0.15f;
        ES.Blur5Weight = 0.13f;
        ES.Blur6Weight = 0.1f;

        SetDefaultSliderValues();

        ES.Pinch = 1.5f;
        ES.PinchText = "1.5";

        doStuff = true;
    }

    private void DoMyWindow(int windowID)
    {
        var spacingX = 0;
        var spacingY = 50;

        var offsetX = 10;
        var offsetY = 30;

        doStuff = GuiHelper.Slider(new Rect(offsetX, offsetY, 280, 50), "Pre Contrast", ES.Blur0Contrast,
            ES.Blur0ContrastText, out ES.Blur0Contrast, out ES.Blur0ContrastText, 0.0f, 5.0f);
        offsetY += 50;

        GUI.Label(new Rect(offsetX, offsetY, 250, 30), "Frequency Equalizer");
        GUI.Label(new Rect(offsetX + 225, offsetY, 100, 30), "Presets");
        if (GUI.Button(new Rect(offsetX + 215, offsetY + 30, 60, 20), "Default")) SetWeightEQDefault();
        if (GUI.Button(new Rect(offsetX + 215, offsetY + 60, 60, 20), "Displace")) SetWeightEQDisplace();
        if (GUI.Button(new Rect(offsetX + 215, offsetY + 90, 60, 20), "Soft")) SetWeightEQSoft();
        if (GUI.Button(new Rect(offsetX + 215, offsetY + 120, 60, 20), "Tight")) SetWeightEQTight();
        offsetY += 30;
        offsetX += 10;
        ES.Blur0Weight = GUI.VerticalSlider(new Rect(offsetX + 180, offsetY, 10, 100), ES.Blur0Weight, 1.0f, 0.0f);
        ES.Blur1Weight = GUI.VerticalSlider(new Rect(offsetX + 150, offsetY, 10, 100), ES.Blur1Weight, 1.0f, 0.0f);
        ES.Blur2Weight = GUI.VerticalSlider(new Rect(offsetX + 120, offsetY, 10, 100), ES.Blur2Weight, 1.0f, 0.0f);
        ES.Blur3Weight = GUI.VerticalSlider(new Rect(offsetX + 90, offsetY, 10, 100), ES.Blur3Weight, 1.0f, 0.0f);
        ES.Blur4Weight = GUI.VerticalSlider(new Rect(offsetX + 60, offsetY, 10, 100), ES.Blur4Weight, 1.0f, 0.0f);
        ES.Blur5Weight = GUI.VerticalSlider(new Rect(offsetX + 30, offsetY, 10, 100), ES.Blur5Weight, 1.0f, 0.0f);
        ES.Blur6Weight = GUI.VerticalSlider(new Rect(offsetX + 0, offsetY, 10, 100), ES.Blur6Weight, 1.0f, 0.0f);
        offsetX -= 10;
        offsetY += 120;

        GuiHelper.Slider(new Rect(offsetX, offsetY, 280, 50), "Edge Amount", ES.EdgeAmount, ES.EdgeAmountText,
            out ES.EdgeAmount, out ES.EdgeAmountText, 0.0f, 1.0f);
        offsetY += 40;

        GuiHelper.Slider(new Rect(offsetX, offsetY, 280, 50), "Crevice Amount", ES.CreviceAmount, ES.CreviceAmountText,
            out ES.CreviceAmount, out ES.CreviceAmountText, 0.0f, 1.0f);
        offsetY += 40;

        GuiHelper.Slider(new Rect(offsetX, offsetY, 280, 50), "Pinch", ES.Pinch, ES.PinchText, out ES.Pinch,
            out ES.PinchText, 0.1f, 10.0f);
        offsetY += 40;

        GuiHelper.Slider(new Rect(offsetX, offsetY, 280, 50), "Pillow", ES.Pillow, ES.PillowText, out ES.Pillow,
            out ES.PillowText, 0.1f, 5.0f);
        offsetY += 40;

        GuiHelper.Slider(new Rect(offsetX, offsetY, 280, 50), "Final Contrast", ES.FinalContrast, ES.FinalContrastText,
            out ES.FinalContrast, out ES.FinalContrastText, 0.1f, 30.0f);
        offsetY += 40;

        GuiHelper.Slider(new Rect(offsetX, offsetY, 280, 50), "Final Bias", ES.FinalBias, ES.FinalBiasText,
            out ES.FinalBias, out ES.FinalBiasText, -1.0f, 1.0f);
        offsetY += 50;


        if (GUI.Button(new Rect(offsetX + 150, offsetY, 130, 30), "Set as Edge Map")) StartCoroutine(ProcessEdge());

        GUI.DragWindow();
    }

    private void OnGUI()
    {
        windowRect.width = 300;
        windowRect.height = 520;

        windowRect = GUI.Window(11, windowRect, DoMyWindow, "Edge from Normal");
    }

    public void InitializeTextures()
    {
        testObject.GetComponent<Renderer>().sharedMaterial = thisMaterial;

        CleanupTextures();

        _NormalMap = MainGuiScript.NormalMap;

        imageSizeX = _NormalMap.width;
        imageSizeY = _NormalMap.height;

        Debug.Log("Initializing Textures of size: " + imageSizeX + "x" + imageSizeY);

        _TempBlurMap = new RenderTexture(imageSizeX, imageSizeY, 0, RenderTextureFormat.RHalf,
            RenderTextureReadWrite.Linear);
        _TempBlurMap.wrapMode = TextureWrapMode.Repeat;
        _BlurMap0 = new RenderTexture(imageSizeX, imageSizeY, 0, RenderTextureFormat.RHalf,
            RenderTextureReadWrite.Linear);
        _BlurMap0.wrapMode = TextureWrapMode.Repeat;
        _BlurMap1 = new RenderTexture(imageSizeX, imageSizeY, 0, RenderTextureFormat.RHalf,
            RenderTextureReadWrite.Linear);
        _BlurMap1.wrapMode = TextureWrapMode.Repeat;
        _BlurMap2 = new RenderTexture(imageSizeX, imageSizeY, 0, RenderTextureFormat.RHalf,
            RenderTextureReadWrite.Linear);
        _BlurMap2.wrapMode = TextureWrapMode.Repeat;
        _BlurMap3 = new RenderTexture(imageSizeX, imageSizeY, 0, RenderTextureFormat.RHalf,
            RenderTextureReadWrite.Linear);
        _BlurMap3.wrapMode = TextureWrapMode.Repeat;
        _BlurMap4 = new RenderTexture(imageSizeX, imageSizeY, 0, RenderTextureFormat.RHalf,
            RenderTextureReadWrite.Linear);
        _BlurMap4.wrapMode = TextureWrapMode.Repeat;
        _BlurMap5 = new RenderTexture(imageSizeX, imageSizeY, 0, RenderTextureFormat.RHalf,
            RenderTextureReadWrite.Linear);
        _BlurMap5.wrapMode = TextureWrapMode.Repeat;
        _BlurMap6 = new RenderTexture(imageSizeX, imageSizeY, 0, RenderTextureFormat.RHalf,
            RenderTextureReadWrite.Linear);
        _BlurMap6.wrapMode = TextureWrapMode.Repeat;
    }

    public void Close()
    {
        CleanupTextures();
        gameObject.SetActive(false);
    }

    private void CleanupTexture(RenderTexture _Texture)
    {
        if (_Texture != null)
        {
            _Texture.Release();
            _Texture = null;
        }
    }

    private void CleanupTextures()
    {
        Debug.Log("Cleaning Up Textures");

        CleanupTexture(_TempBlurMap);
        CleanupTexture(_BlurMap0);
        CleanupTexture(_BlurMap1);
        CleanupTexture(_BlurMap2);
        CleanupTexture(_BlurMap3);
        CleanupTexture(_BlurMap4);
        CleanupTexture(_BlurMap5);
        CleanupTexture(_BlurMap6);
        CleanupTexture(_TempEdgeMap);
    }

    public IEnumerator ProcessEdge()
    {
        busy = true;

        Debug.Log("Processing Height / Edge");

        blitMaterial.SetVector("_ImageSize", new Vector4(imageSizeX, imageSizeY, 0, 0));

        blitMaterial.SetFloat("_Blur0Weight", ES.Blur0Weight * ES.Blur0Weight * ES.Blur0Weight);
        blitMaterial.SetFloat("_Blur1Weight", ES.Blur1Weight * ES.Blur1Weight * ES.Blur1Weight);
        blitMaterial.SetFloat("_Blur2Weight", ES.Blur2Weight * ES.Blur2Weight * ES.Blur2Weight);
        blitMaterial.SetFloat("_Blur3Weight", ES.Blur3Weight * ES.Blur3Weight * ES.Blur3Weight);
        blitMaterial.SetFloat("_Blur4Weight", ES.Blur4Weight * ES.Blur4Weight * ES.Blur4Weight);
        blitMaterial.SetFloat("_Blur5Weight", ES.Blur5Weight * ES.Blur5Weight * ES.Blur5Weight);
        blitMaterial.SetFloat("_Blur6Weight", ES.Blur6Weight * ES.Blur6Weight * ES.Blur6Weight);
        blitMaterial.SetFloat("_EdgeAmount", ES.EdgeAmount);
        blitMaterial.SetFloat("_CreviceAmount", ES.CreviceAmount);
        blitMaterial.SetFloat("_Pinch", ES.Pinch);
        blitMaterial.SetFloat("_Pillow", ES.Pillow);
        blitMaterial.SetFloat("_FinalContrast", ES.FinalContrast);
        blitMaterial.SetFloat("_FinalBias", ES.FinalBias);

        blitMaterial.SetTexture("_MainTex", _NormalMap);
        blitMaterial.SetTexture("_BlurTex0", _BlurMap0);
        blitMaterial.SetTexture("_BlurTex1", _BlurMap1);
        blitMaterial.SetTexture("_BlurTex2", _BlurMap2);
        blitMaterial.SetTexture("_BlurTex3", _BlurMap3);
        blitMaterial.SetTexture("_BlurTex4", _BlurMap4);
        blitMaterial.SetTexture("_BlurTex5", _BlurMap5);
        blitMaterial.SetTexture("_BlurTex6", _BlurMap6);

        // Save low fidelity for texture 2d

        CleanupTexture(_TempEdgeMap);
        _TempEdgeMap = new RenderTexture(imageSizeX, imageSizeY, 0, RenderTextureFormat.ARGB32,
            RenderTextureReadWrite.Linear);
        _TempEdgeMap.wrapMode = TextureWrapMode.Repeat;
        Graphics.Blit(_BlurMap0, _TempEdgeMap, blitMaterial, 6);

        RenderTexture.active = _TempEdgeMap;

        if (MainGuiScript.EdgeMap != null) Destroy(MainGuiScript.EdgeMap);

        MainGuiScript.EdgeMap =
            new Texture2D(_TempEdgeMap.width, _TempEdgeMap.height, TextureFormat.ARGB32, true, true);
        MainGuiScript.EdgeMap.ReadPixels(new Rect(0, 0, _TempEdgeMap.width, _TempEdgeMap.height), 0, 0);
        MainGuiScript.EdgeMap.Apply();

        yield return new WaitForSeconds(0.1f);

        CleanupTexture(_TempEdgeMap);

        busy = false;
    }

    public IEnumerator ProcessNormal()
    {
        busy = true;

        Debug.Log("Processing Normal to Edge");

        blitMaterial.SetVector("_ImageSize", new Vector4(imageSizeX, imageSizeY, 0, 0));

        // Make normal from height
        blitMaterial.SetFloat("_BlurContrast", ES.Blur0Contrast);
        blitMaterial.SetTexture("_MainTex", _NormalMap);
        Graphics.Blit(_NormalMap, _BlurMap0, blitMaterial, 5);

        blitMaterial.SetFloat("_BlurContrast", 1.0f);

        // Blur the image 1
        blitMaterial.SetTexture("_MainTex", _BlurMap0);
        blitMaterial.SetInt("_BlurSamples", 4);
        blitMaterial.SetFloat("_BlurSpread", 1.0f);
        blitMaterial.SetVector("_BlurDirection", new Vector4(1, 0, 0, 0));
        Graphics.Blit(_BlurMap0, _TempBlurMap, blitMaterial, 1);

        blitMaterial.SetTexture("_MainTex", _TempBlurMap);
        blitMaterial.SetVector("_BlurDirection", new Vector4(0, 1, 0, 0));
        Graphics.Blit(_TempBlurMap, _BlurMap1, blitMaterial, 1);


        // Blur the image 2
        blitMaterial.SetTexture("_MainTex", _BlurMap1);
        blitMaterial.SetFloat("_BlurSpread", 2.0f);
        blitMaterial.SetVector("_BlurDirection", new Vector4(1, 0, 0, 0));
        Graphics.Blit(_BlurMap1, _TempBlurMap, blitMaterial, 1);

        blitMaterial.SetTexture("_MainTex", _TempBlurMap);
        blitMaterial.SetVector("_BlurDirection", new Vector4(0, 1, 0, 0));
        Graphics.Blit(_TempBlurMap, _BlurMap2, blitMaterial, 1);


        // Blur the image 3
        blitMaterial.SetTexture("_MainTex", _BlurMap2);
        blitMaterial.SetFloat("_BlurSpread", 4.0f);
        blitMaterial.SetVector("_BlurDirection", new Vector4(1, 0, 0, 0));
        Graphics.Blit(_BlurMap2, _TempBlurMap, blitMaterial, 1);

        blitMaterial.SetTexture("_MainTex", _TempBlurMap);
        blitMaterial.SetVector("_BlurDirection", new Vector4(0, 1, 0, 0));
        Graphics.Blit(_TempBlurMap, _BlurMap3, blitMaterial, 1);


        // Blur the image 4
        blitMaterial.SetTexture("_MainTex", _BlurMap3);
        blitMaterial.SetFloat("_BlurSpread", 8.0f);
        blitMaterial.SetVector("_BlurDirection", new Vector4(1, 0, 0, 0));
        Graphics.Blit(_BlurMap3, _TempBlurMap, blitMaterial, 1);

        blitMaterial.SetTexture("_MainTex", _TempBlurMap);
        blitMaterial.SetVector("_BlurDirection", new Vector4(0, 1, 0, 0));
        Graphics.Blit(_TempBlurMap, _BlurMap4, blitMaterial, 1);


        // Blur the image 5
        blitMaterial.SetTexture("_MainTex", _BlurMap4);
        blitMaterial.SetFloat("_BlurSpread", 16.0f);
        blitMaterial.SetVector("_BlurDirection", new Vector4(1, 0, 0, 0));
        Graphics.Blit(_BlurMap4, _TempBlurMap, blitMaterial, 1);

        blitMaterial.SetTexture("_MainTex", _TempBlurMap);
        blitMaterial.SetVector("_BlurDirection", new Vector4(0, 1, 0, 0));
        Graphics.Blit(_TempBlurMap, _BlurMap5, blitMaterial, 1);


        // Blur the image 6
        blitMaterial.SetTexture("_MainTex", _BlurMap5);
        blitMaterial.SetFloat("_BlurSpread", 32.0f);
        blitMaterial.SetVector("_BlurDirection", new Vector4(1, 0, 0, 0));
        Graphics.Blit(_BlurMap5, _TempBlurMap, blitMaterial, 1);

        blitMaterial.SetTexture("_MainTex", _TempBlurMap);
        blitMaterial.SetVector("_BlurDirection", new Vector4(0, 1, 0, 0));
        Graphics.Blit(_TempBlurMap, _BlurMap6, blitMaterial, 1);


        thisMaterial.SetTexture("_MainTex", _BlurMap0);
        thisMaterial.SetTexture("_BlurTex0", _BlurMap0);
        thisMaterial.SetTexture("_BlurTex1", _BlurMap1);
        thisMaterial.SetTexture("_BlurTex2", _BlurMap2);
        thisMaterial.SetTexture("_BlurTex3", _BlurMap3);
        thisMaterial.SetTexture("_BlurTex4", _BlurMap4);
        thisMaterial.SetTexture("_BlurTex5", _BlurMap5);
        thisMaterial.SetTexture("_BlurTex6", _BlurMap6);

        yield return new WaitForSeconds(0.1f);

        busy = false;
    }
}