using SWLOR.Game.Server.Enumeration;
using SWLOR.Game.Server.Quest;

namespace SWLOR.Game.Server.Scripts.Quest.GuildTasks.ArmorsmithGuild
{
    public class HeavyArmorCore: AbstractQuest
    {
        public HeavyArmorCore()
        {
            CreateQuest(117, "Armorsmith Guild Task: 1x Heavy Armor Core", "arm_tsk_117")
                .IsRepeatable()

                .AddObjectiveCollectItem(1, "core_h_armor", 1, true)

                .AddRewardGold(20)
                .AddRewardGuildPoints(GuildType.ArmorsmithGuild, 5);
        }
    }
}
