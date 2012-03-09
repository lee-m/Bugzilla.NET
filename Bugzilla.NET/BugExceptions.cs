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

namespace Bugzilla
{
  /// <summary>
  /// Thrown when an invalid bug ID or alias is specified.
  /// </summary>
  public class InvalidBugIDOrAliasException : ApplicationException
  {
    /// <summary>
    /// Creates a new instance of this class with the specified id/alias in the exception message.
    /// </summary>
    /// <param name="bugIDOrAlias">The bug ID or alias which triggered the error.</param>
    public InvalidBugIDOrAliasException(string bugIDOrAlias)
      : base(string.Format("No bug with ID or alias {0}", bugIDOrAlias))
    {
    }

    /// <summary>
    /// Creates a new instance of this class with a generic exception message.
    /// </summary>
    public InvalidBugIDOrAliasException()
      : base("One or more invalid bug IDs and/or aliases specified.")
    {
    }
  }

  /// <summary>
  /// Thrown when an attempt is made to perform an operation when the logged in user has insufficient security privilages
  /// for the operation attempted.
  /// </summary>
  public class BugEditAccessDeniedException : ApplicationException
  {
    /// <summary>
    /// Creates a new instance of this class.
    /// </summary>
    /// <param name="bugIDOrAlias">Bug ID or alias which the user attempted to edit.</param>
    public BugEditAccessDeniedException(string bugIDOrAlias)
      : base(string.Format("Currently logged in user does not have rights to edit big {0}.", bugIDOrAlias))
    {
    }
  }

  /// <summary>
  /// Thrown when an attempt is made to attach a file whose size exceed the max allowed on the server.
  /// </summary>
  public class AttachmentTooLargeException : ApplicationException
  {
    /// <summary>
    /// Default constructor.
    /// </summary>
    public AttachmentTooLargeException()
      : base("Size of attachment exceeds max allowed.")
    {
    }
  }

  /// <summary>
  /// Thrown when an invalid MIME type is specified when adding an attachment to a bug.
  /// </summary>
  public class InvalidMIMETypeException : ApplicationException
  {
    /// <summary>
    /// Creates a new instance of this class.
    /// </summary>
    /// <param name="mimeType">The invalid MIME type which was specified.</param>
    public InvalidMIMETypeException(string mimeType)
      : base(string.Format("Invalid MIME type '{0}' specified for attachment.", mimeType))
    {
    }
  }

  /// <summary>
  /// Thrown when an attempt is made to attach a URL to a bug when the attachment data is invalid.
  /// </summary>
  public class InvalidAttachmentURLException : ApplicationException
  {
    /// <summary>
    /// Creates a new instance of this class.
    /// </summary>
    public InvalidAttachmentURLException()
      : base("Invalid URL data specified for attachment.")
    {
    }
  }

     /// <summary>
  /// Thrown when an attempt is made to attach a URL to a bug when URL attaching is disabled.
  /// </summary>
  public class URLAttachmentsDisabledException : ApplicationException
  {
    /// <summary>
    /// Creates a new instance of this class.
    /// </summary>
    public URLAttachmentsDisabledException()
      : base("Attaching of URLs not supported.")
    {
    }
  }

  /// <summary>
  /// Thrown when the logged in user has insufficient security privilages for an operation.
  /// </summary>
  public class InsufficientSecurityPrivilagesException : ApplicationException
  {
    /// <summary>
    /// Default constructor.
    /// </summary>
    public InsufficientSecurityPrivilagesException()
      : base("Insufficient security privilages for requested operation.")
    {
    }
  }

  /// <summary>
  /// Thrown when attempting to get comments with an invalid comment ID.
  /// </summary>
  public class InvalidCommentIDException : ApplicationException
  {
    /// <summary>
    /// Default constructor.
    /// </summary>
    public InvalidCommentIDException()
      : base("One or more of the specified comment IDs do not exist.")
    {
    }
  }

  /// <summary>
  /// Thrown when an attempt is made to get a private comment when the logged in user does not have
  /// the necessary privilages to see private comments.
  /// </summary>
  public class CommentAccessDeniedException : ApplicationException
  {
    /// <summary>
    /// Default constructor.
    /// </summary>
    public CommentAccessDeniedException()
      : base("One or more of the requested comments are not accessible to the current user.")
    {
    }
  }

  /// <summary>
  /// Thrown when an attempt is made to get a private attachment when the logged in user does not have
  /// the necessary privilages to see private attachment.
  /// </summary>
  public class AttachmentAccessDeniedException : ApplicationException
  {
    /// <summary>
    /// Default constructor.
    /// </summary>
    public AttachmentAccessDeniedException()
      : base("One or more of the requested attachments are not accessible to the current user.")
    {
    }
  }

  /// <summary>
  /// Thrown when an attempt is made to get data for a bug when the logged in user does not have
  /// the necessary privilages to see that bug.
  /// </summary>
  public class BugAccessDeniedException : ApplicationException
  {
    /// <summary>
    /// Default constructor.
    /// </summary>
    public BugAccessDeniedException()
      : base("One or more of the requested bugs are not accessible to the current user.")
    {
    }
  }
}
