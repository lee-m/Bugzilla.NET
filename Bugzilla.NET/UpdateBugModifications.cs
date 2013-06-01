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
using System.Linq;
using System.Collections.Generic;

using Bugzilla.Proxies.Bug.Responses;
using CookComputing.XmlRpc;

namespace Bugzilla
{
  /// <summary>
  /// Stores a set of modifications made to a field when a bug has been updated.
  /// </summary>
  public class UpdateBugFieldModifications
  {
    //Name of the field
    private string mFieldName;

    /// <summary>
    /// List of added values
    /// </summary>
    private List<string> mAddedValues;

    /// <summary>
    /// List of removed values.
    /// </summary>
    private List<string> mRemovedValues;

    /// <summary>
    /// Constructor to pass in the property values.
    /// </summary>
    /// <param name="fieldName">Name of the field.</param>
    /// <param name="addedValues">List of added values.</param>
    /// <param name="removedValues">List of removed values.</param>
    public UpdateBugFieldModifications(string fieldName,
                                       IEnumerable<string> addedValues,
                                       IEnumerable<string> removedValues)
    {
      mFieldName = fieldName;
      mAddedValues = new List<string>(addedValues);
      mRemovedValues = new List<string>(removedValues);
    }

    /// <summary>
    /// Accessor for the field name.
    /// </summary>
    public string FieldName { get { return mFieldName; } }

    /// <summary>
    /// Set of added values.
    /// </summary>
    public IEnumerable<string> AddedValues { get { return mAddedValues; } }

    /// <summary>
    /// Set of removed values.
    /// </summary>
    public IEnumerable<string> RemovedValues { get { return mRemovedValues; } }
  }

  /// <summary>
  /// Stores a collection of modifications made to a bug when it was updated.
  /// </summary>
  public class UpdateBugModifications
  {
    /// <summary>
    /// ID of the bug that was updated.
    /// </summary>
    private int mBugID;

    /// <summary>
    /// Alias of the bug that was udated
    /// </summary>
    private string mBugAlias;

    /// <summary>
    /// Date/time the change was made.
    /// </summary>
    private DateTime mChangeTime;
    
    /// <summary>
    /// The modifications made to each field.
    /// </summary>
    private List<UpdateBugFieldModifications> mModifications;

    /// <summary>
    /// Constructs a new instance based on the response structure provided.
    /// </summary>
    /// <param name="resp"></param>
    internal UpdateBugModifications(UpdateBugResponseContents resp)
    {
      mBugID = resp.ID;
      mBugAlias = resp.Alias;
      mChangeTime = resp.LastChangeTime;
      mModifications = new List<UpdateBugFieldModifications>();

      foreach (string fieldName in resp.Modifications.Keys)
      {
        XmlRpcStruct changes = (XmlRpcStruct)resp.Modifications[fieldName];

        //Multiple items in the added/removed fields may be comma space separated so split them down
        //into a list of values
        string addedItems = changes["added"].ToString();
        string removedItems = changes["removed"].ToString();

        var splitAddedItems = addedItems.Split(new string[] { ", " }, StringSplitOptions.None).Select(r => r.Trim());
        var splitRemovedItems = removedItems.Split(new string[] { ", " }, StringSplitOptions.None).Select(r => r.Trim());

        mModifications.Add(new UpdateBugFieldModifications(fieldName, splitAddedItems, splitRemovedItems));
      }
    }

    /// <summary>
    /// Accessor for the bug ID.
    /// </summary>
    public int BugID { get { return mBugID; } }

    /// <summary>
    /// Accessor for the bug's alias.
    /// </summary>
    public string BugAlias { get { return mBugAlias; } }

    /// <summary>
    /// Accessor for the date/time the changes were saved.
    /// </summary>
    public DateTime UpdateChangeTime { get { return mChangeTime; } }
  }
}
