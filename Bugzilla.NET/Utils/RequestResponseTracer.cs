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
using System.IO;
using System.Diagnostics;

using CookComputing.XmlRpc;

namespace Bugzilla.Utils
{
  /// <summary>
  /// XML-RPC request/response tracer to output the XML to the debug window.
  /// </summary>
  internal class RequestResponseTracer : XmlRpcLogger
  {
    /// <summary>
    /// Dumps the request stream XML to the output window.
    /// </summary>
    /// <param name="sender">Ignored.</param>
    /// <param name="e">Event args.</param>
    protected override void OnRequest(object sender, XmlRpcRequestEventArgs e)
    {
      DumpStream(e.RequestStream);
    }

    /// <summary>
    /// Dumps the response XML to the output window.
    /// </summary>
    /// <param name="sender">Ignored.</param>
    /// <param name="e">Event info.</param>
    protected override void OnResponse(object sender, XmlRpcResponseEventArgs e)
    {
      DumpStream(e.ResponseStream);
    }

    /// <summary>
    /// Dumps the contents of the specified stream to the output window.
    /// </summary>
    /// <param name="stream">Stream to output.</param>
    private void DumpStream(Stream stream)
    {
      stream.Position = 0;

      TextReader trdr = new StreamReader(stream);
      String s = trdr.ReadLine();

      while (s != null)
      {
        Debug.WriteLine(s);
        s = trdr.ReadLine();
      }
    }
  }
}
