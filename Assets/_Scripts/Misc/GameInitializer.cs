using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [Tooltip("Order important")]
    [SerializeField] private List<GameObject> gameObjectsToEnable;
    void Awake()
    {
        DisableGameObjects();
        EnableGameObjects();
    }

    private void EnableGameObjects()
    {
        foreach (GameObject gameObject in gameObjectsToEnable)
        {
            gameObject.SetActive(true);
        }
    }

    private void DisableGameObjects()
    {
        foreach (GameObject gameObject in gameObjectsToEnable)
        {
            gameObject.SetActive(false);
        }
    }
}
