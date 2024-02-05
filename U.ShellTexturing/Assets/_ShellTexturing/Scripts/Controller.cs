using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public ShellTexturing shellTexturing;
    public Slider density;
    public Slider thickness;
    public Slider windDirectionX;
    public Slider windDirectionZ;
    public Slider windFrequency;
    public Slider windAmplitude;

    private void Start()
    {
        SetDensity();
    }


    private Color StrToColor(string str)
    {
        try
        {
            byte r = byte.Parse(str.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(str.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(str.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

            return new Color32(r, g, b, 255);
        }
        catch(Exception e)
        {
            Debug.Log(e);
            return new Color32(0,0,0,0);
        }
    }

    public void SetTipColor(string str)
    {
        shellTexturing.SetTipColor(StrToColor(str));
    }

    public void SetBaseColor(string str)
    {
        shellTexturing.SetBaseColor(StrToColor(str));
    }

    public void SetDensity()
    {
        shellTexturing.SetDensity((int)density.value);
    }

    public void SetThickness()
    {
        shellTexturing.SetThickness(thickness.value);
    }

    public void SetWindDirectionX()
    {
        shellTexturing.SetWindDirection(windDirectionX.value, shellTexturing.z);
    }

    public void SetWindDirectionZ()
    {
        shellTexturing.SetWindDirection(shellTexturing.x, windDirectionZ.value);
    }

    public void SetWindFrequency()
    {
        shellTexturing.SetWindFrequency(windFrequency.value);
    }

    public void SetWindAmplitude()
    {
        shellTexturing.SetWindAmplitude(windAmplitude.value);
    }
}
