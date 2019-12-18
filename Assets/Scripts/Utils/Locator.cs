using Units;
using UnityEngine;
using  System.Linq;
using Enums;

namespace Utils
{
    public class Locator : MonoBehaviour
    {
        public static Transform GetClosestPlayerUnit(Vector3 currentLocation)
        {
            var sortedPlayers = FindObjectsOfType<Unit>()
                .Where(unit => unit.Owner.ControlType != ControlType.AI)
                ?.OrderBy(playerUnit => Vector3.Distance(currentLocation, playerUnit.transform.position))
                ?.ToList();

            return sortedPlayers[0].transform;
        }
    }
}