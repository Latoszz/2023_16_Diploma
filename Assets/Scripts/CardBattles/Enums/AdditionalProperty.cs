using System;

namespace CardBattles.Enums {
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class PropertyDescriptionAttribute : Attribute {
        public string Description { get; }
        
        public PropertyDescriptionAttribute(string description) {
            Description = description;
        }
    }

    public enum AdditionalProperty {
        [PropertyDescription("Costs 0 AP to play")]
        Free_To_Play,

        [PropertyDescription("50% chance to not attack")]
        Lazy,

        [PropertyDescription("Kills cards with 1 hit")]
        Poisonous,

        [PropertyDescription("Not affected by Poisonous")]
        Immune_To_Poison,
   
        [PropertyDescription("Restores health when dealing damage")]
        Vampiric,

        [PropertyDescription("Doesn't attack on turn played")]
        Sleepy,

        [PropertyDescription("Cannot be healed")]
        Unhealable,

        [PropertyDescription("Costs 2 AP to play")]
        Costly,

        [PropertyDescription("Takes half damage (rounded up)")]
        Durable,
        
        [PropertyDescription("Deal damage when attacked")]
        Spiky,
        
        [PropertyDescription("Testing, auto applied to all")]
        Just_Played,
        
        [PropertyDescription("Will not be added to deck")]
        Non_Functional
    }

    public static class AdditionalPropertyHelper {
        public static string GetDescription(AdditionalProperty property) {
            var field = property.GetType().GetField(property.ToString());
            var attribute = (PropertyDescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(PropertyDescriptionAttribute));
            return attribute?.Description ?? "No description available";
        }
    }
}