using System;
using System.Collections.Generic;
using System.Linq;
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

using System.Text;

namespace Bugzilla
{
  /// <summary>
  /// Represents a single field that can be set on a bug.
  /// </summary>
  public class BugField
  {
    /// <summary>
    /// Possible field types.
    /// </summary>
    public enum BugFieldType
    {
      /// <summary>
      /// Unknown field type.
      /// </summary>
      Unknown = 0,

      /// <summary>
      /// Free text field.
      /// </summary>
      FreeText = 1,

      /// <summary>
      /// Single-select drop-down list
      /// </summary>
      DropDown = 2,

      /// <summary>
      /// Multi-select drop-down list.
      /// </summary>
      MultiSelectDropDown = 3,

      /// <summary>
      /// Large text field.
      /// </summary>
      LargeTextBox = 4,

      /// <summary>
      /// Date/time field.
      /// </summary>
      DateTime = 5,

      /// <summary>
      /// Bug ID type field.
      /// </summary>
      BugID = 6,

      /// <summary>
      /// Bug URLs type field.
      /// </summary>
      BugURLs = 7
    }

    /// <summary>
    /// Represents a possible valid value that a bug field can be set to.
    /// </summary>
    public class BugFieldValidValue
    {
      /// <summary>
      /// The actual value.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// The item's sort key.
      /// </summary>
      public int SortKey { get; set; }

      /// <summary>
      /// The set of values which controls whether the value is visible or not.
      /// </summary>
      public IEnumerable<string> VisibilityValues { get; set; }

      /// <summary>
      /// Whether this value represents an "Open" bug status - only valid for the bug status field.
      /// </summary>
      public bool IsOpenStatus { get; set; }

      /// <summary>
      /// The set of valid status transitions form this bug status. Only valid for bug status field.
      /// </summary>
      public IEnumerable<BugStatusTransition> ValidStatusTransitions { get; set; }
    }

    /// <summary>
    /// A status transition from one bug status to another.
    /// </summary>
    public struct BugStatusTransition
    {
      /// <summary>
      /// Value of the new status.
      /// </summary>
      public string NewStatus { get; set; }

      /// <summary>
      /// Whether this status transition requires a comment to be applied or not.
      /// </summary>
      public bool CommentRequired { get; set; }
    }

    /// <summary>
    /// ID of the field.
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// The type of this field.
    /// </summary>
    public BugFieldType FieldType { get; set; }

    /// <summary>
    /// Whether this field is a custom field or not.
    /// </summary>
    public bool IsCustomField { get; set; }

    /// <summary>
    /// The internal name of the field.
    /// </summary>
    public string InternalName { get; set; }

    /// <summary>
    /// Display name of the field.
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// Whether this field is mandatory or not.
    /// </summary>
    public bool Mandatory { get; set; }

    /// <summary>
    /// For custom fields, whether the field is valid when creating a new bug or not.
    /// </summary>
    public bool ShownOnBugEntry { get; set; }

    /// <summary>
    /// The name of a field that controls the visibility of this field in the user interface
    /// </summary>
    public string VisibilityField { get; set; }

    /// <summary>
    /// This field is only shown when <see cref="VisibilityField"/> matches one of these values
    /// </summary>
    public IEnumerable<string> VisibilityValues { get; set; }

    /// <summary>
    /// The name of the field that controls whether or not particular values of the field are shown in the user interface.
    /// </summary>
    public string ValueField { get; set; }
  }
}
