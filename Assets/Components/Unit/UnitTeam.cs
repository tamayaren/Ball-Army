using UnityEngine;

public class UnitTeam : MonoBehaviour
{
    public Team team
    {
        get => this._team;
        set
        {
            this._team = value;
            ChangeColor();
        }
    }
    
    [SerializeField] private Team _team;

    private void ChangeColor()
    {
        Color teamColor = TeamEvaluator.GetTeamColor(this.team);
        foreach (Transform bodyPart in GetComponentsInChildren<Transform>())
        {
            if (!bodyPart.CompareTag($"BodyPart")) continue;
            bodyPart.TryGetComponent(out Renderer bodyRenderer);
                
            if (bodyRenderer != null)
                bodyRenderer.material.color = teamColor;
        }
    }
    
    private void Start()
    {
        ChangeColor();
    }
}