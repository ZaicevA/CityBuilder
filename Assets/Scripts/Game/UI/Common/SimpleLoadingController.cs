using Foundation;
using UnityEngine;

namespace Game.UI
{
    public class SimpleLoadingController : AbstractService<ILoadingController>, ILoadingController
    {
        [SerializeField] private GameObject _loadingGO;
        
        public void SetLoading(bool value)
        {
            _loadingGO.SetActive(value);
        }
    }    
}

