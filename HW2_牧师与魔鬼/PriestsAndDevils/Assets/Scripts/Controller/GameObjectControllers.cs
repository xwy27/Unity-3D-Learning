using UnityEngine;

namespace MyNamespace {
    public enum Location { left, right }

    public class CoastController {
        public Coast coast;

        public CoastController(string _location) {
            coast = new Coast(_location);
        }

        public int GetEmptyIndex() {
            for (int i = 0; i < coast.characters.Length; ++i) {
                if (coast.characters[i] == null) {
                    return i;
                }
            }
            return -1;
        }

        public Vector3 GetEmptyPosition() {
            Vector3 pos = coast.positions[GetEmptyIndex()];
            pos.x *= (coast.Location == Location.right ? 1 : -1);
            return pos;
        }

        public void GetOnCoast(CharacterController character) {
            int index = GetEmptyIndex();
            coast.characters[index] = character;
        }

        public void GetOffCoast(string passenger_name) {
            for (int i = 0; i < coast.characters.Length; ++i) {
                if (coast.characters[i] != null && 
                    coast.characters[i].character.Name == passenger_name) {
                    coast.characters[i] = null;
                }
            }
        }

        public int[] GetCharacterNum() {
            int[] count = { 0, 0 };
            for (int i = 0; i < coast.characters.Length; ++i) {
                if (coast.characters[i] != null) {                   
                    if (coast.characters[i].character.Name.Contains("priest")) {
                        count[0]++;
                    } else {
                        count[1]++;
                    }
                }
            }
            return count;
        }

        public void Reset() {
            coast.characters = new CharacterController[6];
        }
    }

    public class BoatController {
        public Boat boat;

        public BoatController() {
            boat = new Boat();
        }


        public void Move() {
            if (boat.Location == Location.left) {
                boat.mScript.SetDestination(boat.departure);
                boat.Location = Location.right;
            } else {
                boat.mScript.SetDestination(boat.destination);
                boat.Location = Location.left;
            }
        }

        public int GetEmptyIndex() {
            for (int i = 0; i < boat.passenger.Length; ++i) {
                if (boat.passenger[i] == null) {
                    return i;
                }
            }
            return -1;
        }

        public bool IsEmpty() {
            for (int i = 0; i < boat.passenger.Length; ++i) {
                if (boat.passenger[i] != null) {
                    return false;
                }
            }
            return true;
        }

        public Vector3 GetEmptyPosition() {
            Vector3 pos;
            int emptyIndex = GetEmptyIndex();
            if (boat.Location == Location.right) {
                pos = boat.departures[emptyIndex];
            } else {
                pos = boat.destinations[emptyIndex];
            }
            return pos;
        }

        public void GetOnBoat(CharacterController character) {
            int index = GetEmptyIndex();
            boat.passenger[index] = character;
        }

        public void GetOffBoat(string passenger_name) {
            for (int i = 0; i < boat.passenger.Length; ++i) {
                if (boat.passenger[i] != null && 
                    boat.passenger[i].character.Name == passenger_name) {
                    boat.passenger[i] = null;
                }
            }
        }

        public int[] GetCharacterNum() {
            int[] count = { 0, 0 };
            for (int i = 0; i < boat.passenger.Length; ++i) {
                if (boat.passenger[i] != null) {
                    if (boat.passenger[i].character.Name.Contains("priest")) {
                        count[0]++;
                    } else {
                        count[1]++;
                    }
                }
            }
            return count;
        }

        public void Reset() {
            boat.mScript.Reset();
            if (boat.Location == Location.left) {
                Move();
            }
            boat.passenger = new CharacterController[2];
        }
    }

    public class CharacterController {
        readonly UserGUI userGUI;
        public Character character;

        public CharacterController(string _name) {
            character = new Character(_name);
            userGUI = character.Role.AddComponent(typeof(UserGUI)) as UserGUI;
            userGUI.SetCharacterCtrl(this);
        }

        public void SetPosition(Vector3 _pos) {
            character.Role.transform.position = _pos;
        }

        public void MoveTo(Vector3 _pos) {
            character.mScript.SetDestination(_pos);
        }

        public void GetOnBoat(BoatController boatCtrl) {
            character.Coast = null;
            character.Role.transform.parent = boatCtrl.boat._Boat.transform;
            character.IsOnBoat = true;
        }

        public void GetOnCoast(CoastController coast) {
            character.Coast = coast;
            character.Role.transform.parent = null;
            character.IsOnBoat = false;
        }

        public void Reset() {
            character.mScript.Reset();
            character.Coast = (Director.GetInstance().CurrentSecnController as FirstController).rightCoastCtrl;
            GetOnCoast(character.Coast);
            SetPosition(character.Coast.GetEmptyPosition());
            character.Coast.GetOnCoast(this);
        }
    }
}
