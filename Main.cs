using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Commands;
using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleEject
{
    public class Main : RocketPlugin
    {
        protected override void Load()
        {
            Logger.Log("Nel Plugins", ConsoleColor.White);
            Logger.Log("Eject Plugin Loaded", ConsoleColor.Green);
            Logger.Log("https://discord.gg/plugin", ConsoleColor.Red);
        }

        protected override void Unload()
        {
            Logger.Log("Nel Plugins", ConsoleColor.White);
            Logger.Log("Eject Plugin Unloaded", ConsoleColor.Green);
            Logger.Log("https://discord.gg/plugin", ConsoleColor.Red);
        }

        [RocketCommand("eject", "Eject Player.", "/eject", AllowedCaller.Player)]
        [RocketCommandPermission("nel.eject")]
        public void Eject(IRocketPlayer caller, string[] args)
        {
            var player = (UnturnedPlayer)caller;

            if (args.Length < 1)
            {
                UnturnedChat.Say(caller, Translate("EjectNoPlayer"), UnityEngine.Color.red);
                return;
            }

            string targetName = args[0];

            UnturnedPlayer targetPlayer = UnturnedPlayer.FromName(targetName);

            if (targetPlayer == null)
            {
                UnturnedChat.Say(caller, Translate("EjectPlayerNotFound"), UnityEngine.Color.red);
                return;
            }

            if (targetPlayer.CurrentVehicle == null)
            {
                UnturnedChat.Say(caller, Translate("EjectPlayerNotInVehicle"), UnityEngine.Color.red);
                return;
            }

            var vehicle = targetPlayer.CurrentVehicle;
            for (int i = 0; i < vehicle.passengers.Length; i++)
            {
                if (vehicle.passengers[i].player?.playerID.steamID == targetPlayer.CSteamID)
                {
                    VehicleManager.forceRemovePlayer(vehicle, targetPlayer.CSteamID); // Remove player from the vehicle
                    UnturnedChat.Say(caller, string.Format(Translate("EjectSuccess"), targetPlayer.CharacterName), UnityEngine.Color.green);
                    return;
                }
            }

            UnturnedChat.Say(caller, Translate("EjectPlayerNotInVehicleFound"), UnityEngine.Color.red);
        }

        public override TranslationList DefaultTranslations => new TranslationList
        {
            { "EjectNoPlayer", "Please enter a player name!" },
            { "EjectPlayerNotFound", "No player found with that name!" },
            { "EjectPlayerNotInVehicle", "The target player is not currently in a vehicle!" },
            { "EjectPlayerNotInVehicleFound", "The target player could not be found in the vehicle!" },
            { "EjectSuccess", "{0} has been ejected from the vehicle." }
        };
    }
}
