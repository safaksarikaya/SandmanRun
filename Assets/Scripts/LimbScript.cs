using System.Collections.Generic;
using UnityEngine;

public class LimbScript : MonoBehaviour
{
  public int activeItemCount;
  public int totalItemCount;
  private List<Vector3> localPositionList = new List<Vector3>();
  private List<bool> isActiveList = new List<bool>();
  private void Start()
  {
    var count = transform.childCount;
    for (int i = 0; i < count; i++)
    {
      localPositionList.Add(transform.GetChild(i).transform.localPosition);
      isActiveList.Add(true);
    }
    activeItemCount = isActiveList.Count;
    totalItemCount = activeItemCount;
  }
  public void DeactivateItem(Vector3 pos)
  {
    var index = localPositionList.IndexOf(pos);
    isActiveList[index] = false;
    activeItemCount--;
    if (activeItemCount == 0)
    {
      CharacterScript.Instance.ControlAnimation();
    }
  }
  public Vector3 GetFreePos()
  {
    var count = isActiveList.Count;
    var position = Vector3.zero;
    for (int i = 0; i < count; i++)
    {
      if (isActiveList[i] == false)
      {
        isActiveList[i] = true;
        position = localPositionList[i];
        break;
      }
    }
    return position;
  }
}