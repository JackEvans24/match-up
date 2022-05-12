using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class GameObjectExtensions
    {
        public static GameObject CreateChild(this GameObject gameObject, string name)
        {
            var newObject = new GameObject(name);

            newObject.transform.parent = gameObject.transform;
            newObject.transform.localPosition = Vector3.zero;

            return newObject;
        }
    }
}
