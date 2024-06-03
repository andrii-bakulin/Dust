using UnityEngine;

namespace Dust.Experimental.Variables
{
    public class AbstractVariablesManager : MonoBehaviour
    {
        public T FindVariableByName<T>(string fieldName) where T : class
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                var child = this.transform.GetChild(i);

                if (child.name == fieldName)
                    return child.GetComponent<T>();
            }

            return null;
        }
    }
}
