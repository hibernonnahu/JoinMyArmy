using System;
using TMPro;
using UnityEngine;

public class TextShortHandler : MonoBehaviour
{
    private const float TEXT_TIME = 1.7F;
    private const float TEXT_START_Y = 2.5F;
    private const float TEXT_END_Y = 7F;
    public TextMeshPro[] text;
    private int currentIndex = 0;
    private void Start()
    {
        text = GetComponentsInChildren<TextMeshPro>();
    }
    public void SetDialog(Vector3 position, string say, Color color = default(Color))
    {
        TextMeshPro text = GetCurrentText();
        text.transform.position = position + Vector3.up * TEXT_START_Y;
        text.text = say;
        text.color = color;
        LeanTween.moveY(text.gameObject, TEXT_END_Y, TEXT_TIME).setEaseOutCubic().setOnComplete(() => { text.text = ""; });
    }

    private TextMeshPro GetCurrentText()
    {
        currentIndex = (currentIndex + 1) % text.Length;
        return text[currentIndex];
    }
    private void OnDestroy()
    {
        for (int i = 0; i < text.Length; i++)
        {
            LeanTween.cancel(text[i].gameObject);
        }
    }
}