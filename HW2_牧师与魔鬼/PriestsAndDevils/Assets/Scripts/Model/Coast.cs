using UnityEngine;

namespace MyNamespace {
    public class Coast {
        readonly GameObject coast;
        readonly Vector3 departure;
        public readonly Vector3 destination;
        public readonly Vector3[] positions;

        public CharacterController[] characters;
        public Location Location { get; set; }

        public Coast(string _location) {
            departure = new Vector3(9, 1, 0);
            destination = new Vector3(-9, 1, 0);
            positions = new Vector3[] {
                new Vector3(6.5f, 2.25f, 0),
                new Vector3(7.5f, 2.25f, 0),
                new Vector3(8.5f, 2.25f, 0),
                new Vector3(9.5f, 2.25f, 0),
                new Vector3(10.5f, 2.25f, 0),
                new Vector3(11.5f, 2.25f, 0),};
            characters = new CharacterController[6];

            if (_location == "right") {
                coast = Object.Instantiate(Resources.Load("Prefab/Stone", typeof(GameObject)),
                        departure, Quaternion.identity, null) as GameObject;
                coast.name = "departure";
                Location = Location.right;
            } else {
                coast = Object.Instantiate(Resources.Load("Prefab/Stone", typeof(GameObject)),
                        destination, Quaternion.identity, null) as GameObject;
                coast.name = "destination";
                Location = Location.left;
            }
        }
    }
}
