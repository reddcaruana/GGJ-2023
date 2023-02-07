using UnityEngine;

public class ActivateIfMobile : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(SystemInfo.deviceType == DeviceType.Handheld);
    }
}