using SWLOR.Game.Server;
using SWLOR.Game.Server.Event.Conversation;
using static NWN.NWScript;

// ReSharper disable once CheckNamespace
namespace NWN.Scripts
{
#pragma warning disable IDE1006 // Naming Styles
    internal class on_qst7_state_3
#pragma warning restore IDE1006 // Naming Styles
    {
        public static int Main()
        {
            return App.RunEvent<QuestCheckState>(7, 3) ? TRUE : FALSE;
        }
    }
}