using System.Collections;
using System.Collections.Generic;
using RPG.Inventories;
using RPG.Stats;
using UnityEngine;

namespace RPG.Inventories{

    public class StatsEquipment : Equipment, IModifierProvider
    {
        // Start is called before the first frame update
        public IEnumerable<float> GetAdditiveModifier(Stat stat)
        {
            foreach(var slot in GetAllPopulatedSlots())
            {
                var item=GetItemInSlot(slot) as IModifierProvider;
                if(item==null)continue;
                foreach(float Modifier in item.GetAdditiveModifier(stat))
                {
                    yield return Modifier;
                }
            }
        }

        public IEnumerable<float> GetPercentageModifier(Stat stat)
        {
            foreach(var slot in GetAllPopulatedSlots())
            {
                var item= GetItemInSlot(slot) as    IModifierProvider;
                if(item==null)continue;
                foreach (float Modifier in item.GetPercentageModifier(stat))
                {
                    yield return Modifier;
                }
            }
        }
    }

}
