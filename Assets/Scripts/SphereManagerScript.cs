using System.Collections.Generic;
using UnityEngine;

public class SphereManagerScript : MonoBehaviour
{
  public static SphereManagerScript Instance;
  public GameObject sphereGameObj;
  public List<GameObject> sphereActiveObjectList;
  public List<GameObject> sphereDeactiveObjectList;
  private void Awake()
  {
    Instance = this;
  }
  public void DeactiveteObject(GameObject sphere)
  {
    sphereActiveObjectList.Remove(sphere);
    sphereDeactiveObjectList.Add(sphere);
    sphere.SetActive(false);
  }
}