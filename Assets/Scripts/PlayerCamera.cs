using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    public void SetPosition(Vector3 position)
    {
        // ī�޶� Ʈ�������� �ݵ�� LateUpdate���� �����ؾ��Ѵ�.
        transform.position = position;
    }
}
