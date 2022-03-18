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
  [SerializeField] float rotateVal = 10f, rotateSpeed = 4;
  private Transform _transform;
  private float _mouseLastPosX;
  private Vector3 _motion = new Vector3(0, 0, 1);
  private string activeAnim;
  private bool gameStart;
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
    if (!gameStart)
    {
      if (Input.GetMouseButtonDown(0))
      {
        characterAnimator.SetTrigger("normalWalk");
        gameStart = true;
        UIScript.Instance.CloseTap2Start();
      }
      return;
    }
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
      if (activeAnim == "lowCrawl")
      {
        _transform.position = new Vector3(_transform.position.x, .5f, _transform.position.z);
      }
      else
      {
        SetCharacterHeight();
      }
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
    if (Input.GetMouseButton(0))
    {
      _motion.x = Input.GetAxis("Mouse X");
    }
    _transform.Translate(_speed * Time.deltaTime * _motion, Space.World);
    _transform.position = new Vector3(Mathf.Clamp(_transform.position.x, movementBounderyLeft, movementBounderyRight), _transform.position.y, _transform.position.z);
    var rotY = Mathf.Lerp(_transform.rotation.eulerAngles.x, rotateVal * 10 * _motion.x, rotateSpeed * Time.deltaTime);
    if ((rotY >= 0 && rotY < rotateVal) || rotY < 0 && (360 + rotY) > (360 - rotateVal))
    {
      _transform.rotation = Quaternion.Euler(new Vector3(_transform.rotation.x, rotY, _transform.rotation.z));
    }
  }
  #endregion
  #region GameFunc
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
          t.GetChild(j).GetComponent<SphereScript>().GoTargetSphere(limbScriptList[i].GetFreeSphere());
        }
      }
    }
  }
  private void SetCharacterHeight()
  {
    var minY = new List<float>();
    for (int i = 0; i < limbScriptList.Count; i++)
    {
      minY.Add(limbScriptList[i].GetMinYItem());
    }
    minY.Sort((p1, p2) => p1.CompareTo(p2));
    _transform.position = new Vector3(_transform.position.x, .5f - minY[0] + 1f, _transform.position.z);
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
    if (emptyLimbList[9] && !emptyLimbList[8])
    {
      triggerName = "injuredJumpRight";
    }
    if (!emptyLimbList[7] && !emptyLimbList[9] && emptyLimbList[6] && emptyLimbList[8])
    {
      triggerName = "injuredJumpLeft";
    }
    if (emptyLimbList[8] && !emptyLimbList[9])
    {
      triggerName = "injuredJumpLeft";
    }
    if (emptyLimbList[6] && emptyLimbList[7] && emptyLimbList[8] && emptyLimbList[9])
    {
      triggerName = "lowCrawl";
    }
    activeAnim = triggerName;
    characterAnimator.SetTrigger(triggerName);
    CheckGameOver();
  }

  #endregion
  #region GameState
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
      UIScript.Instance.ShowGameOver();
    }
  }
  private void Success()
  {
    _speed = 0;
    characterAnimator.SetTrigger("finish");
    for (int i = 0; i < limbScriptList.Count; i++)
    {
      for (int j = 0; j < limbScriptList[i].transform.childCount; j++)
      {
        limbScriptList[i].transform.GetChild(j).GetComponent<SphereScript>().SetIsKinematic(false);
      }
    }
    UIScript.Instance.ShowSuccess();
  }
  #endregion
}