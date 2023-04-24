using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisitationGoal : Quest.QuestGoal
{
    public string scene;

    public override string GetDescription()
    {
        return $"Visit {scene}";
    }

    public override void Initialize()
    {
        base.Initialize();
        EventManager.Instance.AddListener<VisitGameEvent>(OnVisit);
    }

    private void OnVisit(VisitGameEvent ge)
    {
        if (ge.scene == scene)
        {
            CurrentAmount++;
            Evaluate();
        }
    }
}
