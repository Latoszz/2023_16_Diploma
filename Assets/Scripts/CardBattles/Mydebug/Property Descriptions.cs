using System;
using System.Linq;
using CardBattles.Enums;
using NaughtyAttributes;
using UnityEngine;

namespace CardBattles.Mydebug {
    public class PropertyDescriptions : MonoBehaviour {
        [ResizableTextArea]
        [SerializeField]
        private string info;
        
        [Button]
        private void UpdateDescriptions() {
            var enumValues = Enum.GetValues(typeof(AdditionalProperty)).Cast<AdditionalProperty>().ToList();
            info = "";
            foreach (var value in enumValues) {
                info += value+".\n";
                info += AdditionalPropertyHelper.GetDescription(value)+"\n\n";
            }
            if(!String.IsNullOrWhiteSpace(info))
                info =info.Remove(info.Length - 2, 2);

        }
    }
}
