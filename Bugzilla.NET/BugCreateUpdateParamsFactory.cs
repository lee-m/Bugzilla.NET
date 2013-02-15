//Copyright (C) 2012 by Lee Millward

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;

using Bugzilla;
using Bugzilla.Proxies.Bug.Params;

using CookComputing.XmlRpc;

namespace Bugzilla
{
  /// <summary>
  /// Helper class which is used to create dynamic types at run-time to use when creating or updating 
  /// bugs with custom fields.
  /// </summary>
  internal class BugCreateUpdateParamsFactory
  {
    /// <summary>
    /// Module builder used to create the dynamic types.
    /// </summary>
    private ModuleBuilder mModuleBuilder;

    /// <summary>
    /// Set of previously created dynamic types used when creating new bugs.
    /// </summary>
    private Dictionary<int, Type> mCreateBugTypes;
    
    /// <summary>
    /// Set of previously created dynamic types used when updating bugs.
    /// </summary>
    private Dictionary<int, Type> mUpdateBugTypes;

    /// <summary>
    /// Singleton instance.
    /// </summary>
    private static readonly Lazy<BugCreateUpdateParamsFactory> mInstance = new Lazy<BugCreateUpdateParamsFactory>(() => new BugCreateUpdateParamsFactory());

    #region Properties

    /// <summary>
    /// Accessor for the singleton instance.
    /// </summary>
    public static BugCreateUpdateParamsFactory Instance { get { return mInstance.Value; } }

    #endregion

    #region Public Methods

    /// <summary>
    /// Creates a new type derived from <see cref="CreateBugParams"/> with additional fields added to the derived
    /// type for each custom field.
    /// </summary>
    /// <param name="customFields">Custom fields the type should contain.</param>
    /// <returns>An instance of the derived type that has the required fields set.</returns>
    /// <remarks>Each type is cached based on the set of custom fields it contains and are re-used where
    /// possible instead of creating types with the same set of fields over and over again.</remarks>
    public CreateBugParams GetCreateBugParamsTypeInstance(BugCustomFields customFields)
    {
      //If there aren't any custom fields, we can use the type we already have
      if (!customFields.Any())
        return new CreateBugParams();

      //Calculate the hash code for each custom field to see if we already have a type defined
      //that can handle it
      int fieldNamesHashCode = GetCustomFieldsHashCode(customFields);

      //If we won't have a type capable of handling this set of custom fields, define one.
      if (!mCreateBugTypes.ContainsKey(fieldNamesHashCode))
      {
        string typeNameFormat = "Bugzilla.Proxies.Bug.Params.CreateNewBugParams{0}";
        Type dynamicType = DefineDynamicType(customFields, typeNameFormat, typeof(CreateBugParams));

        mCreateBugTypes.Add(fieldNamesHashCode, dynamicType);
      }

      //Create an instance of the required type
      return (CreateBugParams)Activator.CreateInstance(mCreateBugTypes[fieldNamesHashCode]);
    }

    /// <summary>
    /// Creates a new dynamic type with additional fields defined for each custom field specified.
    /// </summary>
    /// <param name="customFields">The custom fields to use when creating the new dynamic type.</param>
    /// <returns>A derived instance of <see cref="UpdateBugParam"/> which has additional fields defined for each custom field.</returns>
    public UpdateBugParam GetUpdateBugParamInstance(BugCustomFields customFields)
    {
      //If we don't have any custom fields we can use the existing type
      if (!customFields.Any())
        return new UpdateBugParam();

      //Calculate the hash code for each custom field to see if we already have a type defined
      //that can handle it
      int fieldNamesHashCode = GetCustomFieldsHashCode(customFields);

      if (!mUpdateBugTypes.ContainsKey(fieldNamesHashCode))
      {
        string typeNameFormat = "Bugzilla.Proxies.Bug.Params.UpdateBugParams{0}";
        Type dynamicType = DefineDynamicType(customFields, typeNameFormat, typeof(UpdateBugParam));

        mUpdateBugTypes.Add(fieldNamesHashCode, dynamicType);
      }

      //Create an instance of the required type
      return (UpdateBugParam)Activator.CreateInstance(mUpdateBugTypes[fieldNamesHashCode]);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Private constructor.
    /// </summary>
    private BugCreateUpdateParamsFactory()
    {
      mCreateBugTypes = new Dictionary<int, Type>();
      mUpdateBugTypes = new Dictionary<int, Type>();

      //Create the in-memory module which will hold the dynamically generated types
      AppDomain currDomain = AppDomain.CurrentDomain;
      AssemblyBuilder asmBuilder = currDomain.DefineDynamicAssembly(new AssemblyName("BugDynamicCreateUpdateParams"), AssemblyBuilderAccess.Run);
      mModuleBuilder = asmBuilder.DefineDynamicModule("BugDynamicCreateUpdateParams");
    }

    /// <summary>
    /// Creates and stores a newly created dynamic type.
    /// </summary>
    /// <param name="customFields">Names of custom fields, each one will have a corresponding field on the new dynamic type.</param>
    /// <param name="typeNameFormat">Format of the new type name to create.</param>
    /// <param name="baseType">Base type for the newly defined dynamic type.</param>
    /// <returns>The dynamic type that was created.</returns>
    private Type DefineDynamicType(BugCustomFields customFields, string typeNameFormat, Type baseType)
    {
      Guid typeGuid = Guid.NewGuid();
      string typeName = string.Format(typeNameFormat, typeGuid.ToString());
      TypeBuilder createParamsType = mModuleBuilder.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Class, baseType);

      //Add fields for each custom field specified
      foreach (BugCustomField bzCustomField in customFields)
      {
        Type fieldType = null;

        if (bzCustomField.FieldType == BugFieldDetails.BugFieldType.MultiSelectDropDown)
          fieldType = typeof(object[]);
        else
          fieldType = typeof(object);

        FieldBuilder fldBuilder = createParamsType.DefineField(bzCustomField.FieldName, fieldType, FieldAttributes.Public);

        //Add the XmlRpcMissingMapping attribute to the field to tell the XML-RPC serialiser to ignore null custom fields
        Type missingMappingActionType = typeof(XmlRpcMissingMappingAttribute);
        ConstructorInfo missingMappingActCons = missingMappingActionType.GetConstructor(new Type[]{typeof(MappingAction)});
        fldBuilder.SetCustomAttribute(new CustomAttributeBuilder(missingMappingActCons, new object[]{MappingAction.Ignore}));
      }

      return createParamsType.CreateType();
    }

    /// <summary>
    /// Calculates the hash code for a set of custom field names.
    /// </summary>
    /// <param name="customFields">The custom field names to calculate the has code for.</param>
    /// <returns>Unique hash code for the set of custom field names.</returns>
    private int GetCustomFieldsHashCode(BugCustomFields customFields)
    {
      //Sort the custom fields by name to before generating the type GUID
      string[] fieldNames = customFields.Select(field => field.FieldName.ToUpperInvariant()).ToArray();
      Array.Sort<string>(fieldNames, StringComparer.InvariantCultureIgnoreCase);

      return string.Join("", fieldNames).GetHashCode();
    }

    #endregion
  }
}
