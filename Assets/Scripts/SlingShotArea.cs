using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlingShotArea : MonoBehaviour
{
    [SerializeField] private LayerMask _slingShotAreaLayerMask;
    public bool IsWithinSlingShotArea()
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(InputManager.MousePosition);

        if (Physics2D.OverlapPoint(worldPosition, _slingShotAreaLayerMask))
        { return true; }
        else
        { return false; }
    }

}
