using UnityEngine;

public interface ITriggerCheckable 
{
      bool IsAggroed {get; set;}

      void SetAgroStatus(bool isAggroed);            
}
