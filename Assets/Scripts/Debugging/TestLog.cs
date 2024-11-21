using UnityEngine;

namespace Debugging {
    public class TestLog : MonoBehaviour
    {
        public void fun1() {
            Debug.Log("fun1");
        }
        public void fun2() {
            Debug.Log("fun2");
        }
        public void fun3() {
            Debug.Log("fun3");
        }
        public void fun4() {
            Debug.Log("fun4");
        }

        public void funCustom(string custom) {
            Debug.Log(custom);

        }
    }
}
