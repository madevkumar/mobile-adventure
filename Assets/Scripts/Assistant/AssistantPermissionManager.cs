using System.Collections;
using UnityEngine;

public enum AssistantPermission
{
    Microphone,
    Camera,
    ComputerControl
}

public class AssistantPermissionManager : MonoBehaviour
{
    [Header("Permission Toggles")]
    [SerializeField] private bool allowMicrophone = true;
    [SerializeField] private bool allowCamera = true;
    [SerializeField] private bool allowComputerControl;

    public bool HasPermission(AssistantPermission permission)
    {
        switch (permission)
        {
            case AssistantPermission.Microphone:
                return allowMicrophone;
            case AssistantPermission.Camera:
                return allowCamera;
            case AssistantPermission.ComputerControl:
                return allowComputerControl;
            default:
                return false;
        }
    }

    public void SetPermission(AssistantPermission permission, bool allowed)
    {
        switch (permission)
        {
            case AssistantPermission.Microphone:
                allowMicrophone = allowed;
                break;
            case AssistantPermission.Camera:
                allowCamera = allowed;
                break;
            case AssistantPermission.ComputerControl:
                allowComputerControl = allowed;
                break;
        }
    }

    public IEnumerator RequestRuntimeAuthorizations()
    {
        if (allowMicrophone)
        {
            yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
        }

        if (allowCamera)
        {
            yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        }
    }
}
