using MyNamespace;
using System.Collections.Generic;
using UnityEngine;

public class FirstSceneActionManager : ActionManager {
    public void MoveBoat(BoatController boatController) {
        SSMoveToAction action = SSMoveToAction.GetSSMoveToAction(boatController.GetDestination(), boatController.boat.movingSpeed);
        AddAction(boatController.boat._Boat, action, this);
    }

    public void MoveCharacter(MyNamespace.CharacterController characterCtrl, Vector3 destination) {
        Vector3 currentPos = characterCtrl.character.Role.transform.position;
        Vector3 middlePos = currentPos;
        //采用线段式的折线运动
        //终点纵坐标小，则为从岸上船
        //否则，从船上岸
        if (destination.y > currentPos.y) middlePos.y = destination.y;
        else {
            middlePos.x = destination.x;
        }
        SSAction action1 = SSMoveToAction.GetSSMoveToAction(middlePos, characterCtrl.character.movingSpeed);
        SSAction action2 = SSMoveToAction.GetSSMoveToAction(destination, characterCtrl.character.movingSpeed);
        //动作队列完成上船或上岸动作
        SSAction seqAction = SequenceAction.GetSequenceAction(1, 0, new List<SSAction> { action1, action2 });
        AddAction(characterCtrl.character.Role, seqAction, this);
    }
}
