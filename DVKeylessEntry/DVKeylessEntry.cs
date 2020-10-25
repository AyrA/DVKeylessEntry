using System;
using System.Reflection;
using UnityEngine;

namespace DVKeylessEntry
{
    public static class DVKeylessEntry
    {
        private static LocoControllerBase CurrentLoco = null;
        private static bool init = false;

        /// <summary>
        /// Print log message
        /// </summary>
        /// <param name="m">Message</param>
        private static void LogMessage(string m)
        {
            Debug.Log($"{nameof(DVKeylessEntry)}: {m}");
        }

        /// <summary>
        /// Print warning message
        /// </summary>
        /// <param name="m">Message</param>
        private static void WarnMessage(string m)
        {
            Debug.LogWarning($"{nameof(DVKeylessEntry)}: {m}");
        }

        /// <summary>
        /// Print error message
        /// </summary>
        /// <param name="m">Message</param>
        private static void ErrorMessage(string m)
        {
            Debug.LogError($"{nameof(DVKeylessEntry)}: {m}");
        }

        /// <summary>
        /// Start the mod
        /// </summary>
        public static void Start()
        {
            if (!init)
            {
                init = true;
                PlayerManager.CarChanged += PlayerManager_CarChanged;
                LogMessage($"Mod version {Assembly.GetExecutingAssembly().GetName().Version} initialized");
            }
        }

        /// <summary>
        /// Stop the mod
        /// </summary>
        public static void Stop()
        {
            if (init)
            {
                PlayerManager.CarChanged -= PlayerManager_CarChanged;
                init = false;
                CurrentLoco = null;
                LogMessage($"Mod stopped");
            }
        }

        /// <summary>
        /// Handle player car change events
        /// </summary>
        /// <param name="obj">New car (can be null if leaving a car)</param>
        private static void PlayerManager_CarChanged(TrainCar obj)
        {
            if (obj != null)
            {
                LocoControllerBase loco;
                try
                {
                    loco = obj.GetComponent<LocoControllerBase>();
                }
                catch (Exception ex)
                {
                    WarnMessage($"Car lacks loco. Error: {ex.Message}");
                    return;
                }
                if (Main.settings.KeylessEntryAutostart)
                {
                    //Keyless entry only for the diesel locos
                    if (loco is LocoControllerDiesel || loco is LocoControllerShunter)
                    {
                        var l1 = loco as LocoControllerDiesel;
                        var l2 = loco as LocoControllerShunter;
                        if (l1 != null)
                        {
                            l1.SetEngineRunning(true);
                        }
                        if (l2 != null)
                        {
                            l2.SetEngineRunning(true);
                        }
                        LogMessage("Started current loco");
                    }
                }
                CurrentLoco = loco;
            }
            else
            {
                //Shuts down a loco if it's not being used
                if (CurrentLoco != null)
                {
                    if (ShouldPerformAutoShutdown(CurrentLoco))
                    {
                        LogMessage("Shutting down current loco");
                        var Shunter = CurrentLoco as LocoControllerShunter;
                        var Diesel = CurrentLoco as LocoControllerDiesel;
                        if (Shunter != null)
                        {
                            Shunter.SetEngineRunning(false);
                        }
                        if (Diesel != null)
                        {
                            Diesel.SetEngineRunning(false);
                        }
                    }
                }
                CurrentLoco = null;
            }
        }

        /// <summary>
        /// Determines whether this locomotive is eligible for automatic shutdown on player exit
        /// </summary>
        /// <param name="Loco">Locomotive</param>
        /// <returns>true, if should be shut down</returns>
        private static bool ShouldPerformAutoShutdown(LocoControllerBase Loco)
        {
            var s = Main.settings.ShutdownOptions;
            if (s == null)
            {
                s = new AutomaticShutdownOptions();
            }
            //Don't do anything if this object is not present
            if (Loco == null)
            {
                WarnMessage("ShouldPerformAutoShutdown argument is <null> but should not be");
                return false;
            }
            //Don't shut down if cars are coupled
            if (s.NoCarsCoupled && (Loco.GetNumberOfCarsInFront() > 0 || Loco.GetNumberOfCarsInRear() > 0))
            {
                return false;
            }
            //Don't shut down if a coupler is in range
            if (s.NoCarsInCouplingRange && Loco.IsCouplerInRange(LocoControllerBase.COUPLING_RANGE))
            {
                return false;
            }
            //Don't shut down if rolling
            if (s.MustBeStationary && Math.Abs(Loco.GetSpeedKmH()) >= 1.0f)
            {
                return false;
            }
            //Don't shut down if the reverser is not centered
            if (s.ReverserCentered && Math.Abs(Loco.reverser) >= 0.1f)
            {
                return false;
            }
            //Don't shut down if independent brake not at least 90% applied
            if (s.IndependentBrakesApplied && Loco.independentBrake < 0.9f)
            {
                return false;
            }
            //Don't shut down if remote controlled
            if (s.RemoteControlDisconnected && Loco.IsRemoteControlled())
            {
                return false;
            }
            //All checks passed
            return true;
        }
    }
}
