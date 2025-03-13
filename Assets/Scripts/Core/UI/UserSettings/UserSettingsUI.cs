using UnityEngine;

namespace Core.UI.UserSettings
{
    public class UserSettingsUI : MonoBehaviour
    {
        public void Show()
        {
            if (gameObject.activeInHierarchy)
                gameObject.SetActive(false);
            else
                gameObject.SetActive(true);
        }
    }
}