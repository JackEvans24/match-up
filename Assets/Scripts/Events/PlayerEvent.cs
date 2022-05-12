using UnityEngine;

namespace Assets.Scripts.Events
{
    [CreateAssetMenu(fileName = "New Player Event", menuName = "Events/Player Event")]
    public class PlayerEvent : GameEvent<Player>
    {
    }
}
