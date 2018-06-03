using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FolderScript : MonoBehaviour {

  private Button btn;
  public Text text;
  public int frame;
  public float height;

  // Use this for initialization  
  void Start() {
    btn = this.gameObject.GetComponent<Button>();
    btn.onClick.AddListener(OnClick);
    StartCoroutine(TextCollapsed());
  }

  IEnumerator TextCollapsed() {
    float y = height;
    for (int i = 0; i < frame; ++i) {
      y -= height / frame;
      text.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, text.rectTransform.sizeDelta.x);
      text.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, y);
      if (i == frame - 1) {
        text.gameObject.SetActive(false);
      }
      yield return null;
    }
  }

  IEnumerator TextVisible() {
    float y = 0;
    for (int i = 0; i < frame; ++i) {
      y += height / frame;
      text.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, text.rectTransform.sizeDelta.x);
      text.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, y);
      if (i == 0) {
        text.gameObject.SetActive(true);
      }
      yield return null;
    }
  }

  void OnClick() {
    if (text.gameObject.activeSelf) {
      StartCoroutine(TextCollapsed());
    } else {
      StartCoroutine(TextVisible());
    }
  }
}