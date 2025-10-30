using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconHandler : MonoBehaviour
{
    [SerializeField] private Image[] _icons;
    [SerializeField] private Color _usedColor;
    [SerializeField] private Color _defaultColor = Color.white; // thêm màu mặc định

    private void Start()
    {
        ResetIcons();
    }

    public void ResetIcons()
    {
        foreach (var icon in _icons)
        {
            icon.color = Color.white;
            icon.gameObject.SetActive(true);
        }
    }

    public void UseShot(int shotNumber)
    {
        for (int i = 0; i < _icons.Length; i++)
        {
            if (i + 1 == shotNumber)
            {
                _icons[i].color = _usedColor;
                return;
            }
        }
    }

    public void InitializeIcons(int shotsAvailable)
    {
        ResetIcons();

        for (int i = 0; i < _icons.Length; i++)
        {
            _icons[i].gameObject.SetActive(i < shotsAvailable);
        }
    }
}
