using UnityEngine;
using UnityEngine.InputSystem;
public class SlingShotHandler : MonoBehaviour
{
    [SerializeField] private LineRenderer _leftLineRenderer, _rightLineRenderer;

    [SerializeField] private Transform _leftStarPosition;
    [SerializeField] private Transform _rightStarPosition;
    [SerializeField] private Transform _centerPosition;
    [SerializeField] private Transform _idlePosition;

    [SerializeField] private float _maxDistance = 3.5f;

    [SerializeField] private SlingShotArea _slingShotArea;
    private Vector2 _slingShotLinesPosition;

    private bool _clickedWithinArea;



    // Update is called once per frame
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && _slingShotArea.IsWithinSlingShotArea())
        {

            _clickedWithinArea = true;

        }
        if (Mouse.current.leftButton.isPressed && _clickedWithinArea)
        {
            DrawSlingShot();
        }
        if (Mouse.current.leftButton.wasReleasedThisFrame && _clickedWithinArea)
        {
            _clickedWithinArea = false;
        }
    }
    

    private void DrawSlingShot()
    {
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        _slingShotLinesPosition = _centerPosition.position + Vector3.ClampMagnitude(touchPosition - _centerPosition.position, _maxDistance);

        SetLines(_slingShotLinesPosition);
    }
    private void SetLines(Vector2 position)
    {
        _leftLineRenderer.SetPosition(0, position);
        _leftLineRenderer.SetPosition(1, _leftStarPosition.position);

        _rightLineRenderer.SetPosition(0, position);
        _rightLineRenderer.SetPosition(1, _rightStarPosition.position);
    }
}
