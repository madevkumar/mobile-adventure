using UnityEngine;

public enum JarvisIntent
{
    Unknown,
    GenerateCode,
    ControlComputer
}

public struct JarvisRouteResult
{
    public JarvisIntent Intent;
    public string Response;
    public string Payload;
    public bool RequiresConfirmation;
}

public class JarvisCommandRouter : MonoBehaviour
{
    [SerializeField] private JarvisCodeGenerator codeGenerator;

    public JarvisRouteResult RouteCommand(string command)
    {
        if (string.IsNullOrWhiteSpace(command))
        {
            return new JarvisRouteResult
            {
                Intent = JarvisIntent.Unknown,
                Response = "Please provide a command.",
                Payload = string.Empty,
                RequiresConfirmation = false
            };
        }

        string lowered = command.ToLowerInvariant();

        if (lowered.Contains("generate") || lowered.Contains("build") || lowered.Contains("create"))
        {
            string generatedCode = codeGenerator != null ? codeGenerator.GenerateCode(command) : "// Code generator not configured.";
            return new JarvisRouteResult
            {
                Intent = JarvisIntent.GenerateCode,
                Response = "Code generation request processed.",
                Payload = generatedCode,
                RequiresConfirmation = false
            };
        }

        if (lowered.Contains("control") || lowered.Contains("open") || lowered.Contains("search"))
        {
            return new JarvisRouteResult
            {
                Intent = JarvisIntent.ControlComputer,
                Response = "Computer-control request detected. Confirmation required.",
                Payload = command,
                RequiresConfirmation = true
            };
        }

        return new JarvisRouteResult
        {
            Intent = JarvisIntent.Unknown,
            Response = "Command not recognized. Try project generation or system-control command.",
            Payload = string.Empty,
            RequiresConfirmation = false
        };
    }
}
