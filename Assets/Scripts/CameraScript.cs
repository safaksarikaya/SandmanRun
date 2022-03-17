using UnityEngine;
public class CameraScript : MonoBehaviour
{
  [SerializeField] Transform targetTransform;
  [SerializeField] Vector3 targetOffset;
  [SerializeField] bool autoOffset;
  [SerializeField] float followSpeed = 2;
  private void Start()
  {
    if (autoOffset) { targetOffset = transform.position - targetTransform.position; }
  }
  private void Update()
  {
    transform.position = Vector3.Lerp(transform.position, targetTransform.position + targetOffset, followSpeed * Time.deltaTime);
  }
}