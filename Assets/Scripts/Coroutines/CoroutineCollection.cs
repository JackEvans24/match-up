using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Coroutines
{
    public class CoroutineCollection
    {
        private List<Coroutine> coroutines;

        public CoroutineCollection(params Coroutine[] coroutines)
        {
            this.coroutines = new List<Coroutine>();

            foreach (var coroutine in coroutines)
                Add(coroutine);
        }

        public void Add(Coroutine coroutine) => this.coroutines.Add(coroutine);

        public IEnumerator WaitForCompletion(bool clear = true)
        {
            foreach (var coroutine in coroutines)
                yield return coroutine;

            if (clear)
                coroutines.Clear();
        }
    }
}
