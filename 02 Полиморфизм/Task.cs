using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Inheritance.MapObjects
{
    public interface IOwnerable   { int Owner { set; get; } }
    public interface IArmyable    { Army Army { get; set; } }
    public interface ITreasurable { Treasure Treasure { get; set; } }

    public class Dwelling : IOwnerable
    {
        public int Owner { get; set; }
    }

    public class Mine : IOwnerable, IArmyable, ITreasurable
    {
        public int Owner { get; set; }
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }
    }

    public class Creeps : IArmyable, ITreasurable
    {
        public Army Army { get; set; }
        public Treasure Treasure { get; set; }
    }

    public class Wolves : IArmyable
    {
        public Army Army { get; set; }
    }

    public class ResourcePile : ITreasurable
    {
        public Treasure Treasure { get; set; }
    }

    public static class Interaction
    {
        public static void Make(Player player, object mapObject)
        {
            if (mapObject is IArmyable army && !player.CanBeat(army.Army))
            {
                player.Die();
                return;
            }
            if (mapObject is IOwnerable onwer) 
                onwer.Owner = player.Id;

            if(mapObject is ITreasurable treasure)
                player.Consume(treasure.Treasure);
        }
    }
}
