using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconHandler : MonoBehaviour
{
    [SerializeField] private Image[] _icons;
    [SerializeField] private Color _usedColor;

    public void UseShot(int ShotNumber)
    {
        for (int i = 0; i < _icons.Length; i++)
        {
            if(i + 1 == ShotNumber)
            {
                _icons[i].color = _usedColor;
                return;
            }
        }
    }
}
