using UnityEngine;
using UnityEngine.InputSystem;

namespace Topdown.Movement
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerMovementController : Mover
    {
        [Header("Movement")]
        private Vector2 movementInput;

        [Header("Rotate")]
        [SerializeField] private bool rotateWithMouse = true;
        [SerializeField] private bool rotateWithGamepad = false;

        [SerializeField] private GameObject weapon;

        // Start is called before the first frame update
        void Start()
        {
            if (Input.GetJoystickNames().Length > 0)
            {
                rotateWithGamepad = true;
            }
        }

        // Update is called once per frame
        void Update()
        {
            //CalculateMovementInput();

            //if (rotateWithGamepad)
            //{
            //    float hRight = Input.GetAxis("RightStickHorizontal");
            //    float vRight = Input.GetAxis("RightStickVertical");

            //    // If the right stick is moved at all
            //    if (new Vector2(hRight, vRight).sqrMagnitude > 0.01f)
            //    {
            //        float angle = Mathf.Atan2(vRight, hRight) * Mathf.Rad2Deg - 90f;
            //        weapon.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            //        return; // Skip mouse rotation if gamepad stick is active
            //    }
            //}

            //if (rotateWithMouse)
            //{
            //    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //    mousePos.z = 0f;

            //    Vector2 dir = (mousePos - transform.position);
            //    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            //    weapon.transform.rotation = Quaternion.Euler(0f, 0f, angle);
            //}
        }

        private void OnMove(InputValue value)
        {
            Vector3 playerInput = new Vector3(value.Get<Vector2>().x, value.Get<Vector2>().y, 0);
            currentInput = playerInput;
        }
    }
}



