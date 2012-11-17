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
  public class InvalidBugIDOrAliasException : BugzillaException
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
  public class BugEditAccessDeniedException : BugzillaException
  {
    /// <summary>
    /// Creates a new instance of this class.
    /// </summary>
    /// <param name="bugIDOrAlias">Bug ID or alias which the user attempted to edit.</param>
    public BugEditAccessDeniedException(string bugIDOrAlias)
      : base(string.Format("Currently logged in user does not have rights to edit big {0}.", bugIDOrAlias))
    {
    }

    /// <summary>
    /// Default constructor.
    /// </summary>
    public BugEditAccessDeniedException()
      : base("Currently logged in user does not have rights to edit one or more of the specified bugs.")
    {
    }
  }

  /// <summary>
  /// Thrown when an attempt is made to attach a file whose size exceed the max allowed on the server.
  /// </summary>
  public class AttachmentTooLargeException : BugzillaException
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
  public class InvalidMIMETypeException : BugzillaException
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
  public class InvalidAttachmentURLException : BugzillaException
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
  public class URLAttachmentsDisabledException : BugzillaException
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
  public class InsufficientSecurityPrivilagesException : BugzillaException
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
  public class InvalidCommentIDException : BugzillaException
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
  public class CommentAccessDeniedException : BugzillaException
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
  public class AttachmentAccessDeniedException : BugzillaException
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
  public class BugAccessDeniedException : BugzillaException
  {
    /// <summary>
    /// Default constructor.
    /// </summary>
    public BugAccessDeniedException()
      : base("One or more of the requested bugs are not accessible to the current user.")
    {
    }
  }

  /// <summary>
  /// Thrown when an attempt is made to create/update a bug and set a field which must be set to a blank value.
  /// </summary>
  public class FieldCannotBeBlankException : BugzillaException
  {
    /// <summary>
    /// Overloaded constructor to pass in the remote error message.
    /// </summary>
    /// <param name="serverMessage">Error message from the server containing specific error details.</param>
    public FieldCannotBeBlankException(string serverMessage)
      : base(serverMessage)
    {
    }
  }

  /// <summary>
  /// Thrown when an attempt is made to set a field to an invalid value.
  /// </summary>
  public class InvalidFieldValueException : BugzillaException
  {
    /// <summary>
    /// Overloaded constructor to pass in the remote error message.
    /// </summary>
    /// <param name="serverMessage">Error message from the server containing specific error details.</param>
    public InvalidFieldValueException(string serverMessage)
      : base(serverMessage)
    {
    }
  }

  /// <summary>
  /// Thrown when an invalid custom field name is specified.
  /// </summary>
  public class InvalidCustomFieldNameException : BugzillaException
  {
    /// <summary>
    /// Overloaded constructor to pass in the remote error message.
    /// </summary>
    /// <param name="serverMessage">Error message from the server containing specific error details.</param>
    public InvalidCustomFieldNameException(string serverMessage)
      : base(serverMessage)
    {
    }
  }

  /// <summary>
  /// Thrown when the alias specified when creating a new bug was invalid.
  /// </summary>
  public class InvalidNewBugAliasException : BugzillaException
  {
    /// <summary>
    /// Overloaded constructor to pass in the remote error message.
    /// </summary>
    /// <param name="serverMessage">Error message from the server containing specific error details.</param>
    public InvalidNewBugAliasException(string serverMessage)
      : base(serverMessage)
    {
    }
  }

  /// <summary>
  /// Thrown when an attempt is made to create a bug against a non-existent or inaccessible product.
  /// </summary>
  public class InvalidProductException : BugzillaException
  {
    /// <summary>
    /// Overloaded constructor to pass in the remote error message.
    /// </summary>
    /// <param name="serverMessage">Error message from the server containing specific error details.</param>
    public InvalidProductException(string serverMessage)
      : base(serverMessage)
    {
    }
  }

  /// <summary>
  /// Thrown when an atrempt is made to create a new bug with a blocks and/or depends on list which would
  /// result in a cyclic dependency.
  /// </summary>
  public class CyclicBugDependenciesException : BugzillaException
  {
    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="message">Error message from the server containing specific error details.</param>
    public CyclicBugDependenciesException(string message) 
      : base(message)
    {
    }
  }

  /// <summary>
  /// Thrown when an atrempt is made to mark a bug as a duplicate of another bug which would result in a infinite
  /// loop of duplicates.
  /// </summary>
  public class CyclicBugDuplicateException : BugzillaException
  {
    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="message">Error message from the server containing specific error details.</param>
    public CyclicBugDuplicateException(string message)
      : base(message)
    {
    }
  }

  /// <summary>
  /// Thrown when an attempt is made to restrict a new bug to a group which doesn't exist or cannot
  /// be used with the specified product.
  /// </summary>
  public class GroupRestrictionDeniedException : BugzillaException
  {
    /// <summary>
    /// Default constructor.
    /// </summary>
    public GroupRestrictionDeniedException()
      : base("Non-existent or invalid group name for the specified product.")
    {
    }
  }

  /// <summary>
  /// Thrown when an invalid user is specified for a bug's assignee, QA contact or CC list.
  /// </summary>
  public class InvalidUserException : BugzillaException
  {
    /// <summary>
    /// Overloaded constructor to pass in the remote error message.
    /// </summary>
    /// <param name="serverMessage">Error message from the server containing specific error details.</param>
    public InvalidUserException(string serverMessage)
      : base(serverMessage)
    {
    }
  }

  /// <summary>
  /// Thrown when an invalid URL is passed when updating the see also field.
  /// </summary>
  public class InvalidSeeAlsoURLException : BugzillaException
  {
    /// <summary>
    /// Default constructor.
    /// </summary>
    public InvalidSeeAlsoURLException()
      : base("Invalid see also URL specified.")
    {
    }
  }

  /// <summary>
  /// Thrown when an invalid keyword is specified for a bug.
  /// </summary>
  public class InvalidKeywordException : BugzillaException
  {
    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="msg">Exception message text.</param>
    public InvalidKeywordException(string msg)
      : base(msg)
    {
    }
  }

  /// <summary>
  /// Thrown when the currenltly logged in user does not have security access to edit the see also on a particular bug.
  /// </summary>
  public class SeeAlsoEditAccessDenied : BugzillaException
  {
    /// <summary>
    /// Default constructor.
    /// </summary>
    public SeeAlsoEditAccessDenied()
      : base("Currently logged in user has insufficient security rights to edit the see also field.")
    {
    }
  }

  /// <summary>
  /// Thrown when an attempt is made to set a bug field to an invalid value.
  /// </summary>
  public class InvalidBugFieldValueException : BugzillaException
  {
    /// <summary>
    /// Overloaded constructor to provide message text.
    /// </summary>
    /// <param name="message">Message text.</param>
    public InvalidBugFieldValueException(string message)
      : base(message)
    {
    }
  }

  /// <summary>
  /// Thrown when an invalid attempt is made to change a bug's resolution.
  /// </summary>
  public class InvalidBugResolutionChangeException : BugzillaException
  {
    /// <summary>
    /// Overloaded constructor to provide message text.
    /// </summary>
    /// <param name="message">Message text.</param>
    public InvalidBugResolutionChangeException(string message)
      : base(message)
    {
    }
  }

  /// <summary>
  /// Thrown when an attempt is made to change a bug's status to a new status which is not allowed by the workflow rules.
  /// </summary>
  public class InvalidBugStatusTransitionException : BugzillaException
  {
    /// <summary>
    /// Overloaded constructor to provide message text.
    /// </summary>
    /// <param name="message">Message text.</param>
    public InvalidBugStatusTransitionException(string message)
      : base(message)
    {
    }
  }

  /// <summary>
  /// Thrown when an attempt is made to change the groups on a bug when the logged in user does not have
  /// the required security access.
  /// </summary>
  public class GroupEditAccessDeniedException : BugzillaException
  {
    /// <summary>
    /// Overloaded constructor to provide message text.
    /// </summary>
    /// <param name="message">Message text.</param>
    public GroupEditAccessDeniedException(string message)
      : base(message)
    {
    }
  }
}
