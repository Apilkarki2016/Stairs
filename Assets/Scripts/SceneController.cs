using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Stairs.Utils
{
    public class SceneController : MonoBehaviour
    {
        public void ReloadScene()
        {
            SceneManager.LoadScene(0);
            Pool.Instance.DestroyAllPools();
            Pool.Instance.ReInitialize();
        }
    }
}
