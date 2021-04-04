using System.Collections.Generic;
using UnityEngine;

public class FogOfWarGraphicsToggle : Observer
{
    [SerializeField] private List<SkinnedMeshRenderer> meshRenderers;

    private void Start()
    {
        FogOfWar.Instance.Attach(this);
    }

    public override void OnNotify(Subject subject)
    {
        FogOfWar fogOfWar = subject as FogOfWar;
        if (fogOfWar.IsEnabled == true)
            if (Map.GetNodeFromPos(transform.position).Visible == true)
                ToggleMeshRenderers(true);
            else
                ToggleMeshRenderers(false);
        else
            ToggleMeshRenderers(true);
    }

    private void ToggleMeshRenderers(bool value)
    {
        foreach (SkinnedMeshRenderer meshRenderer in meshRenderers)
        {
            meshRenderer.enabled = value;
        }
    }
}
