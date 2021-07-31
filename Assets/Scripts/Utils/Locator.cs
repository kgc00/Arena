using Units;
using UnityEngine;
using  System.Linq;
using Data.Types;

namespace Utils
{
    public class Locator : MonoBehaviour
    {
        public static Transform GetClosestPlayerUnit(Vector3 currentLocation)
        {
            var sortedPlayers = FindObjectsOfType<Unit>()
                .Where(unit => unit.Owner != null && unit.Owner.ControlType != ControlType.Ai)
                ?.OrderBy(playerUnit => Vector3.Distance(currentLocation, playerUnit.transform.position))
                ?.ToList();

            
            return sortedPlayers.Count > 0 ? sortedPlayers[0].transform : null;
        }
        
        public static Transform GetClosestVisiblePlayerUnit(Vector3 currentLocation)
        {
            var sortedPlayers = FindObjectsOfType<Unit>()
                .Where(unit => unit.Owner.ControlType != ControlType.Ai && unit.StatusComponent.IsVisible())
                ?.OrderBy(playerUnit => Vector3.Distance(currentLocation, playerUnit.transform.position))
                ?.ToList();
            
            return sortedPlayers.Count > 0 ? sortedPlayers[0].transform : null;
        }
    }
}