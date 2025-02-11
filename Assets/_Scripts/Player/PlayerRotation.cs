using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Topdown.Movement
{
    public class PlayerRotation : Rotator
    {
        private Camera cm1;

        private void Start()
        {
            cm1 = Camera.main;
        }

        private void OnLook(InputValue value)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(value.Get<Vector2>());
            LookAt(mousePosition);
        }
    }
}