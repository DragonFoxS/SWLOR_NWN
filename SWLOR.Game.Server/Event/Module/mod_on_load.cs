﻿using System;
using System.Linq;
using SWLOR.Game.Server.Data;
using SWLOR.Game.Server.Enumeration;
using SWLOR.Game.Server.Event.Module;
using SWLOR.Game.Server.Messaging;
using SWLOR.Game.Server.NWN;
using SWLOR.Game.Server.NWN.Enum;
using SWLOR.Game.Server.NWN.Enum.Creature;
using SWLOR.Game.Server.NWN.Enum.Item;
using SWLOR.Game.Server.NWNX;
using SWLOR.Game.Server.Scripts;
using SWLOR.Game.Server.Service;
using SWLOR.Game.Server.Threading;
using SWLOR.Game.Server.ValueObject;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
#pragma warning disable IDE1006 // Naming Styles
    public class mod_on_load
#pragma warning restore IDE1006 // Naming Styles
    {
        // ReSharper disable once UnusedMember.Local
        public static void Main()
        {
            string nowString = DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss");
            Console.WriteLine(nowString + ": Module OnLoad executing...");

            using (new Profiler(nameof(mod_on_load) + ":DatabaseMigrator"))
            {
                DatabaseMigrationRunner.Start();
            }

            using (new Profiler(nameof(mod_on_load) + ":DBBackgroundThread"))
            {
                Console.WriteLine("Starting background thread manager...");
                BackgroundThreadManager.Start();
            }

            using (new Profiler(nameof(mod_on_load) + ":SetEventScripts"))
            {
                NWNXChat.RegisterChatScript("mod_on_nwnxchat");
                SetModuleEventScripts();
                SetAreaEventScripts();
                SetWeaponSettings();

            }
            // Bioware default
            _.ExecuteScript("x2_mod_def_load", _.OBJECT_SELF);

            using (new Profiler(nameof(mod_on_load) + ":RegisterSubscribeEvents"))
            {
                RegisterServiceSubscribeEvents();
            }

            ScriptService.Initialize();
            MessageHub.Instance.Publish(new OnModuleLoad());

            nowString = DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss");
            Console.WriteLine(nowString + ": Module OnLoad finished!");
        }


        private static void RegisterServiceSubscribeEvents()
        {
            // Use reflection to get all of the SubscribeEvents() methods in the SWLOR namespace.
            var typesInNamespace = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(x => x.Namespace != null && 
                            x.Namespace.StartsWith("SWLOR.Game.Server") && // The entire SWLOR namespace
                            !typeof(IScript).IsAssignableFrom(x) && // Exclude scripts
                            x.IsClass) // Classes only.
                .ToArray();
            foreach (var type in typesInNamespace)
            {
                var method = type.GetMethod("SubscribeEvents");
                if (method != null)
                {
                    method.Invoke(null, null);
                }
            }
        }

        private static void SetAreaEventScripts()
        {
            uint area = _.GetFirstArea();
            while (_.GetIsObjectValid(area) == true)
            {
                _.SetEventScript(area, EventScript.Area_OnEnter, "area_on_enter");
                _.SetEventScript(area, EventScript.Area_OnExit, "area_on_exit");
                // Heartbeat events will be set when players enter the area.
                // There's no reason to have them firing if no players are in the area.
                _.SetEventScript(area, EventScript.Area_OnHeartbeat, string.Empty);
                _.SetEventScript(area, EventScript.Area_OnUserDefined, "area_on_user");

                area = _.GetNextArea();
            }
        }


        private static void SetModuleEventScripts()
        {
            // Vanilla NWN Event Hooks
            _.SetEventScript(_.GetModule(), EventScript.Module_OnAcquireItem, "mod_on_acquire");
            _.SetEventScript(_.GetModule(), EventScript.Module_OnActivateItem, "mod_on_activate");
            _.SetEventScript(_.GetModule(), EventScript.Module_OnClientEnter, "mod_on_enter");
            _.SetEventScript(_.GetModule(), EventScript.Module_OnClientExit, "mod_on_leave");
            _.SetEventScript(_.GetModule(), EventScript.Module_OnPlayerCancelCutscene, "mod_on_csabort");
            _.SetEventScript(_.GetModule(), EventScript.Module_OnHeartbeat, "mod_on_heartbeat");
            _.SetEventScript(_.GetModule(), EventScript.Module_OnPlayerChat , "mod_on_chat");
            _.SetEventScript(_.GetModule(), EventScript.Module_OnPlayerDeath, "mod_on_death");
            _.SetEventScript(_.GetModule(), EventScript.Module_OnPlayerDying, "mod_on_dying");
            _.SetEventScript(_.GetModule(), EventScript.Module_OnEquipItem, "mod_on_equip");
            _.SetEventScript(_.GetModule(), EventScript.Module_OnPlayerLevelUp, "mod_on_levelup");
            _.SetEventScript(_.GetModule(), EventScript.Module_OnRespawnButtonPressed, "mod_on_respawn");
            _.SetEventScript(_.GetModule(), EventScript.Module_OnPlayerRest, "mod_on_rest");
            _.SetEventScript(_.GetModule(), EventScript.Module_OnUnequipItem, "mod_on_unequip");
            _.SetEventScript(_.GetModule(), EventScript.Module_OnLoseItem, "mod_on_unacquire");
            _.SetEventScript(_.GetModule(), EventScript.Module_OnUserDefined , "mod_on_user");

            // NWNX Hooks
            NWNXEvents.SubscribeEvent(EventType.StartCombatRoundBefore, "mod_on_attack");
            NWNXEvents.SubscribeEvent(EventType.ExamineObjectBefore, "mod_on_examine");
            NWNXEvents.SubscribeEvent(EventType.UseFeatBefore, "mod_on_usefeat");
            NWNXEvents.SubscribeEvent(EventType.EnterStealthAfter, "mod_on_entstlth");
            NWNXEvents.SubscribeEvent(EventType.DecrementItemStackSizeBefore, "item_dec_stack");
            NWNXEvents.SubscribeEvent(EventType.UseItemBefore, "item_use_before");
            NWNXEvents.SubscribeEvent(EventType.UseItemAfter, "item_use_after");
            NWNXDamage.SetDamageEventScript("mod_on_applydmg");

            // DM Hooks
            NWNXEvents.SubscribeEvent(EventType.DMAppearBefore, "dm_appear");
            NWNXEvents.SubscribeEvent(EventType.DMChangeDifficultyBefore, "dm_change_diff");
            NWNXEvents.SubscribeEvent(EventType.DMDisableTrapBefore, "dm_disab_trap");
            NWNXEvents.SubscribeEvent(EventType.DMDisappearBefore, "dm_disappear");
            NWNXEvents.SubscribeEvent(EventType.DMForceRestBefore, "dm_force_rest");
            NWNXEvents.SubscribeEvent(EventType.DMGetVariableBefore, "dm_get_var");
            NWNXEvents.SubscribeEvent(EventType.DMGiveGoldBefore, "dm_give_gold");
            NWNXEvents.SubscribeEvent(EventType.DMGiveItemBefore, "dm_give_item");
            NWNXEvents.SubscribeEvent(EventType.DMGiveLevelBefore, "dm_give_level");
            NWNXEvents.SubscribeEvent(EventType.DMGiveXPBefore, "dm_give_xp");
            NWNXEvents.SubscribeEvent(EventType.DMHealBefore, "dm_heal");
            NWNXEvents.SubscribeEvent(EventType.DMJumpBefore, "dm_jump");
            NWNXEvents.SubscribeEvent(EventType.DMJumpAllPlayersToPointBefore, "dm_jump_all");
            NWNXEvents.SubscribeEvent(EventType.DMJumpTargetToPointBefore, "dm_jump_target");
            NWNXEvents.SubscribeEvent(EventType.DMKillBefore, "dm_kill");
            NWNXEvents.SubscribeEvent(EventType.DMLimboBefore, "dm_limbo");
            NWNXEvents.SubscribeEvent(EventType.DMPossessBefore, "dm_possess");
            NWNXEvents.SubscribeEvent(EventType.DMSetDateBefore, "dm_set_date");
            NWNXEvents.SubscribeEvent(EventType.DMSetStatBefore, "dm_set_stat");
            NWNXEvents.SubscribeEvent(EventType.DMSetTimeBefore, "dm_set_time");
            NWNXEvents.SubscribeEvent(EventType.DMSetVariableBefore, "dm_set_var");
            NWNXEvents.SubscribeEvent(EventType.DMSpawnCreatureAfter, "dm_spawn_crea");
            NWNXEvents.SubscribeEvent(EventType.DMSpawnEncounterAfter, "dm_spawn_enco");
            NWNXEvents.SubscribeEvent(EventType.DMSpawnItemAfter, "dm_spawn_item");
            NWNXEvents.SubscribeEvent(EventType.DMSpawnPlaceableAfter, "dm_spawn_plac");
            NWNXEvents.SubscribeEvent(EventType.DMSpawnPortalAfter, "dm_spawn_port");
            NWNXEvents.SubscribeEvent(EventType.DMSpawnTrapOnObjectAfter, "dm_spawn_trap");
            NWNXEvents.SubscribeEvent(EventType.DMSpawnTriggerAfter, "dm_spawn_trigg");
            NWNXEvents.SubscribeEvent(EventType.DMSpawnWaypointAfter, "dm_spawn_wayp");
            NWNXEvents.SubscribeEvent(EventType.DMTakeItemBefore, "dm_take_item");
            NWNXEvents.SubscribeEvent(EventType.DMToggleImmortalBefore, "dm_togg_immo");
            NWNXEvents.SubscribeEvent(EventType.DMToggleAIBefore, "dm_toggle_ai");
            NWNXEvents.SubscribeEvent(EventType.DMToggleLockBefore, "dm_toggle_lock");
        }

        private static void SetWeaponSettings()
        {
            NWNXWeapon.SetWeaponFocusFeat(BaseItem.Lightsaber, Feat.EpicWeaponFocus_Longsword);
            NWNXWeapon.SetWeaponFocusFeat(BaseItem.Saberstaff, Feat.WeaponFocus_TwoBladedSword);

            NWNXWeapon.SetWeaponImprovedCriticalFeat(BaseItem.Lightsaber, Feat.ImprovedCritical_LongSword);
            NWNXWeapon.SetWeaponImprovedCriticalFeat(BaseItem.Saberstaff, Feat.ImprovedCritical_TwoBladedSword);

            NWNXWeapon.SetWeaponSpecializationFeat(BaseItem.Lightsaber, Feat.EpicWeaponSpecialization_Longsword);
            NWNXWeapon.SetWeaponSpecializationFeat(BaseItem.Saberstaff, Feat.EpicWeaponSpecialization_Twobladedsword);

            NWNXWeapon.SetWeaponFinesseSize(BaseItem.Lightsaber, CreatureSize.Medium);
            NWNXWeapon.SetWeaponFinesseSize(BaseItem.Saberstaff, CreatureSize.Medium);
            NWNXWeapon.SetWeaponFinesseSize(BaseItem.Longsword, CreatureSize.Medium);

            NWNXWeapon.SetWeaponUnarmed(BaseItem.QuarterStaff);
            NWNXWeapon.SetWeaponUnarmed(BaseItem.Club);
        }


    }
}
