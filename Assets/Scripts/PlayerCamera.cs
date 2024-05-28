using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    public void SetPosition(Vector3 position)
    {
        // 카메라 트랜스폼은 반드시 LateUpdate에서 변경해야한다.
        transform.position = position;
    }
}
