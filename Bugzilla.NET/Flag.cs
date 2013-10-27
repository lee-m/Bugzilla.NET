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

using CookComputing.XmlRpc;

namespace Bugzilla
{
  /// <summary>
  /// Details about a flag which is set on a bug or attachment.
  /// </summary>
  public class Flag
  {
    /// <summary>
    /// ID of the flag.
    /// </summary>
    private int mID;

    /// <summary>
    /// Name of the flag.
    /// </summary>
    private string mName;

    /// <summary>
    /// Type ID of the flag.
    /// </summary>
    private int mTypeID;

    /// <summary>
    /// Date/time the flag was created.
    /// </summary>
    private DateTime mCreationTime;

    /// <summary>
    /// Date/time when the flag was last modified.
    /// </summary>
    private DateTime mModificationTime;

    /// <summary>
    /// Status of the flag.
    /// </summary>
    private string mStatus;

    /// <summary>
    /// Login name of the person who set the flag.
    /// </summary>
    private string mSetter;

    /// <summary>
    /// Login name of the user this flag has been requested to be granted or denied.
    /// </summary>
    private string mRequestee;

    /// <summary>
    /// Initialises this instance with details from the remote server.
    /// </summary>
    /// <param name="dets">Details about this flag.</param>
    internal Flag(XmlRpcStruct dets)
    {
      mID = int.Parse(dets["id"].ToString());
      mName = dets["name"].ToString();
      mTypeID = int.Parse(dets["type_id"].ToString());
      mCreationTime = DateTime.Parse(dets["creation_date"].ToString());
      mModificationTime = DateTime.Parse(dets["modification_date"].ToString());
      mStatus = dets["status"].ToString();
      mSetter = dets["setter"].ToString();

      if(dets.ContainsKey("requestee"))
        mRequestee = dets["requestee"].ToString();
    }

    /// <summary>
    /// Accessor for the flag ID.
    /// </summary>
    public int ID { get { return mID; } }

    /// <summary>
    /// Accessor for the flag name.
    /// </summary>
    public string Name { get { return mName; } }

    /// <summary>
    /// Accessor for the flag's type-id.
    /// </summary>
    public int TypeID { get { return mTypeID; } }

    /// <summary>
    /// Accessor for the flag's creation date/time
    /// </summary>
    public DateTime CreationTime { get { return mCreationTime; } }

    /// <summary>
    /// Accessor for the flag's last modification date/time
    /// </summary>
    public DateTime ModificationTime { get { return mModificationTime; } }

    /// <summary>
    /// Accessor for the flag status.
    /// </summary>
    public string Status { get { return mStatus; } }

    /// <summary>
    /// Accessor for the flag's setter.
    /// </summary>
    public string Setter { get { return mSetter; } }

    /// <summary>
    /// Accessor for the requestee who can grant/deny this flag.
    /// </summary>
    public string Requestee { get { return mRequestee; } }
  }
}
