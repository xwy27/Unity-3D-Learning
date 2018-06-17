using UnityEngine;
using MyNamespace;

public class Coast {
    readonly GameObject coast;
    public static Vector3 departure = new Vector3(9, 1, 0);
    public static Vector3 destination = new Vector3(-9, 1, 0);
    public readonly Vector3[] positions;

    public MyNamespace.CharacterController[] characters;
    public Location Location { get; set; }

    public Coast(string _location) {
        positions = new Vector3[] {
            new Vector3(6.5f, 2.25f, 0),
            new Vector3(7.5f, 2.25f, 0),
            new Vector3(8.5f, 2.25f, 0),
            new Vector3(9.5f, 2.25f, 0),
            new Vector3(10.5f, 2.25f, 0),
            new Vector3(11.5f, 2.25f, 0),};
        characters = new MyNamespace.CharacterController[6];

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
