using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePatrolPath : MonoBehaviour {
  public float positionRange = 20.0f;
  public float defaultSideLength = 5.0f;
  public float yPosition = 1.0f;

  /**
   * Genrate the patrol path from a random rectangle
   * by selecting points on its side randomly.
   * @return {List<Vector2>} selected points.
   */
  public List<Vector3> GetRandomRect(int sides = 4, float sideLength = 0) {
    List<Vector3> rect = new List<Vector3>();

    if (sideLength == 0) {
      sideLength = defaultSideLength;
    }

    sideLength = Random.Range(10f, 20f);
    Vector3 leftDown = new Vector3(
        Random.Range(-positionRange, positionRange), yPosition, Random.Range(-positionRange, positionRange));
    Vector3 rightDown = leftDown + Vector3.right * sideLength;
    Vector3 rightUp = leftDown + Vector3.forward * sideLength;
    Vector3 leftUp = rightDown + Vector3.forward * sideLength;

    Vector3 temp = leftDown + Vector3.forward * sideLength * Random.Range(0f, 1f);
    rect.Add(temp);
    temp = leftUp + Vector3.right * sideLength * Random.Range(0f, 1f);
    rect.Add(temp);
    temp = rightUp + Vector3.forward * sideLength * Random.Range(0f, 1f);
    rect.Add(temp);

    if (sides >= 4) {
      temp = rightDown + Vector3.right * (-sideLength) * Random.Range(0f, 0.5f);
      rect.Add(temp);
      if (sides == 5) {
        temp = rightDown + Vector3.right * (-sideLength) * Random.Range(0f, 0.5f);
        rect.Add(temp);
      }
    }
    return rect;
  }
}
