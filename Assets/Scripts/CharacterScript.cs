using UnityEngine;
using System.Collections.Generic;
public class CharacterScript : MonoBehaviour
{
  public static CharacterScript Instance;
  #region VARIABLES
  [SerializeField] float _speed = 3;
  [SerializeField] Animator characterAnimator;
  [SerializeField] float movementBounderyLeft, movementBounderyRight;
  [SerializeField] List<LimbScript> limbScriptList = new List<LimbScript>();
  private Transform _transform;
  private float _mouseLastPosX;
  private Vector3 _motion = new Vector3(0, 0, 1);
  private string lastAnimationTrigger = "";
  #endregion
  #region MAIN FUNC
  private void Awake()
  {
    Instance = this;
  }
  private void Start()
  {
    _transform = transform;
  }
  private void Update()
  {
    Movement();
  }
  private void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Collectable"))
    {
      ControlLimbs(other.transform);
    }
  }
  private void OnTriggerExit(Collider other)
  {
    if (other.CompareTag("Obstacle") || other.CompareTag("Collectable"))
    {
      SetCharacterHeight();
    }
    if (other.CompareTag("Finish"))
    {
      Success();
    }
  }
  #endregion
  #region MOVEMENT
  private void Movement()
  {
    if (Input.GetMouseButtonDown(0))
    {
      _mouseLastPosX = Input.mousePosition.x;
    }
    if (Input.GetMouseButton(0))
    {
      var mousePosX = Input.mousePosition.x;
      if (mousePosX > _mouseLastPosX) { _motion.x = 1; }
      if (mousePosX < _mouseLastPosX) { _motion.x = -1; }
      if (mousePosX == _mouseLastPosX) { _motion.x = 0; }
      _mouseLastPosX = mousePosX;
    }
    if (Input.GetMouseButtonUp(0))
    {
      _motion.x = 0;
    }
    transform.Translate(_speed * Time.deltaTime * _motion);
    transform.position = new Vector3(Mathf.Clamp(transform.position.x, movementBounderyLeft, movementBounderyRight), transform.position.y, transform.position.z);
  }
  #endregion
  private void ControlLimbs(Transform t)
  {
    var count = limbScriptList.Count;
    for (int i = 0; i < count; i++)
    {
      if (limbScriptList[i].activeItemCount < limbScriptList[i].totalItemCount)
      {
        var childCount = t.childCount;
        var difference = limbScriptList[i].totalItemCount - limbScriptList[i].activeItemCount;
        var length = -1;
        if (childCount < difference)
          length = childCount;
        else
          length = difference;

        for (int j = length - 1; j >= 0; j--)
        {
          var sphere = t.GetChild(j).GetComponent<SphereScript>();
          sphere.transform.parent = limbScriptList[i].transform;
          sphere.GoLocalPosition(limbScriptList[i].GetFreePos());
          sphere.SetLimbScript(limbScriptList[i]);
        }
      }
    }
  }
  public void ControlAnimation()
  {
    var emptyLimbList = new List<bool>();
    var nonEmptyCount = 0;
    var triggerName = "";
    var count = limbScriptList.Count;
    for (int i = 0; i < count; i++)
    {
      if (limbScriptList[i].activeItemCount <= limbScriptList[i].totalItemCount * 10 / 100)
      { emptyLimbList.Add(true); }
      else
      {
        emptyLimbList.Add(false);
        nonEmptyCount++;
      }
    }
    if (nonEmptyCount + 1 == count)
    {
      triggerName = "normalWalk";
    }
    if (emptyLimbList[7] && emptyLimbList[9] && !emptyLimbList[6] && !emptyLimbList[8])
    {
      triggerName = "injuredJumpRight";
    }
    if (!emptyLimbList[7] && !emptyLimbList[9] && emptyLimbList[6] && emptyLimbList[8])
    {
      triggerName = "injuredJumpLeft";
    }
    if (emptyLimbList[6] && emptyLimbList[7] && emptyLimbList[8] && emptyLimbList[9])
    {
      triggerName = "lowCrawl";
    }
    lastAnimationTrigger = triggerName;
    characterAnimator.SetTrigger(triggerName);
    CheckGameOver();
  }
  public void SetCharacterHeight()
  {
    var full = true;
    for (int i = 0; i < limbScriptList.Count; i++)
    {
      if (limbScriptList[i].activeItemCount < limbScriptList[i].totalItemCount)
        full = false;
    }
    if (full)
      return;
    if (lastAnimationTrigger == "lowCrawl")
    {
      transform.position = new Vector3(transform.position.x, .5f, transform.position.z);
      return;
    }
    var legSphereYPositionList = new List<Transform>();

    for (int i = 0; i < limbScriptList[6].transform.childCount; i++)
    {
      legSphereYPositionList.Add(limbScriptList[6].transform.GetChild(i).transform);
    }
    for (int i = 0; i < limbScriptList[7].transform.childCount; i++)
    {
      legSphereYPositionList.Add(limbScriptList[7].transform.GetChild(i).transform);
    }
    for (int i = 0; i < limbScriptList[8].transform.childCount; i++)
    {
      legSphereYPositionList.Add(limbScriptList[8].transform.GetChild(i).transform);
    }
    for (int i = 0; i < limbScriptList[9].transform.childCount; i++)
    {
      legSphereYPositionList.Add(limbScriptList[9].transform.GetChild(i).transform);
    }

    legSphereYPositionList.Sort((p1, p2) => p1.transform.position.y.CompareTo(p2.transform.position.y));

    var y = .5f;
    RaycastHit raycastHit;
    if (Physics.Raycast(legSphereYPositionList[0].position, Vector3.down, out raycastHit, 5f, 1 << 6))
    {
      y = legSphereYPositionList[0].position.y - raycastHit.transform.position.y - raycastHit.transform.localScale.y;
    }

    transform.position = new Vector3(transform.position.x, y, transform.position.z);
  }
  private void CheckGameOver()
  {
    var gameOver = true;
    for (int i = 0; i < limbScriptList.Count; i++)
    {
      if (limbScriptList[i].activeItemCount > 0)
        gameOver = false;
    }
    if (gameOver)
    {
      _speed = 0;
      Debug.Log("GAME OVER");
    }
  }
  private void Success()
  {
    _speed = 0;
    characterAnimator.enabled = false;
    for (int i = 0; i < limbScriptList.Count; i++)
    {
      for (int j = 0; j < limbScriptList[i].transform.childCount; j++)
      {
        limbScriptList[i].transform.GetChild(j).GetComponent<SphereScript>().SetIsKinematicFalse();
      }
    }
    Debug.Log("SUCCESS");
  }
}