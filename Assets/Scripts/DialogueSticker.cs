using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSticker : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset;
    private RectTransform uiTransform;
    // Start is called before the first frame update
    void Start()
    {
        uiTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && player.activeSelf)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(player.transform.position) + offset;
            uiTransform.position = screenPosition;
        }
    }
}
