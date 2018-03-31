using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace {
    public interface ISceneController {
        void LoadResources();
    }

    public interface IUserAction {
        void MoveBoat();
        void CharacterClicked(CharacterController characterCtrl);
        void Restart();
    }
}
