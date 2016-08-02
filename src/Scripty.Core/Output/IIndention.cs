namespace Scripty.Core.Output
{
    using System;

    public interface IIndention : IDisposable
    {
        /// <summary>
        /// Gets the total indention <see cref="string"/>, combined from all <see cref="Indent"/> calls.
        /// </summary>
        string TotalIndention { get; }

        /// <summary>
        /// Gets or sets the character used for indenting.
        /// </summary>
        char IndentionCharacter { get; set; }

        /// <summary>
        /// Gets or sets the amount of times the <see cref="IndentionCharacter"/> is applied for each <see cref="Indent"/> call.
        /// </summary>
        byte IndentionRepeat { get; set; }

        /// <summary>
        /// Indents further output.
        /// </summary>
        /// <returns>The <see cref="IIndention"/> instance.</returns>
        /// <remarks>When the result is disposed, the indention will be decreased.</remarks>
        IIndention Indent();
    }
}