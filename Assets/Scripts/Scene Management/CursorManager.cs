using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    private Image _cursorImage;
    
    private void Awake()
    {
        _cursorImage = GetComponent<Image>();
    }
    
    void Start()
    {
        Cursor.visible = false;
        if (Application.isPlaying)
            Cursor.lockState = CursorLockMode.None;
        else
            Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        Vector2 cursorPos = Input.mousePosition;
        _cursorImage.rectTransform.position = cursorPos;        
        if (!Application.isPlaying)
            return;
    }
}
