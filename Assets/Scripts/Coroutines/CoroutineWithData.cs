using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Coroutines
{
    public class CoroutineWithData<T>
    {
        public Coroutine Coroutine { get; private set; }
        public T Result { get => (T)result; }

        private object result;
        private IEnumerator target;

        public CoroutineWithData(MonoBehaviour owner, IEnumerator target)
        {
            this.target = target;
            this.Coroutine = owner.StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            while (target.MoveNext())
            {
                result = target.Current;
                yield return result;
            }
        }
    }
}
