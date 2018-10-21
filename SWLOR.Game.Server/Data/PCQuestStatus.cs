//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SWLOR.Game.Server.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class PCQuestStatus
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PCQuestStatus()
        {
            this.PCQuestItemProgresses = new HashSet<PCQuestItemProgress>();
            this.PCQuestKillTargetProgresses = new HashSet<PCQuestKillTargetProgress>();
        }
    
        public int PCQuestStatusID { get; set; }
        public string PlayerID { get; set; }
        public int QuestID { get; set; }
        public int CurrentQuestStateID { get; set; }
        public Nullable<System.DateTime> CompletionDate { get; set; }
        public Nullable<int> SelectedItemRewardID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PCQuestItemProgress> PCQuestItemProgresses { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PCQuestKillTargetProgress> PCQuestKillTargetProgresses { get; set; }
        public virtual QuestState CurrentQuestState { get; set; }
        public virtual PlayerCharacter PlayerCharacter { get; set; }
        public virtual Quest Quest { get; set; }
        public virtual QuestRewardItem SelectedQuestRewardItem { get; set; }
    }
}