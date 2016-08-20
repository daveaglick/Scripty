using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.Options;

namespace Scripty.Core.Output
{
    public class FormatterOptions
    {
        public bool NewLinesForBracesInTypes { get; set; } = CSharpFormattingOptions.NewLinesForBracesInTypes.DefaultValue;
        public bool NewLinesForBracesInMethods { get; set; } = CSharpFormattingOptions.NewLinesForBracesInMethods.DefaultValue;
        public bool NewLinesForBracesInProperties { get; set; } = CSharpFormattingOptions.NewLinesForBracesInProperties.DefaultValue;
        public bool NewLinesForBracesInAccessors { get; set; } = CSharpFormattingOptions.NewLinesForBracesInAccessors.DefaultValue;
        public bool NewLinesForBracesInObjectCollectionArrayInitializers { get; set; } = CSharpFormattingOptions.NewLinesForBracesInObjectCollectionArrayInitializers.DefaultValue;
        
        public bool NewLineForCatch { get; set; } = CSharpFormattingOptions.NewLineForCatch.DefaultValue;
        public bool NewLineForFinally { get; set; } = CSharpFormattingOptions.NewLineForFinally.DefaultValue;

        public bool NewLinesForBracesInControlBlocks { get; set; } = CSharpFormattingOptions.NewLinesForBracesInControlBlocks.DefaultValue;
        public bool NewLineForElse { get; set; } = CSharpFormattingOptions.NewLineForElse.DefaultValue;

        public bool NewLinesForBracesInAnonymousTypes { get; set; } = CSharpFormattingOptions.NewLinesForBracesInAnonymousTypes.DefaultValue;
        public bool NewLinesForBracesInAnonymousMethods { get; set; } = CSharpFormattingOptions.NewLinesForBracesInAnonymousMethods.DefaultValue;
        public bool NewLinesForBracesInLambdaExpressionBody { get; set; } = CSharpFormattingOptions.NewLinesForBracesInLambdaExpressionBody.DefaultValue;
        public bool NewLineForClausesInQuery { get; set; } = CSharpFormattingOptions.NewLineForClausesInQuery.DefaultValue;

        public bool IndentSwitchCaseSection { get; set; } = CSharpFormattingOptions.IndentSwitchCaseSection.DefaultValue;
        public bool IndentSwitchSection { get; set; } = CSharpFormattingOptions.IndentSwitchSection.DefaultValue;

        public bool SpaceAfterCast { get; set; } = CSharpFormattingOptions.SpaceAfterCast.DefaultValue;
        public bool SpaceWithinCastParentheses { get; set; } = CSharpFormattingOptions.SpaceWithinCastParentheses.DefaultValue;

        public bool SpaceBeforeComma { get; set; } = CSharpFormattingOptions.SpaceBeforeComma.DefaultValue;
        public bool SpaceAfterComma { get; set; } = CSharpFormattingOptions.SpaceAfterComma.DefaultValue;

        public bool SpaceBeforeDot { get; set; } = CSharpFormattingOptions.SpaceBeforeDot.DefaultValue;
        public bool SpaceAfterDot { get; set; } = CSharpFormattingOptions.SpaceAfterDot.DefaultValue;

        public bool SpaceAfterControlFlowStatementKeyword { get; set; } = CSharpFormattingOptions.SpaceAfterControlFlowStatementKeyword.DefaultValue;

        public bool SpaceWithinMethodCallParentheses { get; set; } = CSharpFormattingOptions.SpaceWithinMethodCallParentheses.DefaultValue;
        public bool SpaceBetweenEmptyMethodCallParentheses { get; set; } = CSharpFormattingOptions.SpaceBetweenEmptyMethodCallParentheses.DefaultValue;
        public bool SpaceAfterMethodCallName { get; set; } = CSharpFormattingOptions.SpaceAfterMethodCallName.DefaultValue;

        public bool SpaceWithinMethodDeclarationParenthesis { get; set; } = CSharpFormattingOptions.SpaceWithinMethodDeclarationParenthesis.DefaultValue;
        public bool SpacingAfterMethodDeclarationName { get; set; } = CSharpFormattingOptions.SpacingAfterMethodDeclarationName.DefaultValue;

        public bool SpaceBeforeSemicolonsInForStatement { get; set; } = CSharpFormattingOptions.SpaceBeforeSemicolonsInForStatement.DefaultValue;
        public bool SpaceAfterSemicolonsInForStatement { get; set; } = CSharpFormattingOptions.SpaceAfterSemicolonsInForStatement.DefaultValue;

        public bool SpaceBeforeOpenSquareBracket { get; set; } = CSharpFormattingOptions.SpaceBeforeOpenSquareBracket.DefaultValue;
        public bool SpaceBetweenEmptySquareBrackets { get; set; } = CSharpFormattingOptions.SpaceBetweenEmptySquareBrackets.DefaultValue;
        public bool SpaceWithinSquareBrackets { get; set; } = CSharpFormattingOptions.SpaceWithinSquareBrackets.DefaultValue;

        public bool SpaceBetweenEmptyMethodDeclarationParentheses { get; set; } = CSharpFormattingOptions.SpaceBetweenEmptyMethodDeclarationParentheses.DefaultValue;
        public bool SpacesIgnoreAroundVariableDeclaration { get; set; } = CSharpFormattingOptions.SpacesIgnoreAroundVariableDeclaration.DefaultValue;

        public bool SpaceWithinExpressionParentheses { get; set; } = CSharpFormattingOptions.SpaceWithinExpressionParentheses.DefaultValue;
        public bool SpaceWithinOtherParentheses { get; set; } = CSharpFormattingOptions.SpaceWithinOtherParentheses.DefaultValue;

        public bool WrappingKeepStatementsOnSingleLine { get; set; } = CSharpFormattingOptions.WrappingKeepStatementsOnSingleLine.DefaultValue;

        public OptionSet Apply(OptionSet options)
        {
            return options
                .WithChangedOption(CSharpFormattingOptions.WrappingPreserveSingleLine, false)

                .WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInTypes, NewLinesForBracesInTypes)
                .WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInMethods, NewLinesForBracesInMethods)
                .WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInProperties, NewLinesForBracesInProperties)
                .WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInAccessors, NewLinesForBracesInAccessors)
                .WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInObjectCollectionArrayInitializers, NewLinesForBracesInObjectCollectionArrayInitializers)
                .WithChangedOption(CSharpFormattingOptions.NewLineForCatch, NewLineForCatch)
                .WithChangedOption(CSharpFormattingOptions.NewLineForFinally, NewLineForFinally)
                .WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInControlBlocks, NewLinesForBracesInControlBlocks)
                .WithChangedOption(CSharpFormattingOptions.NewLineForElse, NewLineForElse)
                .WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInAnonymousTypes, NewLinesForBracesInAnonymousTypes)
                .WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInAnonymousMethods, NewLinesForBracesInAnonymousMethods)
                .WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInLambdaExpressionBody, NewLinesForBracesInLambdaExpressionBody)
                .WithChangedOption(CSharpFormattingOptions.NewLineForClausesInQuery, NewLineForClausesInQuery)
                .WithChangedOption(CSharpFormattingOptions.IndentSwitchCaseSection, IndentSwitchCaseSection)
                .WithChangedOption(CSharpFormattingOptions.IndentSwitchSection, IndentSwitchSection)
                .WithChangedOption(CSharpFormattingOptions.SpaceAfterCast, SpaceAfterCast)
                .WithChangedOption(CSharpFormattingOptions.SpaceWithinCastParentheses, SpaceWithinCastParentheses)
                .WithChangedOption(CSharpFormattingOptions.SpaceBeforeComma, SpaceBeforeComma)
                .WithChangedOption(CSharpFormattingOptions.SpaceAfterComma, SpaceAfterComma)
                .WithChangedOption(CSharpFormattingOptions.SpaceBeforeDot, SpaceBeforeDot)
                .WithChangedOption(CSharpFormattingOptions.SpaceAfterDot, SpaceAfterDot)
                .WithChangedOption(CSharpFormattingOptions.SpaceAfterControlFlowStatementKeyword, SpaceAfterControlFlowStatementKeyword)
                .WithChangedOption(CSharpFormattingOptions.SpaceWithinMethodCallParentheses, SpaceWithinMethodCallParentheses)
                .WithChangedOption(CSharpFormattingOptions.SpaceBetweenEmptyMethodCallParentheses, SpaceBetweenEmptyMethodCallParentheses)
                .WithChangedOption(CSharpFormattingOptions.SpaceAfterMethodCallName, SpaceAfterMethodCallName)
                .WithChangedOption(CSharpFormattingOptions.SpaceWithinMethodDeclarationParenthesis, SpaceWithinMethodDeclarationParenthesis)
                .WithChangedOption(CSharpFormattingOptions.SpacingAfterMethodDeclarationName, SpacingAfterMethodDeclarationName)
                .WithChangedOption(CSharpFormattingOptions.SpaceBeforeSemicolonsInForStatement, SpaceBeforeSemicolonsInForStatement)
                .WithChangedOption(CSharpFormattingOptions.SpaceAfterSemicolonsInForStatement, SpaceAfterSemicolonsInForStatement)
                .WithChangedOption(CSharpFormattingOptions.SpaceBeforeOpenSquareBracket, SpaceBeforeOpenSquareBracket)
                .WithChangedOption(CSharpFormattingOptions.SpaceBetweenEmptySquareBrackets, SpaceBetweenEmptySquareBrackets)
                .WithChangedOption(CSharpFormattingOptions.SpaceWithinSquareBrackets, SpaceWithinSquareBrackets)
                .WithChangedOption(CSharpFormattingOptions.SpaceBetweenEmptyMethodDeclarationParentheses, SpaceBetweenEmptyMethodDeclarationParentheses)
                .WithChangedOption(CSharpFormattingOptions.SpacesIgnoreAroundVariableDeclaration, SpacesIgnoreAroundVariableDeclaration)
                .WithChangedOption(CSharpFormattingOptions.SpaceWithinExpressionParentheses, SpaceWithinExpressionParentheses)
                .WithChangedOption(CSharpFormattingOptions.SpaceWithinOtherParentheses, SpaceWithinOtherParentheses)
                .WithChangedOption(CSharpFormattingOptions.WrappingKeepStatementsOnSingleLine, WrappingKeepStatementsOnSingleLine)
            ;
        }
    }
}
