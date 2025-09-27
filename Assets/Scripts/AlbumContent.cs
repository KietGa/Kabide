using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlbumContent : MonoBehaviour
{
    private void Start()
    {
        float mul = transform.childCount / 8 + 1;
        if (transform.childCount % 8 == 0)
        {
            mul--;
        }
        float space = 220 * mul + 10 * (transform.childCount / 8);
        transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, space);
    }
}
