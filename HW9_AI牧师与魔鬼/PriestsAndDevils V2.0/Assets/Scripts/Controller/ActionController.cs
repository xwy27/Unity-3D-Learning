using System.Collections.Generic;
using UnityEngine;
using MyNamespace;

public class SSAction : ScriptableObject {
    public bool enable = true;
    public bool destroy = false;

    public GameObject GameObject { get; set; }
    public Transform Transform { get; set; }
    public ISSActionCallback Callback { get; set; }

    public virtual void Start() {
        throw new System.NotImplementedException();
    }

    public virtual void Update() {
        throw new System.NotImplementedException();
    }
}

public class SSMoveToAction : SSAction {
    public Vector3 target;
    public float speed;

    private SSMoveToAction() { }

    public static SSMoveToAction GetSSMoveToAction(Vector3 target, float speed) {
        SSMoveToAction action = CreateInstance<SSMoveToAction>();
        action.target = target;
        action.speed = speed;
        return action;
    }

    public override void Start() { }

    public override void Update() {
        Transform.position = Vector3.MoveTowards(Transform.position, target, speed*Time.deltaTime);
        if (Transform.position == target) {
            destroy = true;
            Callback.ActionDone(this);
        }
    }
}
public class SequenceAction: SSAction, ISSActionCallback {
    public List<SSAction> sequence;
    public int repeat = -1;
    public int currentActionIndex = 0;

    public static SequenceAction GetSequenceAction(int repeat, int currentActionIndex, List<SSAction> sequence) {
        SequenceAction action = CreateInstance<SequenceAction>();
        action.sequence = sequence;
        action.repeat = repeat;
        action.currentActionIndex = currentActionIndex;
        return action;
    }

    public override void Update() {
        if (sequence.Count == 0) return;
        if (currentActionIndex < sequence.Count) {
            sequence[currentActionIndex].Update();
        }
    }

    public void ActionDone(SSAction source) {
        source.destroy = false;
        currentActionIndex++;
        if (currentActionIndex >= sequence.Count) {
            currentActionIndex = 0;
            if (repeat > 0) repeat--;
            if (repeat == 0) {
                destroy = true;
                Callback.ActionDone(this);
            }
        }
    }

    public override void Start() {
        foreach(SSAction action in sequence) {
            action.GameObject = GameObject;
            action.Transform = Transform;
            action.Callback = this;
            action.Start();
        }
    }

    void OnDestroy() {
        foreach(SSAction action in sequence) {
            DestroyObject(action);
        }
    }
}

public class ActionManager : MonoBehaviour, ISSActionCallback {
    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
    private List<SSAction> waitingAdd = new List<SSAction>();
    private List<int> waitingDelete = new List<int>();

    protected void Update() {
        foreach(SSAction action in waitingAdd) {
            actions[action.GetInstanceID()] = action;
        }
        waitingAdd.Clear();

        foreach(KeyValuePair<int, SSAction> kv in actions) {
            SSAction action = kv.Value;
            if (action.destroy) {
                waitingDelete.Add(action.GetInstanceID());
            } else if (action.enable) {
                action.Update();
            }
        }

        foreach(int key in waitingDelete) {
            SSAction action = actions[key];
            actions.Remove(key);
            DestroyObject(action);
        }
        waitingDelete.Clear();
    }

    public void AddAction(GameObject gameObject, SSAction action, ISSActionCallback callback) {
        action.GameObject = gameObject;
        action.Transform = gameObject.transform;
        action.Callback = callback;
        waitingAdd.Add(action);
        action.Start();
    }

    public void ActionDone(SSAction source) { }
}