using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour {

    public static FogOfWar Instance;

    private Color32 visibleColor = new Color32(0, 0, 0, 0);
    private Color32 visitedColor = new Color32(0, 0, 0, 127);
    private Color32 notVisitedColor = new Color32(0, 0, 0, 255);
    private Texture2D fogOfWarTexture2D = default;
    private Renderer fogOfWarRenderer = default;

    private List<FogOfWarAgent> agents = new List<FogOfWarAgent>();

    public void Initialize() {
        if (Instance != null && Instance != this) {
            Debug.LogError($"There can be only one instance of {ToString()} script!");
            Destroy(this);
        }

        fogOfWarRenderer = GetComponent<Renderer>();
        Instance = this;
    }

    private void LateUpdate() {
        UpdateDataFromAgents();
        GenerateFOWTexture2D();
        SetFOWTexture2DToMaterial();
    }

    public void AddAgent(FogOfWarAgent agent) {
        if (agents.Contains(agent) == false)
            agents.Add(agent);
    }

    public void RemoveAgent(FogOfWarAgent agent) {
        if (agents.Contains(agent) == true)
            agents.Add(agent);
    }

    private void UpdateDataFromAgents() {
        foreach (FogOfWarAgent agent in agents) {
            agent.SetNodesInRadius(false);
        }
        foreach (FogOfWarAgent agent in agents) {
            agent.UpdateNodesInRadius();
        }
        foreach (FogOfWarAgent agent in agents) {
            agent.SetNodesInRadius(true);
        }
    }

    private void GenerateFOWTexture2D() {
        fogOfWarTexture2D = new Texture2D(Map.Instance.MapSize, Map.Instance.MapSize);
        for (int y = 0; y < Map.Instance.MapSize; y++) {
            for (int x = 0; x < Map.Instance.MapSize; x++) {
                Color32 color;
                Node currentNode = Map.Instance.Grid[x, y];
                if (currentNode.Visible)
                    color = visibleColor;
                else if (currentNode.Visited)
                    color = visitedColor;
                else
                    color = notVisitedColor;

                fogOfWarTexture2D.SetPixel(x, y, color);
            }
        }
        fogOfWarTexture2D.Apply();
    }

    private void SetFOWTexture2DToMaterial() {
        fogOfWarRenderer.material.SetTexture("_MainTex", fogOfWarTexture2D);
    }

}
