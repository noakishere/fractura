using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField] private GameObject m_Camera;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CameraControl.Instance.ChangeCamera(m_Camera);
    }
}
