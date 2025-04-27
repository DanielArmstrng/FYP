using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject CrosshairCanvas;
    private PlayerController playerController;
    
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
        playerController = FindObjectOfType<PlayerController>();
        playerController.gravity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Invoke(nameof(EnableMovement), 10.0f);
        Invoke(nameof(EnableObject), 16.9f); // Enable the object at the same time as movement
        Destroy(gameObject, 17.0f);
    }

    void EnableMovement()
    {
        playerController.gravity = 10;
    }

    void EnableObject()
    {
        if (CrosshairCanvas != null)
        {
            CrosshairCanvas.SetActive(true);
        }
    }
}
