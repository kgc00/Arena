using Units;
using UnityEngine;
using  System.Linq;
using Enums;
using Stats;

namespace Utils
{
    public class Locator : MonoBehaviour
    {
        public static Transform GetClosestPlayerUnit(Vector3 currentLocation)
        {
            var sortedPlayers = FindObjectsOfType<Unit>()
                .Where(unit => unit.Owner.ControlType != ControlType.Ai)
                ?.OrderBy(playerUnit => Vector3.Distance(currentLocation, playerUnit.transform.position))
                ?.ToList();

            
            return sortedPlayers.Count > 0 ? sortedPlayers[0].transform : null;
        }
        
        public static Transform GetClosestVisiblePlayerUnit(Vector3 currentLocation)
        {
            var sortedPlayers = FindObjectsOfType<Unit>()
                .Where(unit => unit.Owner.ControlType != ControlType.Ai && !unit.StatusComponent.Status.HasFlag(Status.Hidden))
                ?.OrderBy(playerUnit => Vector3.Distance(currentLocation, playerUnit.transform.position))
                ?.ToList();
            
            return sortedPlayers.Count > 0 ? sortedPlayers[0].transform : null;
        }
    }
}