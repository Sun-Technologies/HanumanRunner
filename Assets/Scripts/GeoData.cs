using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class RootObject
{
    public string @as;
    public string city;
    public string country;
    public string countryCode;
    public string isp;
    public double lat;
    public double lon;
    public string org;
    public string query;
    public string region;
    public string regionName;
    public string zip;
}

public class GeoData : MonoBehaviour
{
    public RootObject rootdata;

    private string url = "http://ip-api.com/json";

    public void Start()
    {
        WWW www = new WWW(url);
        StartCoroutine(OnResponse(www));
        GetUserCountryCode();
    }

    private IEnumerator OnResponse(WWW req)
    {
        yield return req;
        rootdata = JsonUtility.FromJson<RootObject>(req.text);
    }

    public string GetUserCountryCode()
    {
        return rootdata.countryCode;
    }
}







