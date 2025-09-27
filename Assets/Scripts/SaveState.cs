using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveState : MonoBehaviour
{
    public static SaveState SSInstance { get; private set; }
    [SerializeField] Random.State lastState;

    private void Awake()
    {
        SSInstance = this;
    }

    public void LoadSeed()
    {
        var json = PlayerPrefs.GetString("random");
        if (!string.IsNullOrEmpty(json))
        {
            JsonUtility.FromJsonOverwrite(json, this);
            Random.state = lastState;
        }
    }

    public void SaveSeed()
    {
        lastState = Random.state;
        var json = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("random", json);
    }
}
