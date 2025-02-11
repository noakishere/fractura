using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletSpeed;

    [Header("Rotate")]
    [SerializeField] private bool rotateWithMouse = true;
    [SerializeField] private bool rotateWithGamepad = false;

    [SerializeField] private float angle;
    [SerializeField] private Vector2 dir;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rotateWithGamepad)
        {
            float hRight = Input.GetAxis("RightStickHorizontal");
            float vRight = Input.GetAxis("RightStickVertical");

            // If the right stick is moved at all
            if (new Vector2(hRight, vRight).sqrMagnitude > 0.01f)
            {
                angle = Mathf.Atan2(vRight, hRight) * Mathf.Rad2Deg - 90f;

                //weapon.transform.rotation = Quaternion.Euler(0f, 0f, angle);
                return; // Skip mouse rotation if gamepad stick is active
            }
        }

        if (rotateWithMouse)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;

            dir = (mousePos - transform.position);
            angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            //weapon.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var newBullet = Instantiate(bullet, transform.position, Quaternion.identity);

            Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = dir * bulletSpeed;
            }
        }
    }
}
