using UnityEngine;
using UnityEngine.EventSystems;

public class TestEventSystem : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse click
        {
            Debug.Log("Current selected GameObject: " + EventSystem.current.currentSelectedGameObject);
        }
    }
}