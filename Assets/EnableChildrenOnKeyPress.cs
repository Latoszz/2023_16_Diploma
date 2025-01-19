using UnityEngine;

public class EnableChildrenOnKeyPress : MonoBehaviour
{
    [SerializeField] private KeyCode toggleKey = KeyCode.P; 
    private bool areChildrenEnabled = false;

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            areChildrenEnabled = !areChildrenEnabled;
            SetChildrenActive(areChildrenEnabled);
        }
    }

    private void SetChildrenActive(bool isActive)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(isActive);
        }
    }
}