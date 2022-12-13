using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] private GameObject objectToFollow;

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(objectToFollow.transform.position.x, objectToFollow.transform.position.y, -1f);
    }
}
