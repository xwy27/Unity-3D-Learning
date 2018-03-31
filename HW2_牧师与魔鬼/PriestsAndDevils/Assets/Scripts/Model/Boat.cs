using UnityEngine;

namespace MyNamespace {
    public class Boat {
        public readonly Moveable mScript;
        public readonly Vector3 departure;
        public readonly Vector3 destination;
        public readonly Vector3[] departures;
        public readonly Vector3[] destinations;
        public CharacterController[] passenger = new CharacterController[2];

        public GameObject _Boat { get; set; }
        public Location Location { get; set; }

        public Boat() {
            departure = new Vector3(5, 1, 0);
            destination = new Vector3(-5, 1, 0);
            Location = Location.right;

            departures = new Vector3[] {
                new Vector3(4.5f, 1.5f, 0),
                new Vector3(5.5f, 1.5f, 0) };
            destinations = new Vector3[] {
                new Vector3(-5.5f, 1.5f, 0),
                new Vector3(-4.5f, 1.5f, 0) };

            _Boat = Object.Instantiate(Resources.Load("Prefab/Boat", typeof(GameObject)),
                    departure, Quaternion.identity, null) as GameObject;
            _Boat.name = "boat";

            mScript = _Boat.AddComponent(typeof(Moveable)) as Moveable;
            _Boat.AddComponent(typeof(UserGUI));
        }
    }
}
