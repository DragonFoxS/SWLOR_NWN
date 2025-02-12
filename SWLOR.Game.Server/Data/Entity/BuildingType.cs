using Dapper.Contrib.Extensions;
using SWLOR.Game.Server.Data.Contracts;

namespace SWLOR.Game.Server.Data.Entity
{
    [Table("BuildingType")]
    public class BuildingType: IEntity
    {
        [ExplicitKey]
        public int ID { get; set; }
        public string Name { get; set; }

        public IEntity Clone()
        {
            return new BuildingType
            {
                ID = ID,
                Name = Name
            };
        }
    }
}
