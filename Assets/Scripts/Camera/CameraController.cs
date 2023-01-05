using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    public Transform player;

    private void Update()
        
    {
        transform.position = new Vector3(player.position.x - 1, player.position.y, transform.position.z);
    }
}

