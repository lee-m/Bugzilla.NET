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

using CookComputing.XmlRpc;

namespace Bugzilla.Proxies.Bug.Responses
{
  internal struct GetFieldsResponse
  {
    /// <summary>
    /// The set of fields and any valid values that are allowed for those fields.
    /// </summary>
    [XmlRpcMember("fields")]
    public BugField[] Fields;
  }

  /// <summary>
  /// Represents details of a single field returned from the remote server.
  /// </summary>
  [XmlRpcMissingMapping(MappingAction.Ignore)]
  internal struct BugField
  {
    /// <summary>
    /// Internal ID of this field.
    /// </summary>
    [XmlRpcMember("id")]
    public int FieldID;

    /// <summary>
    /// The type of this field.
    /// </summary>
    [XmlRpcMember("type")]
    public int Type;

    /// <summary>
    /// Whether this field is a custom field or not.
    /// </summary>
    [XmlRpcMember("is_custom")]
    public bool IsCustomField;

    /// <summary>
    /// The internal name of the field.
    /// </summary>
    [XmlRpcMember("name")]
    public string InternalName;

    /// <summary>
    /// The display name of the field as it would appear in the user interface.
    /// </summary>
    [XmlRpcMember("display_name")]
    public string DisplayName;

    /// <summary>
    /// Whether this field's value must be set.
    /// </summary>
    [XmlRpcMember("is_mandatory")]
    public bool Mandatory;

    /// <summary>
    /// For custom fields, whether the field can be set on new bug entry or not.
    /// </summary>
    [XmlRpcMember("is_on_bug_entry")]
    public bool CanBeSetOnBugEntry;

    /// <summary>
    /// The name of a field that controls the visibility of this field in the user interface.
    /// </summary>
    [XmlRpcMember("visibility_field")]
    public string VisibilityField;

    /// <summary>
    /// When <see cref="VisibilityField"/> is set to one of these values then the field will be shown.
    /// </summary>
    [XmlRpcMember("visibility_values")]
    public string[] VisibilityValues;

    /// <summary>
    /// The name of the field that controls whether or not particular values of the field are shown in the user interface.
    /// </summary>
    [XmlRpcMember("value_field")]
    public string ValueField;

    /// <summary>
    /// The legal values for field which provide a drop-down or multi-select list.
    /// </summary>
    [XmlRpcMember("values")]
    public FieldValue[] Values;
  }

  /// <summary>
  /// Details of a single field.
  /// </summary>
  [XmlRpcMissingMapping(MappingAction.Ignore)]
  internal struct FieldValue
  {
    /// <summary>
    /// The value which would be entered for this field.
    /// </summary>
    [XmlRpcMember("name")]
    public string Name;

    /// <summary>
    /// Sort key for the field.
    /// </summary>
    [XmlRpcMember("sortkey")]
    public int SortKey;

    /// <summary>
    /// If <see cref="BugField.ValueField"/> is set, this value will only be shown when that value is set to one
    /// of the values contained here.
    /// </summary>
    [XmlRpcMember("visibility_values")]
    public string[] VisibilityValues;

    /// <summary>
    /// For bug status fields, indicates if the value is for an "open" bug status or not.
    /// </summary>
    [XmlRpcMember("is_open")]
    public bool? IsOpenStatus;

    /// <summary>
    /// For bug status fields, the set of status transitions which are valid form this status.
    /// </summary>
    [XmlRpcMember("can_change_to")]
    public StatusTransition[] StatusTransitions;
  }

  /// <summary>
  /// Details of a valid status transition from a bug's current status.
  /// </summary>
  internal struct StatusTransition
  {
    /// <summary>
    /// Name of the new status.
    /// </summary>
    [XmlRpcMember("name")]
    public string Name;

    /// <summary>
    /// Whether a comment is required when changing to this status.
    /// </summary>
    [XmlRpcMember("comment_required")]
    public bool CommentRequired;
  }
}
