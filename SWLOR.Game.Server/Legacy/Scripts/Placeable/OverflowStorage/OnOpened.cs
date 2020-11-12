﻿using SWLOR.Game.Server.Core.NWScript;
using SWLOR.Game.Server.Legacy.GameObject;
using SWLOR.Game.Server.Legacy.Service;

namespace SWLOR.Game.Server.Legacy.Scripts.Placeable.OverflowStorage
{
    public class OnOpened: IScript
    {
        public void SubscribeEvents()
        {
        }

        public void UnsubscribeEvents()
        {
        }

        public void Main()
        {
            NWPlaceable container = (NWScript.OBJECT_SELF);
            NWPlayer oPC = (NWScript.GetLastOpenedBy());
            var items = DataService.PCOverflowItem.GetAllByPlayerID(oPC.GlobalID);
            foreach (var item in items)
            {
                var oItem = SerializationService.DeserializeItem(item.ItemObject, container);
                oItem.SetLocalString("TEMP_OVERFLOW_ITEM_ID", item.ID.ToString());
            }

            container.IsUseable = false;
        }
    }
}