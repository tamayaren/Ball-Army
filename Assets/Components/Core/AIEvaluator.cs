using JetBrains.Annotations;
using UnityEngine;

public class AIEvaluator : MonoBehaviour
{
    public static AIEvaluator instance;

    private void Awake() => instance = this;
    
    [CanBeNull]
    public Transform FindTarget(UnitTeam team, float range)
    {
        Transform originTransform = team.transform;
        Team opposingTeam = team.team == Team.Red ? Team.Blue : Team.Red;
        
        UnitTeam bestTarget = null;
        foreach (GameObject unit in GameObject.FindGameObjectsWithTag($"Unit"))
        {
            Transform unitTransform = unit.transform;

            unit.TryGetComponent(out Humanoid unitHumanoid);
            unit.TryGetComponent(out UnitTeam unitTeam);
            if (!unitTeam) continue;
            if (!unitHumanoid) continue;
            if (unitHumanoid.state == HumanoidState.Dead) continue;
            if (unitTeam.team != opposingTeam) continue;
            
            float distance = Vector3.Distance(unitTransform.position, originTransform.position);
            if (distance <= range)
            {
                if (bestTarget)
                {
                    float bestTargetDistance = Vector3.Distance(bestTarget.transform.position, originTransform.position);
                    
                    if (bestTargetDistance > distance)
                        bestTarget = unitTeam;
                    continue;
                }
                
                bestTarget = unitTeam;
            }
        }

        return bestTarget?.transform;
    }
}
