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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bugzilla
{
  /// <summary>
  /// Information about a single custom field on a bug.
  /// </summary>
  public class BugCustomField
  {
    /// <summary>
    /// Name of the field
    /// </summary>
    private string mFieldName;

    /// <summary>
    /// Type of the field.
    /// </summary>
    private BugFieldDetails.BugFieldType mFieldType;

    /// <summary>
    /// Value of the field.
    /// </summary>
    private object mFieldValue;

    /// <summary>
    /// Initialises this instance with the specified field name and type.
    /// <param name="fieldName">Name of the custom field.</param>
    /// <param name="fieldType">Type of the custom field.</param>
    /// </summary>
    public BugCustomField(string fieldName, BugFieldDetails.BugFieldType fieldType) : this(fieldName, fieldType, null)
    {
    }

    /// <summary>
    /// Overloaded constructor to initialise each field.
    /// </summary>
    /// <param name="fieldName">Name of the custom field.</param>
    /// <param name="fieldType">Type of the custom field.</param>
    /// <param name="fieldValue">Value of the custom field.</param>
    public BugCustomField(string fieldName, BugFieldDetails.BugFieldType fieldType, object fieldValue)
    {
      mFieldName = fieldName;
      mFieldType = fieldType;
      mFieldValue = fieldValue;
    }

    /// <summary>
    /// Accessor for the field's name.
    /// </summary>
    public string FieldName { get { return mFieldName; } }

    /// <summary>
    /// Accessor for the field's type.
    /// </summary>
    public BugFieldDetails.BugFieldType FieldType { get { return mFieldType; } }

    /// <summary>
    /// Accessor for the field value.
    /// </summary>
    public object FieldValue 
    {
      get { return mFieldValue; }
      set { mFieldValue = value; } 
    }
  }

  /// <summary>
  /// Collection of custom field details/values for a particular bug.
  /// </summary>
  public class BugCustomFields : IEnumerable<BugCustomField>
  {
    /// <summary>
    /// Field values returned by the remote server for the parent bug.
    /// </summary>
    private Dictionary<string, BugCustomField> mFields;

    /// <summary>
    /// Constructor to pass in the field values.
    /// </summary>
    /// <param name="fields">Custom field values.</param>
    internal BugCustomFields(List<BugCustomField> fields)
    {
      mFields = fields.ToDictionary(field => field.FieldName);
    }

    /// <summary>
    /// Accessor for a typed enumerator.
    /// </summary>
    /// <returns>An enumerator instance.</returns>
    public IEnumerator<BugCustomField> GetEnumerator()
    {
      return mFields.Values.GetEnumerator();
    }

    /// <summary>
    /// Accessor for an untyped enumerator.
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
      return mFields.Values.GetEnumerator();
    }

    /// <summary>
    /// Accessor for the value of a field based on its name.
    /// </summary>
    /// <param name="fieldName">Name of the field to access. Must start with "cf_"</param>
    /// <returns>The value of the specified field.</returns>
    /// <exception cref="KeyNotFoundException">No custom field with the specified key was found.</exception>
    public BugCustomField this[string fieldName]
    {
      get { return mFields[fieldName]; }
      set
      {
        if (!mFields.ContainsKey(fieldName))
          throw new KeyNotFoundException(string.Format("No custom field exists with the name '{0}'", fieldName));

        mFields[fieldName] = value;
      }
    }

    /// <summary>
    /// Accessor for the set of custom field names.
    /// </summary>
    public IEnumerable<BugCustomField> Fields { get { return mFields.Values; } }
  }
}
