using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ComboActivator : BaseSceneActivatable
{
    public ComboActivatable[] activatables;
    List<Activator> activators = new List<Activator>();
    bool comboActivated = false;
    public UnityEvent comboActivateEvent = new UnityEvent();

    public bool[] activated;

    public override void activateModifiers()
    {
        foreach (var activatable in activatables)
        {
            activatable.initialize(this);
            activators.Add(new Activator(activatable));
        }
        activated = new bool[activatables.Length];
    }

    public void updateCombo(ComboActivatable comboUpdate, bool active)
    {
        if (sceneActive && comboActivated) return;
        var activatable = activators.Find(x => x.comboActivatable == comboUpdate);
        if (activatable != null) activatable.activated = active;
        checkCombo();
    }

    void checkCombo()
    {
        if (comboActivated) return;
        bool allTrue = true;
        for (int i = 0; i < activators.Count; i++)
        {
            activated[i] = activators[i].activated;
            if (allTrue && !activators[i].activated) allTrue = false;
        }
        if (!allTrue) return;
        comboActivated = true;
        comboActivateEvent.Invoke();
        activateNextScene();
    }
}

class Activator
{
    public Activator(ComboActivatable activatable)
    {
        comboActivatable = activatable;
        activated = false;
    }
    public ComboActivatable comboActivatable;
    public bool activated;
}