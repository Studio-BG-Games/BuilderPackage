using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Editor
{
    public static class Builder
    {
        [MenuItem("📦Build/Android")]
        public static void BuildAndroid()
        {
            string path = Application.dataPath;
            path = path.Replace($"/Assets", "");
            path = Path.Combine(path, "Build");
            Directory.CreateDirectory(path);

            BuildReport report = BuildPipeline.BuildPlayer(
                new BuildPlayerOptions
                {
                    scenes = GetScenes(),
                    locationPathName =
                        $"{path}/artifacts/Build.apk",
                    target = BuildTarget.Android,
                    options = BuildOptions.None
                });

            BuildSummary summary = report.summary;

            switch (summary.result)
            {
                case BuildResult.Succeeded:
                    UnityEngine.Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
                    break;
                case BuildResult.Failed:
                    UnityEngine.Debug.LogError("Build failed");
                    break;
            }
        }

        private static string[] GetScenes()
        {
            string[] scenes = EditorBuildSettings.scenes
                .Where(scene => scene.enabled) // Только включенные сцены
                .Select(scene => scene.path)
                .ToArray();
            return scenes;
        }
    }
}
