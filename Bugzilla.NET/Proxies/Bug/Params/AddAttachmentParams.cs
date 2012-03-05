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

namespace Bugzilla.Proxies.Bug.Params
{
  /// <summary>
  /// Parameters required when adding an attachment to a bug.
  /// </summary>
  internal struct AddAttachmentParams
  {
    /// <summary>
    /// List of bug IDs or aliases to add the attachment to.
    /// </summary>
    [XmlRpcMember("ids")]
    public string[] IdsOrAliases;

    /// <summary>
    /// Base64 encoded data of the attachment.
    /// </summary>
    [XmlRpcMember("data")]
    public byte[] Data;

    /// <summary>
    /// The 'file name' that will be shown in the UI.
    /// </summary>
    [XmlRpcMember("file_name")]
    public string FileName;

    /// <summary>
    /// Short description for the attachment.
    /// </summary>
    [XmlRpcMember("summary")]
    public string Summary;

    /// <summary>
    /// MIME type of the attachment.
    /// </summary>
    [XmlRpcMember("content_type")]
    public string MIMEType;

    /// <summary>
    /// Comment text to add along with the attachment.
    /// </summary>
    [XmlRpcMember("comment")]
    public string Comment;

    /// <summary>
    /// Whether Bugzilla should treat the attachment as a patch.
    /// </summary>
    [XmlRpcMember("is_patch")]
    public bool IsPatch;

    /// <summary>
    /// Whether the attachment is private or not.
    /// </summary>
    [XmlRpcMember("is_private")]
    public bool IsPrivate;
  }
}
