//Copyright (C) 2013 by Lee Millward

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
  /// Stores details of a single parameter of the remote server.
  /// </summary>
  public struct Parameter
  {
    /// <summary>
    /// Name of the parameter.
    /// </summary>
    private readonly string mParamName;

    /// <summary>
    /// Value of the parameter.
    /// </summary>
    private readonly object mParamValue;

    /// <summary>
    /// Creates a new instance with the specified name and value.
    /// </summary>
    /// <param name="name">Name of the parameter.</param>
    /// <param name="value">Value of the parameter.</param>
    internal Parameter(string name, object value)
    {
      mParamName = name;
      mParamValue = value;
    }

    /// <summary>
    /// Accessor for the parameter name.
    /// </summary>
    public string Name { get { return mParamName; } }

    /// <summary>
    /// Accessor for the parameter value.
    /// </summary>
    public object Value { get { return mParamValue; } }
  }
}
