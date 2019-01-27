using System;
using System.Linq;
using UnityEngine;

public class GraphEdge : MonoBehaviour {
  public GraphNode[] graphNodes = new GraphNode[2];

  private void OnValidate() {
    if (graphNodes.Length == 2) return;

    Debug.LogWarning("Don't change the 'Graph Nodes' field's array size!");
    Array.Resize(ref graphNodes, 2);
  }

  private Transform firstNodeTransform;
  private Transform lastNodeTransform;

  public float lowestNodePositionX;
  public float highestNodePositionX;
  public float lowestNodePositionY;
  public float highestNodePositionY;

  public Vector2 catheti;
  public float shortestCathetus;
  public float longestCathetus;
  public float hypotenuse;

  private void Start() {
    firstNodeTransform = graphNodes.First().transform;
    lastNodeTransform = graphNodes.Last().transform;

    CalculateTransform();
  }

  private void Update() {
    CalculateTransform();
  }

  private void CalculateTransform() {
    Vector2 firstNodePosition = firstNodeTransform.localPosition;
    Vector2 lastNodePosition = lastNodeTransform.localPosition;
    var firstNodeScale = firstNodeTransform.localScale;
    var lastNodeScale = lastNodeTransform.localScale;

    lowestNodePositionX = Math.Min(firstNodePosition.x, lastNodePosition.x);
    highestNodePositionX = Math.Max(firstNodePosition.x, lastNodePosition.x);
    lowestNodePositionY = Math.Min(firstNodePosition.y, lastNodePosition.y);
    highestNodePositionY = Math.Max(firstNodePosition.y, lastNodePosition.y);

    catheti = new Vector2(
      highestNodePositionX - lowestNodePositionX,
      highestNodePositionY - lowestNodePositionY
    );
    hypotenuse = (float) Math.Sqrt(Math.Pow(catheti.x, 2) + Math.Pow(catheti.y, 2));

    Vector2 nodePositionDiff = firstNodePosition - lastNodePosition;
    nodePositionDiff.Normalize();
    float edgeRotationDegrees = Mathf.Atan2(nodePositionDiff.y, nodePositionDiff.x) * Mathf.Rad2Deg;

    Console.WriteLine(edgeRotationDegrees);

    var buttonLength = hypotenuse - Math.Max(firstNodeScale.x, firstNodeScale.y) / 2 - Math.Max(lastNodeScale.x, lastNodeScale.y) / 2 - 16;

    var edgeScale = new Vector3(buttonLength, transform.localScale.y, transform.localScale.z);

    var edgePosition = new Vector2(lowestNodePositionX + catheti.x / 2, lowestNodePositionY + catheti.y / 2);

    var edgeRotation = new Vector3(0, 0, edgeRotationDegrees);

    transform.localScale = edgeScale;
    transform.localPosition = edgePosition;
    transform.localEulerAngles = edgeRotation;
  }
}
