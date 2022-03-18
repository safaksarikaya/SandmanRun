using UnityEngine;
public class SphereScript : MonoBehaviour
{
  [SerializeField] Rigidbody rb;
  [SerializeField] LimbScript limbScript;
  private Vector3 gameStartLocalPosition;
  private Vector3 startLocalPosition, endLocalPosition;
  private float localPositionCounter;
  private bool goLocalPosition;
  public float startYPosition;
  private void Start()
  {
    SphereManagerScript.Instance.sphereActiveObjectList.Add(this);
    gameStartLocalPosition = transform.localPosition;
    startYPosition = transform.position.y;
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
        SphereManagerScript.Instance.DeactiveteObject(this);
        CharacterScript.Instance.ControlAnimation();
      }
    }
  }
  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Obstacle") && transform.parent.GetComponent<LimbScript>())
    {
      Fall();
    }
  }
  private void Fall()
  {
    limbScript.DeactivateItem(this);
    gameObject.SetActive(false);
    var cloneSphere = SphereManagerScript.Instance.GetSphere();
    cloneSphere.transform.position = transform.position;
    cloneSphere.SetIsKinematic(false);
    cloneSphere.DeactiveteObj();
  }
  public void DeactiveteObj()
  {
    Invoke("Deactivete", 1f);
  }
  private void Deactivete()
  {
    rb.isKinematic = true;
    SphereManagerScript.Instance.DeactiveteObject(this);
  }
  public void SetIsKinematic(bool isKinematic = false)
  {
    rb.isKinematic = isKinematic;
  }
  private SphereScript targetSphere;
  public void GoTargetSphere(SphereScript targetSphere)
  {
    this.targetSphere = targetSphere;
    transform.parent = targetSphere.transform.parent;
    startLocalPosition = transform.localPosition;
    endLocalPosition = targetSphere.transform.localPosition;
    localPositionCounter = 0;
    goLocalPosition = true;
  }
}