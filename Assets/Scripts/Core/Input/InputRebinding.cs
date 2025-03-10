using UnityEngine;
using UnityEngine.InputSystem;

public class InputRebinding : MonoBehaviour
{
    private void Awake()
    {
        InputSystem.onActionChange += OnActionChange;
    }

    // When the action system re-resolves bindings, we want to update our UI in response. While this will
    // also trigger from changes we made ourselves, it ensures that we react to changes made elsewhere. If
    // the user changes keyboard layout, for example, we will get a BoundControlsChanged notification and
    // will update our UI to reflect the current keyboard layout.
    private void OnActionChange(object obj, InputActionChange inputActionChange)
    {
        // TODO: 인풋액션을 바꿀 일이 있으면 구현
    }

    
}
