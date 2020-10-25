using System;
using UnityModManagerNet;

namespace DVKeylessEntry
{
    /// <summary>
    /// Mod settings
    /// </summary>
    public class Settings : UnityModManager.ModSettings, IDrawable
    {
        /// <summary>
        /// Enable/Disable automatic engine start
        /// </summary>
        [Draw("Automatic start")]
        public bool KeylessEntryAutostart = true;

        /// <summary>
        /// Enable/disable various shutdown conditions
        /// </summary>
        [Draw("== Automatic shutdown conditions (all checked conditions must be met) ==", Box = true, Collapsible = true)]
        public AutomaticShutdownOptions ShutdownOptions = new AutomaticShutdownOptions();

        /// <summary>
        /// Request settings to save
        /// </summary>
        /// <param name="modEntry">Mod</param>
        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }

        /// <summary>
        /// Event for settings change
        /// </summary>
        /// <remarks>This is currently not needed by this mod but it needs to be there</remarks>
        public void OnChange()
        {
            //NOOP
        }
    }

    /// <summary>
    /// Collections of Shutdown option settings
    /// </summary>
    public class AutomaticShutdownOptions
    {
        /// <summary>
        /// Reverser in center position
        /// </summary>
        /// <remarks>
        /// This is exactly 0.0 for the current diesel locos,
        /// but for comparibility with other locos in the future, we allow up to 0.1 deviation
        /// </remarks>
        [Draw("Reverser must be centered")]
        public bool ReverserCentered = true;
        /// <summary>
        /// Independent brakes must be at least 90% applied
        /// </summary>
        [Draw("Independent brakes must be applied 90% or more")]
        public bool IndependentBrakesApplied = true;
        /// <summary>
        /// Loco can't be moving (allows for 1 Km/h movement)
        /// </summary>
        [Draw("Locomotive must be stationary")]
        public bool MustBeStationary = true;
        /// <summary>
        /// No cars must be in coupling range
        /// </summary>
        /// <remarks>
        /// The coupling range is defined in <see cref="LocoControllerBase.COUPLING_RANGE"/>
        /// It's 0.64 as of this writing
        /// </remarks>
        [Draw("No Cars in coupling range")]
        public bool NoCarsInCouplingRange = true;
        /// <summary>
        /// No cars can be coupled to the loco
        /// </summary>
        /// <remarks>
        /// I have not tested whether parially coupled cars (coupler OR brakes only) are considered coupled
        /// </remarks>
        [Draw("No cars coupled")]
        public bool NoCarsCoupled = true;
        /// <summary>
        /// Remote controller can't be connected.
        /// Whether the cotroller is turned on or in range doesn't matters.
        /// </summary>
        [Draw("Remote control is disconnected")]
        public bool RemoteControlDisconnected = true;
    }
}
