using UnityEngine;

public class JarvisCodeGenerator : MonoBehaviour
{
    public string GenerateCode(string prompt)
    {
        if (string.IsNullOrWhiteSpace(prompt))
        {
            return "// Please provide a request to generate code.";
        }

        string loweredPrompt = prompt.ToLowerInvariant();

        if (loweredPrompt.Contains("game"))
        {
            return BuildGameTemplate();
        }

        if (loweredPrompt.Contains("web"))
        {
            return BuildWebTemplate();
        }

        if (loweredPrompt.Contains("ml") || loweredPrompt.Contains("machine learning"))
        {
            return BuildMachineLearningTemplate();
        }

        if (loweredPrompt.Contains("dl") || loweredPrompt.Contains("deep learning"))
        {
            return BuildDeepLearningTemplate();
        }

        if (loweredPrompt.Contains("app") || loweredPrompt.Contains("mobile"))
        {
            return BuildAppTemplate();
        }

        return "// Request recognized, but no template matched. Try: app, game, web, ML, or DL.";
    }

    private string BuildAppTemplate()
    {
        return
            "using UnityEngine;\n\n" +
            "public class MobileAppStarter : MonoBehaviour\n" +
            "{\n" +
            "    private void Start()\n" +
            "    {\n" +
            "        Debug.Log(\"App starter initialized.\");\n" +
            "    }\n" +
            "}";
    }

    private string BuildGameTemplate()
    {
        return
            "using UnityEngine;\n\n" +
            "public class GameStarter : MonoBehaviour\n" +
            "{\n" +
            "    public int score;\n\n" +
            "    public void AddScore(int points)\n" +
            "    {\n" +
            "        score += Mathf.Max(points, 0);\n" +
            "    }\n" +
            "}";
    }

    private string BuildWebTemplate()
    {
        return
            "// Basic web app pseudo-template\n" +
            "// 1. Create API endpoint: GET /health\n" +
            "// 2. Add frontend route: /dashboard\n" +
            "// 3. Connect UI with API client\n";
    }

    private string BuildMachineLearningTemplate()
    {
        return
            "# ML starter pipeline\n" +
            "# 1. Load dataset\n" +
            "# 2. Train/test split\n" +
            "# 3. Train baseline model\n" +
            "# 4. Evaluate and save metrics\n";
    }

    private string BuildDeepLearningTemplate()
    {
        return
            "# DL starter pipeline\n" +
            "# 1. Build neural network\n" +
            "# 2. Train with validation\n" +
            "# 3. Save checkpoints\n" +
            "# 4. Evaluate on test set\n";
    }
}
