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
        [PropertyDescription("This card can be played for free.")]
        FreeToPlay,

        [PropertyDescription("50% chance that the card will not attack.")]
        Lazy,

        [PropertyDescription("This card deals poison damage.")]
        Poisonous,

        [PropertyDescription("This card cannot be affected by poison.")]
        ImmuneToPoison,
   
        [PropertyDescription("This card restores it's health when dealing damage.")]
        Vampiric,

        [PropertyDescription("Doesn't attack on turn played")]
        Sleepy,

        [PropertyDescription("This card cannot be healed.")]
        Unhealable,

        [PropertyDescription("This card costs 2 AP to play.")]
        Costly,

        [PropertyDescription("Takes half damage (rounded up).")]
        Durable

    }

    public static class AdditionalPropertyHelper {
        public static string GetDescription(AdditionalProperty property) {
            var field = property.GetType().GetField(property.ToString());
            var attribute = (PropertyDescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(PropertyDescriptionAttribute));
            return attribute?.Description ?? "No description available";
        }
    }
}