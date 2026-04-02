using HarmonyLib;
using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;

namespace HelveHammerSoundMod
{
    public class HHSMConfig
    {
        public float volume = 1f;
        public bool CustomSound = true;
    }

    public class HHSM : ModSystem
    {
        public void logger(string thing)
        {
            Mod.Logger.Chat(thing);
        }
        static HHSM nomnom;
        public static HHSMConfig config;


        private void TryToLoadConfig(ICoreAPI api)
        {
            //It is important to surround the LoadModConfig function in a try-catch. 
            //If loading the file goes wrong, then the 'catch' block is run.
            try
            {
                config = api.LoadModConfig<HHSMConfig>("HHSMConfig.json");
                if (config == null) //if the 'HHSMConfig.json' file isn't found...
                {
                    config = new HHSMConfig();
                }
                //Save a copy of the mod config.
                api.StoreModConfig<HHSMConfig>(config, "HHSMConfig.json");
            }
            catch (Exception e)
            {
                //Couldn't load the mod config... Create a new one with default settings, but don't save it.
                Mod.Logger.Error("Could not load config! Loading default settings instead.");
                Mod.Logger.Error(e);
                config = new HHSMConfig();
            }
        }

        public override void Start(ICoreAPI api)
        {
            TryToLoadConfig(api);
        }




        public override void StartClientSide(ICoreClientAPI api)
        {
            nomnom = this;

            var harmony = new Harmony(Mod.Info.ModID);
            harmony.PatchAll();
        }
    }
    [HarmonyPatch(typeof(ClientMain), nameof(ClientMain.PlaySoundAt), new Type[] { typeof(AssetLocation), typeof(double), typeof(double), typeof(double), typeof(IPlayer), typeof(Single), typeof(Single), typeof(Single) })]
    public static class patch0
    {


        public static bool Prefix(ClientMain __instance, double posx, double posy, double posz, IPlayer dualCallByPlayer, float pitch, float range)
        {
            string name = (new System.Diagnostics.StackTrace()).GetFrame(2).GetMethod().Name;
            if (name == "get_Angle")
            {
                if (HHSM.config.CustomSound == true)
                {
                    __instance.PlaySoundAt(new AssetLocation("sounds/effect/anvilhit"), posx, posy, posz, dualCallByPlayer, pitch, range, HHSM.config.volume);
                }
                else
                {
                    __instance.PlaySoundAt(new AssetLocation("sounds/anvil2/hhanvil"), posx, posy, posz, dualCallByPlayer, pitch, range, HHSM.config.volume);
                }

                return false;
            }
            return true;
        }


    }


}
