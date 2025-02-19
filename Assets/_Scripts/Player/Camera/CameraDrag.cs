using UnityEngine;
using UnityEngine.InputSystem;

public class CameraDrag : MonoBehaviour
{
    private Vector3 _origin;
    private Vector3 _difference;

    private Camera _mainCamera;
    private float _mainCameraSize;

    private bool _isDragging;

    private bool _isScrolling;

    PlayerInput _playerInput;
    public float mouseScrollY;

    private void Awake()
    {
        _playerInput = new PlayerInput();

        //_playerInput.Gameplay.Zoom.performed += x => mouseScrollY = x.ReadValue<float>();
        _playerInput.Gameplay.Zoom.performed += Zoom;

        _mainCamera = Camera.main;
        _mainCameraSize = _mainCamera.fieldOfView;
    }

    public void OnDrag(InputAction.CallbackContext ctx)
    {
        if (ctx.started) _origin = _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        _isDragging = ctx.started || ctx.performed;
    }

    private void Zoom(InputAction.CallbackContext ctx)
    {
        mouseScrollY = ctx.ReadValue<float>();

        if(mouseScrollY > 0 && _mainCamera.orthographicSize - 0.5 >= 7f)
        {
            _mainCamera.orthographicSize -= 0.5f;
        }

        else if(mouseScrollY < 0 && _mainCamera.orthographicSize + 0.5 <= 15f)
        {
            _mainCamera.orthographicSize += 0.5f;
        }
    }

    private void LateUpdate()
    {
        if (_isDragging)
        {
            _difference = GetMousePosition() - _mainCamera.transform.position;
            _mainCamera.transform.position = _origin - _difference;
        }
    }

    private Vector3 GetMousePosition()
    {
        return _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }

    public void OnScroll(InputAction.CallbackContext ctx)
    {
        _isScrolling = ctx.started || ctx.performed;        
    }

    private void OnEnable()
    {
        _playerInput.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Disable();
    }
}
