﻿using System.Linq;
using SWLOR.Game.Server.Core.NWScript;
using SWLOR.Game.Server.Core.NWScript.Enum;
using SWLOR.Game.Server.Core.NWScript.Enum.Item;
using SWLOR.Game.Server.Legacy.GameObject;
using SWLOR.Game.Server.Legacy.Service;

namespace SWLOR.Game.Server.Legacy.Scripts.Placeable.MolecularReassembler
{
    public class OnDisturbed: IScript
    {
        public void SubscribeEvents()
        {
        }

        public void UnsubscribeEvents()
        {
        }

        public void Main()
        {
            if (NWScript.GetInventoryDisturbType() != DisturbType.Added)
                return;
            
            NWPlayer player = NWScript.GetLastDisturbed();
            NWPlaceable device = NWScript.OBJECT_SELF;
            NWItem item = NWScript.GetInventoryDisturbItem();

            // Check the item type to see if it's valid.
            if (!IsValidItemType(item))
            {
                ItemService.ReturnItem(player, item);
                player.SendMessage("You cannot reassemble this item.");
                return;
            }

            // Only crafted items can be reassembled.
            if (string.IsNullOrWhiteSpace(item.GetLocalString("CRAFTER_PLAYER_ID")))
            {
                ItemService.ReturnItem(player, item);
                player.SendMessage("Only crafted items may be reassembled.");
                return;
            }

            // DMs cannot reassemble because they don't have the necessary DB records.
            if (player.IsDM)
            {
                ItemService.ReturnItem(player, item);
                player.SendMessage("DMs cannot reassemble items at this time.");
                return;
            }

            // Serialize the item into a string and store it into the temporary data for this player. Destroy the physical item.
            var model = CraftService.GetPlayerCraftingData(player);
            model.SerializedSalvageItem = SerializationService.Serialize(item);
            item.Destroy();

            // Start the Molecular Reassembly conversation.
            DialogService.StartConversation(player, device, "MolecularReassembly");
        }

        private bool IsValidItemType(NWItem item)
        {
            var type = item.BaseItemType;
            BaseItem[] validTypes =
            {
                BaseItem.Dart,
                BaseItem.ThrowingAxe,
                BaseItem.Shuriken,
                BaseItem.ShortSword,
                BaseItem.Longsword,
                BaseItem.BattleAxe,
                BaseItem.BastardSword,
                BaseItem.LightFlail,
                BaseItem.WarHammer,
                BaseItem.Cannon,
                BaseItem.Rifle,
                BaseItem.Longbow,
                BaseItem.LightMace,
                BaseItem.Halberd,
                BaseItem.Pistol,
                BaseItem.TwoBladedSword,
                BaseItem.GreatSword,
                BaseItem.SmallShield,
                BaseItem.Armor,
                BaseItem.Helmet,
                BaseItem.GreatAxe,
                BaseItem.Amulet,
                BaseItem.Belt,
                BaseItem.Dagger,
                BaseItem.Boots,
                BaseItem.Club,
                BaseItem.DireMace,
                BaseItem.DoubleAxe,
                BaseItem.HeavyFlail,
                BaseItem.Gloves,
                BaseItem.LightHammer,
                BaseItem.HandAxe,
                BaseItem.Kama,
                BaseItem.Katana,
                BaseItem.Kukri,
                BaseItem.MorningStar,
                BaseItem.QuarterStaff,
                BaseItem.Rapier,
                BaseItem.Ring,
                BaseItem.Scimitar,
                BaseItem.Scythe,
                BaseItem.LargeShield,
                BaseItem.TowerShield,
                BaseItem.ShortSpear,
                BaseItem.Sickle,
                BaseItem.Sling,
                BaseItem.ThrowingAxe,
                BaseItem.Bracer,
                BaseItem.Cloak,
                BaseItem.Trident,
                BaseItem.DwarvenWarAxe,
                BaseItem.Whip,
                BaseItem.Lightsaber,
                BaseItem.Saberstaff
            };

            return validTypes.Contains(type);
        }
        
    }
}