using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Hacks
    {
        private static Hacks _hacks = new Hacks();
        public static Hacks Hak { get => _hacks; }

        public bool Win() => Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.W);
        public bool Lose() => Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.L);
        public bool Money() => Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.M);
        public bool Immortal() => Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.I);

    }
}
