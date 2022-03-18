using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using System.Collections.Generic;

public class Main : MonoBehaviour
{

    public Response responseObject;
    void Start()
    {
        // A correct website page.
        StartCoroutine(GetRequest("https://servizos.meteogalicia.gal/mgrss/predicion/jsonPredConcellos.action?idConc=15030"));
    }
    

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;

            }
            // Convertimos el formato JSON en objeto
            responseObject = JsonUtility.FromJson<Response>(webRequest.downloadHandler.text);
            Debug.Log(responseObject.predconcello.idConcello);
            Debug.Log(responseObject.predconcello.listaPredDiaConcello[1].ceo.dataPredicion);
        }
    }

}

[Serializable]
public class Response{
    public ConcelloPred predconcello;
}

[Serializable]
public class ConcelloPred{

    public int idConcello;
    public List<DiaConcello> listaPredDiaConcello;
    public string nome;

}

[Serializable]
public class DiaConcello{
    public TempoCeo ceo;
    public TempoChoiva pchoiva;
    public TempoVento vento;
}

[Serializable]
public class TempoCeo{
    public string dataPredicion;
    public int nivelAviso;
}

[Serializable]
public class TempoChoiva{
    public int tMax;
    public int tMin;
    public int tmaxFranxa;
    public int tminFranxa;
    public int uvMax;
}

[Serializable]
public class TempoVento{
    public int manha;
    public int noite;
    public int tarde;
}

