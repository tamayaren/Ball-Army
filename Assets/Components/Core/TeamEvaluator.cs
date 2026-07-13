using UnityEngine;

public class TeamEvaluator : MonoBehaviour
{
    public static Color GetTeamColor(Team team)
    {
        switch (team)
        {
            case Team.Blue:
                return new Color(.25f, .45f, 1f);
                break;
            case Team.Red:
            default:
                return new Color(1f, .25f, .15f);
                break;
        }   
    }

}

public enum Team
{
    Red,
    Blue
}
