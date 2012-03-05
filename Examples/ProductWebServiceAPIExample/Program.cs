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
using Bugzilla;

namespace Bugzilla.Examples
{
  class Program
  {
    static void Main(string[] args)
    {
      BugzillaServer server = new BugzillaServer("localhost", string.Empty, "user@example.com", "password", false);
      
      //Get products example
      Product exampleProd = server.GetProduct(1);
      Console.WriteLine("Product 1:");
      Console.WriteLine("Description: {0}", exampleProd.Description);
      Console.WriteLine("ID: {0}", exampleProd.Id);
      Console.WriteLine("Name: {0}", exampleProd.Name);
      Console.WriteLine();

      //Accessible products
      Console.WriteLine("Accessible Products:");

      foreach (Product accessibleProd in server.AccessibleProducts)
      {
        Console.WriteLine("Description: {0}", accessibleProd.Description);
        Console.WriteLine("ID: {0}", accessibleProd.Id);
        Console.WriteLine("Name: {0}", accessibleProd.Name);
      }

      Console.WriteLine();

      //Enterable products
      Console.WriteLine("Enterable Products:");

      foreach (Product enterableProd in server.EnterableProducts)
      {
        Console.WriteLine("Description: {0}", enterableProd.Description);
        Console.WriteLine("ID: {0}", enterableProd.Id);
        Console.WriteLine("Name: {0}", enterableProd.Name);
      }

      Console.WriteLine();

      //Selectable products
      Console.WriteLine("Selectable Products:");

      foreach (Product selectableProd in server.SelectableProducts)
      {
        Console.WriteLine("Description: {0}", selectableProd.Description);
        Console.WriteLine("ID: {0}", selectableProd.Id);
        Console.WriteLine("Name: {0}", selectableProd.Name);
      }

      Console.WriteLine();

      Console.Read();
    }

  }
}
