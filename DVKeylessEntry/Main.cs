using UnityModManagerNet;

namespace DVKeylessEntry
{
    /// <summary>
    /// Main entry point for the mod
    /// </summary>
#if DEBUG
    [EnableReloading]
#endif
    static class Main
    {
        /// <summary>
        /// Current mod settings
        /// </summary>
        public static Settings settings;

        /// <summary>
        /// Loads the mod
        /// </summary>
        /// <param name="modEntry">Mod</param>
        /// <returns>true, if load succeeded</returns>
        static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                settings = Settings.Load<Settings>(modEntry);
            }
            catch
            {
                settings = null;
            }
            if (settings == null)
            {
                settings = new Settings();
            }
            modEntry.OnToggle = OnToggle;
            modEntry.OnUnload = OnUnload;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            return true;
        }

        /// <summary>
        /// Renders the UI
        /// </summary>
        /// <param name="modEntry">Mod</param>
        static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Draw(modEntry);
        }

        /// <summary>
        /// Save button event from UI
        /// </summary>
        /// <param name="modEntry">Mod</param>
        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            settings.Save(modEntry);
        }

        /// <summary>
        /// Mod unload
        /// </summary>
        /// <param name="modEntry">Mod</param>
        /// <returns>true, if unloaded</returns>
        static bool OnUnload(UnityModManager.ModEntry modEntry)
        {
            return OnToggle(modEntry, false);
        }

        /// <summary>
        /// Mod status toggle
        /// </summary>
        /// <param name="modEntry">Mod</param>
        /// <param name="value">status</param>
        /// <returns>true, if toggled</returns>
        static bool OnToggle(UnityModManager.ModEntry modEntry, bool value)
        {
            if (value)
            {
                DVKeylessEntry.Start();
            }
            else
            {
                DVKeylessEntry.Stop();
            }
            return true;
        }
    }
}
