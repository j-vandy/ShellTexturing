using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Security.Cryptography;
using System;
using NaughtyAttributes;

public class ShellTexturing : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Mesh shellMesh;
    [SerializeField] private Shader shader;
    [Header("Settings")]
    [Range(0.5f, 2f)]
    public float height = 1f;
    public bool autoShellCount = true;
    [HideIf("autoShellCount")]
    [Range(1, 256)]
    public int shellCount = 16;
    [Header("Shader Properties")]
    public Texture heightMapTex;
    [Range(0.01f, 1f)]
    public float heightMapTexScale = 1f;
    public Color tipColor = Color.green;
    public Color baseColor = Color.green;
    [Range(1, 20)]
    public float meshScale = 1f;
    [Range(10, 100)]
    public int density = 125;
    [Range(0f, 1f)]
    public float thickness = 1f;
    public float windFrequency;
    public float windAmplitude;
    [Header("Wind Direction")]
    public float x;
    public float z;

    
    private List<GameObject> gos = new List<GameObject>();

    private void SetShaderProperties(Material mat, float height, float shellHeight)
    {
        mat.SetTexture("_HeightMapTex", heightMapTex);
        mat.SetFloat("_HeightMapTexScale", heightMapTexScale);
        mat.SetColor("_TipColor", tipColor);
        mat.SetColor("_BaseColor", baseColor);
        mat.SetFloat( "_MeshScale", meshScale);
        mat.SetInteger("_Density", density * (int) meshScale);
        mat.SetFloat( "_Height", height);
        mat.SetFloat( "_ShellHeight", shellHeight);
        mat.SetFloat("_Thickness", thickness);
        mat.SetVector("_WindDirection", new Vector3(x, 0, z));
        mat.SetFloat("_WindFrequency", windFrequency);
        mat.SetFloat("_WindAmplitude", windAmplitude);
    }

    public void SetTipColor(Color c)
    {
        tipColor = c;

        foreach(GameObject go in gos)
        {
            go.transform.localScale = transform.localScale;
            Material mat = go.GetComponent<MeshRenderer>().material;
            mat.SetColor("_TipColor", tipColor);
        }
    }

    public void SetBaseColor(Color c)
    {
        baseColor = c;

        foreach(GameObject go in gos)
        {
            go.transform.localScale = transform.localScale;
            Material mat = go.GetComponent<MeshRenderer>().material;
            mat.SetColor("_BaseColor", baseColor);
        }
    }

    public void SetDensity(int d)
    {
        density = d;

        foreach(GameObject go in gos)
        {
            go.transform.localScale = transform.localScale;
            Material mat = go.GetComponent<MeshRenderer>().material;
            mat.SetInteger("_Density", density * (int) meshScale);
        }
    }

    public void SetThickness(float f)
    {
        thickness = f;

        foreach(GameObject go in gos)
        {
            go.transform.localScale = transform.localScale;
            Material mat = go.GetComponent<MeshRenderer>().material;
            mat.SetFloat("_Thickness", thickness);
        }
    }

    public void SetWindDirection(float x, float z)
    {
        this.x = x;
        this.z = z;

        foreach(GameObject go in gos)
        {
            go.transform.localScale = transform.localScale;
            Material mat = go.GetComponent<MeshRenderer>().material;
            mat.SetVector("_WindDirection", new Vector3(x, 0, z));
        }
    }

    public void SetWindFrequency(float f)
    {
        windFrequency = f;

        foreach(GameObject go in gos)
        {
            go.transform.localScale = transform.localScale;
            Material mat = go.GetComponent<MeshRenderer>().material;
            mat.SetFloat("_WindFrequency", windFrequency);
        }
    }

    public void SetWindAmplitude(float f)
    {
        windAmplitude = f;

        foreach(GameObject go in gos)
        {
            go.transform.localScale = transform.localScale;
            Material mat = go.GetComponent<MeshRenderer>().material;
            mat.SetFloat("_WindAmplitude", windAmplitude);
        }
    }
    
    private void InstantiateObjs()
    {
        if (autoShellCount)
            shellCount = (int) (height * 640);

        GameObject parent = new GameObject();
        parent.name = "Shells";

        for (int i = 0; i < shellCount; i++)
        {
            GameObject go = new GameObject();
            go.name = "Shell (" + i + ")";
            go.transform.parent = parent.transform;

            go.AddComponent<MeshFilter>().mesh = shellMesh;
            Material mat = new Material(shader);
            // Configure Shader Properties
            SetShaderProperties(mat, ((float) i / shellCount) * height, (float) i / shellCount);
            go.AddComponent<MeshRenderer>().material = mat;

            gos.Add(go);
        }
    }

    private void Start()
    {
        InstantiateObjs();
    }
}
