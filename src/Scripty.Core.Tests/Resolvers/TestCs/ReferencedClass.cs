#pragma warning disable 1591
using System;


namespace Scripty.Core.Tests.TestCs
{

    using System.Data;
    
    /// <summary>
    ///     This example class is for testing the Resolvers namespace"/>
    /// </summary>
    /// <remarks>
    ///     This should have a little bit of everything common
    /// </remarks>
    public class ReferencedClass
    {
        //do these need to be renamed to avoid conflicts? Not for now, the class is lifted 
        private string _someString;
        public Object _EwPublicVariable;

        /// <summary>
        ///     The only way to get access to the Context for now is by injecting it into the classes 
        /// or methods that aren't in default global scope. 
        /// 
        ///     Some way to tag this or another like it as "These are specifically for scripty 
        /// to remove also" may be needed.
        ///     
        ///     Or the rewrite/recompile could just add a constructor and readwrite property available 
        /// as context, but dunno how existing classes not intended for scripting would use it.
        /// 
        ///     Or it could redirect text writers and other output mechanisims in the class 
        /// to emit script.
        /// 
        ///     Or another set of attributes could be used to have the re/re allow specific functions 
        /// to Opt-In to be lifted into the global scope like [DataMember] does for DCS.
        /// 
        /// </summary>
        //[ScriptyContext("This attribute could tell the CsFileRewriter to rewrite calls to this to use the global context, but it doesn't exist")]
        public ScriptContext InjectedContext { get; set; }

        public ReferencedClass()
        {
            _someString = $"Value_{Guid.NewGuid()}";
            _backingField = 69;
        }


        public ReferencedClass(ScriptContext injectedContext)
        {
            InjectedContext = injectedContext;
            _someString = $"Value_{Guid.NewGuid()}";
            _backingField = 69;
        }

        public int AutoPropertySomewhereElse { get; set; }

        /*
         Odd block of text in between members
             
             */

        //[ScriptyLift("This could be the opt-in attribute")]
        public void Owl(string message)
        {
            InjectedContext.Output.WriteLine($"{message} - {_someString}");
        }

        public int PropertyWithBackingField
        {
            /*
         Odd block of text inside member
             
             */
            get
            {
                return _backingField;
            }
            set { _backingField = value; }
        }

        private int _backingField;

        /// <summary>
        ///     A private class
        /// </summary>
        private class privClass
        {
            public int MyProperty { get; set; }
        }

        public System.Data.DataSet GetDataSet()
        {
            return new DataSet();
        }

    }
}
