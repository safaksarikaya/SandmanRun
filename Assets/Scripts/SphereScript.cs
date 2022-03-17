using UnityEngine;
public class SphereScript : MonoBehaviour
{
  [SerializeField] Rigidbody rb;
  [SerializeField] LimbScript limbScript;
  private Vector3 gameStartLocalPosition;
  private Vector3 startLocalPosition, endLocalPosition;
  private float localPositionCounter;
  private bool goLocalPosition;
  private void Start()
  {
    SphereManagerScript.Instance.sphereActiveObjectList.Add(gameObject);
    gameStartLocalPosition = transform.localPosition;
  }
  private void Update()
  {
    if (goLocalPosition)
    {
      localPositionCounter += Time.deltaTime * 2f;
      transform.localPosition = Vector3.Slerp(startLocalPosition, endLocalPosition, localPositionCounter);
      if (localPositionCounter > 1f)
      {
        localPositionCounter = 0;
        goLocalPosition = false;
        limbScript.activeItemCount++;
      }
    }
  }
  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Obstacle") && transform.parent.GetComponent<LimbScript>() && transform.parent.GetComponent<LimbScript>() == limbScript)
    {
      Fall();
    }
  }
  private void Fall()
  {
    limbScript.DeactivateItem(gameStartLocalPosition);
    transform.parent = SphereManagerScript.Instance.transform;
    rb.isKinematic = false;
    Invoke("DeactiveteObj", 1f);
  }
  private void DeactiveteObj()
  {
    rb.isKinematic = true;
    SphereManagerScript.Instance.DeactiveteObject(gameObject);
  }
  public void GoLocalPosition(Vector3 targetPosition)
  {
    startLocalPosition = transform.localPosition;
    endLocalPosition = targetPosition;
    localPositionCounter = 0;
    goLocalPosition = true;
  }
  public void SetLimbScript(LimbScript ls)
  {
    limbScript = ls;
  }
  public void SetIsKinematicFalse()
  {
    rb.isKinematic = false;
  }
}