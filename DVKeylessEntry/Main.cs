using UnityModManagerNet;

namespace DVKeylessEntry
{
    [EnableReloading]
    static class Main
    {
        static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnToggle = OnToggle;
            modEntry.OnUnload = OnUnload;
            return true;
        }

        static bool OnUnload(UnityModManager.ModEntry modEntry)
        {
            return OnToggle(modEntry, false);
        }

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
