﻿using System;
using SWLOR.Game.Server.NWN;
using SWLOR.Game.Server.Enumeration;
using SWLOR.Game.Server.GameObject;
using SWLOR.Game.Server.NWN.Enum;
using SWLOR.Game.Server.NWN.Enum.Creature;
using SWLOR.Game.Server.NWN.Enum.VisualEffect;
using static SWLOR.Game.Server.NWN._;
using SWLOR.Game.Server.Service;

namespace SWLOR.Game.Server.Perk.ForceSense
{
    public class BattleInsight: IPerkHandler
    {
        public PerkType PerkType => PerkType.BattleInsight;
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
            const float MaxDistance = 5.0f;
            int nth = 1;
            int amount;

            switch (perkLevel)
            {
                case 1:
                    amount = 5;
                    break;
                case 2:
                case 3:
                    amount = 10;
                    break;
                default: return;
            }

            // Penalize the caster
            Effect effect = _.EffectACDecrease(amount);
            effect = _.EffectLinkEffects(effect, _.EffectAttackDecrease(amount));
            ApplyEffectToObject(DurationType.Temporary, effect, creature, 6.1f);


            NWCreature targetCreature = _.GetNearestCreature(CreatureType.IsAlive, 1, creature, nth);
            while (targetCreature.IsValid && GetDistanceBetween(creature, targetCreature) <= MaxDistance)
            {
                // Skip the caster, if they get picked up.
                if (targetCreature == creature)
                {
                    nth++;
                    targetCreature = _.GetNearestCreature(CreatureType.IsAlive, 1, creature, nth);
                    continue;
                }

                // Handle effects for differing spellTier values
                switch (perkLevel)
                {
                    case 1:
                        amount = 5;

                        if (_.GetIsReactionTypeHostile(targetCreature, creature) == true)
                        {
                            nth++;
                            targetCreature = _.GetNearestCreature(CreatureType.IsAlive, 1, creature, nth);
                            continue;        
                        }                            
                        break;
                    case 2:
                        amount = 10;

                        if (_.GetIsReactionTypeHostile(targetCreature, creature) == true)
                        {
                            nth++;
                            targetCreature = _.GetNearestCreature(CreatureType.IsAlive, 1, creature, nth);
                            continue;
                        }
                        break;
                    case 3:
                        amount = 10;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(perkLevel));
                }

                if (_.GetIsReactionTypeHostile(targetCreature, creature) == true)
                {
                    effect = _.EffectACDecrease(amount);
                    effect = _.EffectLinkEffects(effect, _.EffectAttackDecrease(amount));
                }
                else
                {
                    effect = _.EffectACIncrease(amount);
                    effect = _.EffectLinkEffects(effect, _.EffectAttackIncrease(amount));
                }

                _.ApplyEffectToObject(DurationType.Temporary, effect, targetCreature, 6.1f);
                _.ApplyEffectToObject(DurationType.Instant, _.EffectVisualEffect(VisualEffect.Vfx_Dur_Magic_Resistance), targetCreature);

                if (creature.IsPlayer)
                {
                    SkillService.RegisterPCToAllCombatTargetsForSkill(creature.Object, SkillType.ForceSense, null);
                }

                nth++;
                targetCreature = _.GetNearestCreature(CreatureType.IsAlive, 1, creature, nth);
            }
            
        }
    }
}
