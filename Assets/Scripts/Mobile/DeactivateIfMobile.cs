using UnityEngine;

public class DeactivateIfMobile : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(SystemInfo.deviceType != DeviceType.Handheld);
    }
}