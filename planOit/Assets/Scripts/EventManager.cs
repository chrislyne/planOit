using UnityEngine;

namespace planOit
{
    public enum PlanetEventType
    {
        UNSET = 0,
        NOTHING,
        NOTHING2,
        NOTHING_OF_NOTE,
        BONUS_RESOURCES,
        BONUS_RESOURCES2,
        RESOURCE_PENALTY,
        ALIEN_ATTACK,
        COUNT,
        CUSTOM
    }

    public class EventManager
    {
        public static PlanetEventType getRandomEvent()
        {
            return (PlanetEventType)Random.Range(0, (int)PlanetEventType.COUNT);
        }
    }
}