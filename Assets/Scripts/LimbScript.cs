using System.Collections.Generic;
using UnityEngine;

public class LimbScript : MonoBehaviour
{
  public int activeItemCount;
  public int totalItemCount;
  private List<SphereScript> activeSphereList = new List<SphereScript>();
  private List<SphereScript> deactiveSphereList = new List<SphereScript>();
  private void Start()
  {
    for (int i = 0; i < transform.childCount; i++)
    {
      activeSphereList.Add(transform.GetChild(i).GetComponent<SphereScript>());
    }
    activeItemCount = activeSphereList.Count;
    totalItemCount = activeItemCount;
  }
  public void ActivateItem(SphereScript sphereScript)
  {
    if (deactiveSphereList.Contains(sphereScript))
      deactiveSphereList.Remove(sphereScript);
    activeSphereList.Add(sphereScript);
    sphereScript.gameObject.SetActive(true);
    activeItemCount++;
  }
  public void DeactivateItem(SphereScript sphere)
  {
    activeItemCount--;
    if (activeItemCount == 0)
    {
      CharacterScript.Instance.ControlAnimation();
    }
    activeSphereList.Remove(sphere);
    deactiveSphereList.Add(sphere);
  }
  public SphereScript GetFreeSphere()
  {
    deactiveSphereList.Sort((p1, p2) => p2.transform.position.y.CompareTo(p1.transform.position.y));
    var sphere = deactiveSphereList[0];
    ActivateItem(sphere);
    return sphere;
  }
  public float GetMinYItem()
  {
    var y = 100f;
    if (activeSphereList.Count > 0)
    {
      activeSphereList.Sort((p1, p2) => p2.startYPosition.CompareTo(p1.startYPosition));
      y = activeSphereList[0].startYPosition;
    }
    return y;
  }
}