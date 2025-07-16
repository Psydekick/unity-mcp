using UnityEditor;
using UnityMcpBridge.Editor;
using UnityEngine; // For Debug.Log
using UnityMcpBridge.Editor.Helpers; // For ServerInstaller

namespace UnityMcpBridge.Editor
{
    /// <summary>
    /// Adds the "Psydekick/Setup" menu item to the Unity Editor. Selecting this menu item
    /// starts (or restarts) the Unity-MCP bridge by invoking <see cref="UnityMcpBridge.Start"/>.
    /// </summary>
    internal static class PsydekickMenu
    {
        // The priority value places the item near the top of the Psydekick submenu (lower = higher).
        [MenuItem("Psydekick/Setup", priority = 1)]
        private static void Setup()
        {
            // Start (or restart) the bridge. UnityMcpBridge.Start handles stopping any existing listener first.
            UnityMcpBridge.Start();

            // Retrieve the installed UnityMcpServer directory (src)
            string serverPath = ServerInstaller.GetServerPath();
            Debug.Log($"[Psydekick] UnityMcpServer directory: {serverPath}");

            // Write the server path to Packages/UnityMCPBridge/serverpath.txt (relative to the project root)
            try
            {
                // Project root is one directory above Assets
                string projectRoot = System.IO.Path.GetDirectoryName(UnityEngine.Application.dataPath);
                // Create Assets/Unity MCP Bridge folder (with spaces) and write the file there
                string bridgeDir = System.IO.Path.Combine(UnityEngine.Application.dataPath, "Unity MCP Bridge");
                System.IO.Directory.CreateDirectory(bridgeDir);

                string txtPath = System.IO.Path.Combine(bridgeDir, "serverpath.txt");

                // Write the path string into the file (overwrites if exists)
                System.IO.File.WriteAllText(txtPath, serverPath);
                Debug.Log($"[Psydekick] Wrote server path to {txtPath}");

                // Refresh AssetDatabase so file shows in Project window
                UnityEditor.AssetDatabase.Refresh();
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[Psydekick] Failed to write serverpath.txt: {ex.Message}");
            }

            // Optionally focus the main MCP editor window so the user immediately sees the status.
            // Uncomment the line below if automatic window focus is desired.
            // UnityMcpBridge.Editor.Windows.UnityMcpEditorWindow.ShowWindow();
        }
    }
} 