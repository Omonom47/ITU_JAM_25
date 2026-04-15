using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Int")]
    public class IntVariable : ScriptableObject
    {
        public int Value;
    }
}