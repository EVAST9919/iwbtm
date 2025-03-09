using System.Collections.Generic;

namespace IWBTM.Game.BossCreation;

public class Composition : ActiveObject
{
    public List<ActiveObject> Children { get; set; } = new();
}
