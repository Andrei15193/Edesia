﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Andrei15193.Edesia.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ErrorControllerStrings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ErrorControllerStrings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Andrei15193.Edesia.Resources.ErrorControllerStrings", typeof(ErrorControllerStrings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ne pare rău dar nu aveți suficiente drepturi pentru a accesa această funcționalitate..
        /// </summary>
        public static string ForbiddenLabel_DisplayName {
            get {
                return ResourceManager.GetString("ForbiddenLabel_DisplayName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Interzis.
        /// </summary>
        public static string ForbiddenViewTitle {
            get {
                return ResourceManager.GetString("ForbiddenViewTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ne pare rău dar locația pe care încercați să o accesați nu este disponibilă.
        /// </summary>
        public static string NotFoundLabel_DisplayName {
            get {
                return ResourceManager.GetString("NotFoundLabel_DisplayName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Indisponibil.
        /// </summary>
        public static string NotFoundViewTitle {
            get {
                return ResourceManager.GetString("NotFoundViewTitle", resourceCulture);
            }
        }
    }
}
