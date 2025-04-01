using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{

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
        Invoke(nameof(EnableMovement), 10.5f);
        Destroy(gameObject, 11.0f);
    }

    void EnableMovement()
    {
        playerController.gravity = 10;
    }
}
