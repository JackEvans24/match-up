using UnityEngine;

namespace Assets.Scripts.Events
{
    [CreateAssetMenu(fileName = "New Score Event", menuName = "Events/Score Event")]
    public class ScoreEvent : GameEvent<ScoreType>
    {
    }
}
