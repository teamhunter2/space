using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Object = UnityEngine.Object;

[System.Serializable]
public class LevelPath {
  public int id;
  public string name;
  public int[] pointsOfInterest;
}

[System.Serializable]
public class LevelPointOfInterest {
  public int id;
  public string name;
  public Vector2 mapPosition;
  public int[] paths;
}

[System.Serializable]
public class Levels {
  public LevelPath[] paths;
  public LevelPointOfInterest[] pointsOfInterest;
}

public class LevelSelectExplorer : MonoBehaviour {
  private const string levelsFileName = "levels.json";

  public Object nodePrefab;
  public Object edgePrefab;

  public Levels levelsData;

  private Dictionary<int, GraphNode> nodes = new Dictionary<int, GraphNode>();
  private Dictionary<int, GraphEdge> edges = new Dictionary<int, GraphEdge>();

  private void Awake() {
    nodePrefab = Resources.Load("Prefabs/GraphNode");
    edgePrefab = Resources.Load("Prefabs/GraphEdge");

    LoadLevels();
  }

  private void Start() {
    InstantiateNodes();
  }

  private void LoadLevels() {
    string filePath = Path.Combine(Application.dataPath, levelsFileName);

    if (File.Exists(filePath)) {
      string levelsAsJson = File.ReadAllText(filePath);

      levelsData = JsonUtility.FromJson<Levels>(levelsAsJson);
    } else {
      Debug.LogError("Cannot load game data!");
    }
  }

  private void InstantiateNodes() {
    foreach (LevelPointOfInterest pointOfInterest in levelsData.pointsOfInterest) {
      GraphNode node = ((GameObject) Instantiate(nodePrefab, transform)).GetComponent<GraphNode>();

      node.name = pointOfInterest.name;
      node.transform.localPosition = pointOfInterest.mapPosition;

      nodes.Add(pointOfInterest.id, node);
    }

    foreach (LevelPath path in levelsData.paths) {
      GraphEdge edge = ((GameObject) Instantiate(edgePrefab, transform)).GetComponent<GraphEdge>();

      edge.name = path.name;
      edge.graphNodes[0] = nodes[path.pointsOfInterest[0]];
      edge.graphNodes[1] = nodes[path.pointsOfInterest[1]];

      edges.Add(path.id, edge);
    }

//    edges.Add((GraphEdge) Instantiate(edgePrefab));
  }
}
