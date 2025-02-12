﻿using SWLOR.Game.Server.Enumeration;
using SWLOR.Game.Server.GameObject;

using SWLOR.Game.Server.NWN;
using SWLOR.Game.Server.NWN.Enum;
using SWLOR.Game.Server.Service;


namespace SWLOR.Game.Server.Perk.Shields
{
    public class ExpulsionManeuver : IPerkHandler
    {
        public PerkType PerkType => PerkType.ExpulsionManeuver;

        public string CanCastSpell(NWCreature oPC, NWObject oTarget, int spellTier)
        {
            return string.Empty;
        }
        
        public int FPCost(NWCreature oPC, int baseFPCost, int spellTier)
        {
            return baseFPCost;
        }

        public float CastingTime(NWCreature oPC, float baseCastingTime, int spellTier)
        {
            return baseCastingTime;
        }

        public float CooldownTime(NWCreature oPC, float baseCooldownTime, int spellTier)
        {
            return baseCooldownTime;
        }

        public int? CooldownCategoryID(NWCreature creature, int? baseCooldownCategoryID, int spellTier)
        {
            return baseCooldownCategoryID;
        }

        public void OnImpact(NWCreature creature, NWObject target, int perkLevel, int spellTier)
        {
            float length;
            int ab;
            int chance;

            switch (perkLevel)
            {
                case 1:
                    length = 12.0f;
                    ab = 1;
                    chance = 10;
                    break;
                case 2:
                    length = 12.0f;
                    ab = 1;
                    chance = 20;
                    break;
                case 3:
                    length = 12.0f;
                    ab = 2;
                    chance = 20;
                    break;
                case 4:
                    length = 12.0f;
                    ab = 2;
                    chance = 30;
                    break;
                case 5:
                    length = 12.0f;
                    ab = 3;
                    chance = 30;
                    break;
                default:
                    return;
            }

            if (creature.IsPlayer)
            {
                var effectiveStats = PlayerStatService.GetPlayerItemEffectiveStats(creature.Object);
                int luck = PerkService.GetCreaturePerkLevel(creature, PerkType.Lucky) + effectiveStats.Luck;
                chance += luck;
            }

            if (RandomService.Random(100) + 1 <= chance)
            {
                _.ApplyEffectToObject(DurationType.Temporary, _.EffectAttackIncrease(ab), creature.Object, length);
                creature.SendMessage(ColorTokenService.Combat("You perform a defensive maneuver."));
            }
        }

        public void OnPurchased(NWCreature creature, int newLevel)
        {
        }

        public void OnRemoved(NWCreature creature)
        {
        }

        public void OnItemEquipped(NWCreature creature, NWItem oItem)
        {
        }

        public void OnItemUnequipped(NWCreature creature, NWItem oItem)
        {
        }

        public void OnCustomEnmityRule(NWCreature creature, int amount)
        {
        }

        public bool IsHostile()
        {
            return false;
        }

        public void OnConcentrationTick(NWCreature creature, NWObject target, int perkLevel, int tick)
        {
            
        }
    }
}
