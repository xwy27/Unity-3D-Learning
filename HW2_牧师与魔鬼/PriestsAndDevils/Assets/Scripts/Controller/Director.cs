using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyNamespace;

namespace MyNamespace {
    public class Director : System.Object {
        private static Director _instance;
        public ISceneController CurrentSecnController { get; set; }

        public static Director GetInstance() {
            return _instance ?? (_instance = new Director());
        }
    }
}