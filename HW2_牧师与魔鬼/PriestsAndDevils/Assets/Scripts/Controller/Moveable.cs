using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyNamespace {
    public class Moveable : MonoBehaviour {
        readonly float speed = 20;

        int status;
        Vector3 middle;
        Vector3 destination;

        public void SetDestination(Vector3 _pos) {
            destination = _pos;
            middle = _pos;
            if (_pos.y == transform.position.y) {          // Boat moving
                status = 2;
            } else if (_pos.y < transform.position.y) {    // Character from coast to boat
                middle.y = transform.position.y;
            } else {                                        // Character from boat to coast
                middle.x = transform.position.x;
            }
            status = 1;
        }

        void Update() {
            if (status == 1) {
                transform.position = Vector3.MoveTowards(transform.position, middle, speed * Time.deltaTime);
                if (transform.position == middle) {
                    status = 2;
                }
            } else if (status == 2) {
                transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
                if (transform.position == destination) {
                    status = 0;
                }
            }
        }

        public void Reset() {
            status = 0;
        }
    }
}
