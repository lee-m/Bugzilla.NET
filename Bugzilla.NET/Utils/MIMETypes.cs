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
using System.Xml.Linq;
using System.Runtime.InteropServices;
using System.IO;

namespace Bugzilla.Utils
{
  /// <summary>
  /// Helper class for determining the MIME type of a file given it's extension.
  /// </summary>
  static internal class MIMETypes
  {
    /// <summary>
    /// Mapping from extension to its MIME type.
    /// </summary>
    private static Dictionary<string, string> mMimeTypes;

    /// <summary>
    /// P/Invoke method to determine the MIME type from the file. Only used if the extension doesn't
    /// exist in our own database.
    /// </summary>
    /// <returns></returns>
    [DllImport(@"urlmon.dll", CharSet = CharSet.Auto)]
    private extern static System.UInt32 FindMimeFromData(
        System.UInt32 pBC,
        [MarshalAs(UnmanagedType.LPStr)] System.String pwzUrl,
        [MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer,
        System.UInt32 cbSize,
        [MarshalAs(UnmanagedType.LPStr)] System.String pwzMimeProposed,
        System.UInt32 dwMimeFlags,
        out System.UInt32 ppwzMimeOut,
        System.UInt32 dwReserverd
    );

    /// <summary>
    /// Parses the MIME types XML listing.
    /// </summary>
    static MIMETypes()
    {
      mMimeTypes = new Dictionary<string, string>();

      //Parse the data from the XML file.
      XElement mimeTypesFile = XElement.Load(new StringReader(Bugzilla.Properties.Resources.MIMETypes));

      foreach (var elem in mimeTypesFile.Elements("MIMEType"))
        mMimeTypes.Add((string)elem.Attribute("extension"), (string)elem.Attribute("type"));
    }

    /// <summary>
    /// Gets the MIME type from a file based on its extension.
    /// </summary>
    /// <param name="fileName">Name of the file to get the MIME type from.</param>
    /// <returns></returns>
    public static string GetMIMEType(string fileName)
    {
      if(string.IsNullOrEmpty(fileName))
        throw new ArgumentNullException("fileName");

      string extension = Path.GetExtension(fileName);

      //If the extension could not be determined, or if we don't have an entry for this 
      //particular extension than fall back to the P/Invoke method
      if (string.IsNullOrEmpty(extension))
        return GetMIMETypeViaPInvoke(fileName);

      //Strip of the leading '.' and lookup the MIME type
      extension = extension.Replace(".", string.Empty);
      return mMimeTypes[extension];
    }

    /// <summary>
    /// Attempts to determine the MIME type for a file via the P/Invoke FindMimeFromData function.
    /// </summary>
    /// <param name="fileName">File name to get the MIME type for.</param>
    /// <returns></returns>
    private static string GetMIMETypeViaPInvoke(string fileName)
    {
      if (!File.Exists(fileName))
        throw new FileNotFoundException(fileName + " not found");
    
      //FindMimeFromData only needs the first 256 bytes
      byte[] buffer = new byte[256];

      using (FileStream fs = new FileStream(fileName, FileMode.Open))
      {
        if (fs.Length >= 256)
          fs.Read(buffer, 0, 256);
        else
          fs.Read(buffer, 0, (int)fs.Length);
      }

      try
      {
        UInt32 mimetype;
        FindMimeFromData(0, null, buffer, 256, null, 0, out mimetype, 0);

        IntPtr mimeTypePtr = new IntPtr(mimetype);
        string mime = Marshal.PtrToStringUni(mimeTypePtr);
        Marshal.FreeCoTaskMem(mimeTypePtr);
        return mime;
      }
      catch
      {
        return "unknown/unknown";
      }
    }
  }
}
