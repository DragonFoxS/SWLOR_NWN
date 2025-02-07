using SWLOR.Game.Server.Enumeration;
using SWLOR.Game.Server.Quest;

namespace SWLOR.Game.Server.Scripts.Quest.GuildTasks.WeaponsmithGuild
{
    public class BasicBatonM: AbstractQuest
    {
        public BasicBatonM()
        {
            CreateQuest(223, "Weaponsmith Guild Task: 1x Basic Baton M", "wpn_tsk_223")
                .IsRepeatable()

                .AddObjectiveCollectItem(1, "mace_b", 1, true)

                .AddRewardGold(35)
                .AddRewardGuildPoints(GuildType.WeaponsmithGuild, 9);
        }
    }
}
