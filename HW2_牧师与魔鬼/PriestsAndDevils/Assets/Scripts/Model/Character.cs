using UnityEngine;

namespace MyNamespace {
    public class Character {
        public Moveable mScript;
        public GameObject Role { get; set; }
        public CoastController Coast { get; set; }
        public bool IsOnBoat { get; set; }
        public string Name {
            get {
                return Role.name;
            }
            set {
                Role.name = value;
            }
        }

        public Character(string _name) {
            if (_name.Contains("priest")) {
                Role = Object.Instantiate(Resources.Load("Prefab/Priest", typeof(GameObject)),
                            Vector3.zero, Quaternion.identity, null) as GameObject;
            } else {
                Role = Object.Instantiate(Resources.Load("Prefab/Devil", typeof(GameObject)),
                            Vector3.zero, Quaternion.identity, null) as GameObject;
            }
            IsOnBoat = false;
            Name = _name;
            mScript = Role.AddComponent(typeof(Moveable)) as Moveable;
            
        }
    }
}
