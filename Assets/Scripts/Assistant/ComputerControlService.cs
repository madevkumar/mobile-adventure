using System;
using System.Collections.Generic;
using UnityEngine;

public class ComputerControlService : MonoBehaviour
{
    [SerializeField] private AssistantPermissionManager permissionManager;
    private readonly List<string> actionLog = new List<string>();

    public bool TryExecuteAction(string requestedAction, bool confirmed, out string response)
    {
        if (permissionManager != null && !permissionManager.HasPermission(AssistantPermission.ComputerControl))
        {
            response = "Computer control is disabled by permission settings.";
            return false;
        }

        if (!confirmed)
        {
            response = "Action denied. Confirmation is required for computer control.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(requestedAction))
        {
            response = "No action provided.";
            return false;
        }

        string loweredAction = requestedAction.ToLowerInvariant();
        string executionResult;

        if (loweredAction.Contains("open browser"))
        {
            executionResult = "Browser open request accepted (stub execution).";
        }
        else if (loweredAction.Contains("open editor"))
        {
            executionResult = "Editor open request accepted (stub execution).";
        }
        else if (loweredAction.Contains("search files"))
        {
            executionResult = "File search request accepted (stub execution).";
        }
        else
        {
            executionResult = "Unknown system action. No changes executed.";
        }

        string logEntry = $"{DateTime.Now:O} | {executionResult} | Request: {requestedAction}";
        actionLog.Add(logEntry);
        Debug.Log(logEntry);

        response = executionResult;
        return true;
    }

    public IReadOnlyList<string> GetActionLog()
    {
        return actionLog;
    }
}
