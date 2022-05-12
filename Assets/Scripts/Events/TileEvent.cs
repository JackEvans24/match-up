using UnityEngine;

namespace Assets.Scripts.Events
{
    [CreateAssetMenu(fileName = "New Tile Event", menuName = "Events/Tile Event")]
    public class TileEvent : GameEvent<Tile>
    {
    }
}
