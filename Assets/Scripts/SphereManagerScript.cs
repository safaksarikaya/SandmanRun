using System.Collections.Generic;
using UnityEngine;
public class SphereManagerScript : MonoBehaviour
{
  public static SphereManagerScript Instance;
  public GameObject sphereGameObj;
  public List<SphereScript> sphereActiveObjectList;
  public List<SphereScript> sphereDeactiveObjectList;
  private void Awake()
  {
    Instance = this;
  }
  public void DeactiveteObject(SphereScript sphere)
  {
    sphere.transform.parent = transform;
    sphereActiveObjectList.Remove(sphere);
    sphereDeactiveObjectList.Add(sphere);
    sphere.gameObject.SetActive(false);
  }
  public SphereScript GetSphere()
  {
    if (sphereDeactiveObjectList.Count > 0)
    {
      var sphere = sphereDeactiveObjectList[0];
      sphereDeactiveObjectList.Remove(sphere);
      sphereActiveObjectList.Add(sphere);
      return sphere;
    }
    else
    {
      var sphere = Instantiate(sphereGameObj, transform.position, Quaternion.identity, transform).GetComponent<SphereScript>();
      sphereActiveObjectList.Add(sphere);
      return sphere;
    }
  }
}