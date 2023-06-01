using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class Api : MonoBehaviour
{
    public static Api Instance { get; private set; }
    public string BlockEndpoint;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Run a WebRequest and receive a callback on response.
    /// </summary>
    /// <param name="url">Api endpoint to get data from.</param>
    /// <param name="callback">Callback to run after response succeeded.</param>
    public IEnumerator Get(string url, Action<string> callback)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        switch (request.result)
        {
            case UnityWebRequest.Result.Success:
                var response = request.downloadHandler.text;
                callback?.Invoke(response);
                break;
            default:
                Debug.LogError("Error: " + request.error);
                break;
        }
    }

    /// <summary>
    /// Get Block data from Remote Server.
    /// </summary>
    /// <param name="callback">Callback to run after data received.</param>
    public void GetBlocksData(Action<string> callback)
    {
        StartCoroutine(Get(BlockEndpoint, (o) =>
        {
            callback?.Invoke(o);
        }
        ));
    }
}
