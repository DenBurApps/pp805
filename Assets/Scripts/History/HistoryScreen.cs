using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(ScreenVisabilityHandler))]
public class HistoryScreen : MonoBehaviour
{
   [SerializeField] private List<HistoryPlane> _historyPlanes;
   [SerializeField] private GameObject _emptyObject;

   private ScreenVisabilityHandler _screenVisabilityHandler;

   private void Awake()
   {
      _screenVisabilityHandler = GetComponent<ScreenVisabilityHandler>();
   }

   private void Start()
   {
      DisableAllPlanes();
      DisableScreen();
   }

   public void EnableScreen()
   {
      _screenVisabilityHandler.EnableScreen();

      var historyDatas = HistoryDataHolder.HistoryDatas;

      if (historyDatas.Count <= 0)
      {
         _emptyObject.SetActive(true);
         return;
      }

      foreach (var data in historyDatas)
      {
         var availablePlane = _historyPlanes.FirstOrDefault(plane => !plane.IsActive);

         availablePlane?.Enable(data);
      }

      _emptyObject.SetActive(false);
   }

   private void DisableAllPlanes()
   {
      foreach (var plane in _historyPlanes)
      {
         plane.Disable();
      }
      
      _emptyObject.SetActive(true);
   }

   public void DisableScreen()
   {
      _screenVisabilityHandler.DisableScreen();
   }
}
