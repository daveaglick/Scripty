namespace Scripty.Core
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using Output;

    /// <summary>
    ///      How to handle output attached to the executing script
    /// </summary>
    public enum OutputBehavior
    {
        /// <summary>
        ///      If script execution produces errors, the output should be left unaltered. This is the default.
        /// </summary>
        [Display(Name = "Dont overwrite existing output if evaluation fails")]
        [Description("If the script evaluation fails, existing prior output is not modified")]
        DontOverwriteIfEvaluationFails,

        /// <summary>
        ///     The script controls what to do with output by using the 
        /// <see cref=" OutputFile.KeepOutput"/> value
        /// </summary>
        [Display(Name = "The script controls output retention")]
        [Description("The script must use Output.Keep to retain or discard output")]
        ScriptControlsOutput,

        /// <summary>
        ///     Saving script ouput is never attempted. 
        /// </summary>
        /// <remarks>
        ///     May be useful when using scripty in a macro or an easy ide extensibility fashion
        /// </remarks>
        [Display(Name = "Never generate output")]
        [Description("Script evaluation discards any generated output.")]
        NeverGenerateOutput
    }

    public static class Enums
    {
        public static string GetDescription(this Enum value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            if (name != null)
            {
                var field = type.GetField(name);
                if (field != null)
                {
                    var attr =
                        Attribute.GetCustomAttribute(field,
                            typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }

        public static string GetName(this Enum value)
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            if (name != null)
            {
                var field = type.GetField(name);
                if (field != null)
                {
                    var attr =
                        Attribute.GetCustomAttribute(field,
                            typeof(DisplayAttribute)) as DisplayAttribute;
                    if (attr != null)
                    {
                        return attr.Name;
                    }
                }
            }
            return null;
        }
    }
}