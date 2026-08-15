using System;
using UnityEngine;

public class JarvisAssistantManager : MonoBehaviour
{
    [Header("Identity")]
    [SerializeField] private string userName = "User";

    [Header("Voice Output (non-AI)")]
    [SerializeField] private AudioSource voiceSource;
    [SerializeField] private AudioClip prerecordedWelcomeClip;

    [Header("Modules")]
    [SerializeField] private AssistantPermissionManager permissionManager;
    [SerializeField] private JarvisCommandRouter commandRouter;
    [SerializeField] private ComputerControlService computerControlService;

    private void Start()
    {
        if (permissionManager != null)
        {
            StartCoroutine(permissionManager.RequestRuntimeAuthorizations());
        }

        string greeting = BuildGreetingMessage();
        Debug.Log(greeting);

        if (voiceSource != null && prerecordedWelcomeClip != null)
        {
            voiceSource.clip = prerecordedWelcomeClip;
            voiceSource.Play();
        }
    }

    public string BuildGreetingMessage()
    {
        DateTime now = DateTime.Now;
        string partOfDay = GetPartOfDay(now.Hour);
        return $"Good {partOfDay}, {userName}. Today is {now:dddd, dd MMM yyyy}. Time is {now:HH:mm}.";
    }

    public JarvisRouteResult HandleCommand(string command, bool confirmedForComputerControl)
    {
        if (commandRouter == null)
        {
            return new JarvisRouteResult
            {
                Intent = JarvisIntent.Unknown,
                Response = "Command router is not configured.",
                Payload = string.Empty,
                RequiresConfirmation = false
            };
        }

        JarvisRouteResult routeResult = commandRouter.RouteCommand(command);

        if (routeResult.Intent == JarvisIntent.ControlComputer && computerControlService != null)
        {
            bool executed = computerControlService.TryExecuteAction(routeResult.Payload, confirmedForComputerControl, out string controlResponse);
            routeResult.Response = controlResponse;
            routeResult.Payload = executed ? routeResult.Payload : string.Empty;
        }

        return routeResult;
    }

    private string GetPartOfDay(int hour)
    {
        if (hour < 12)
        {
            return "morning";
        }

        if (hour < 18)
        {
            return "afternoon";
        }

        return "evening";
    }
}
