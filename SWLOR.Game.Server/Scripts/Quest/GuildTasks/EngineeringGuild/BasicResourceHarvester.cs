using SWLOR.Game.Server.Enumeration;
using SWLOR.Game.Server.Quest;

namespace SWLOR.Game.Server.Scripts.Quest.GuildTasks.EngineeringGuild
{
    public class BasicResourceHarvester: AbstractQuest
    {
        public BasicResourceHarvester()
        {
            CreateQuest(352, "Engineering Guild Task: 1x Basic Resource Harvester", "eng_tsk_352")
                .IsRepeatable()

                .AddObjectiveCollectItem(1, "harvest_r_b", 1, true)

                .AddRewardGold(60)
                .AddRewardGuildPoints(GuildType.EngineeringGuild, 15);
        }
    }
}
