/****************************** Module Header ******************************\
Module Name:  NativeMethods.h
Project:      CppCLINativeDllWrapper
Copyright (c) Microsoft Corporation.

The code in this file declares the C++/CLI wrapper classes that allow you 
to call from any .NET code to the functions exported by a native C++ DLL 
module.

  CSCallNativeDllWrapper/VBCallNativeDllWrapper (any .NET clients)
          -->
      CppCLINativeDllWrapper (this C++/CLI wrapper)
              -->
          CppDynamicLinkLibrary (a native C++ DLL module)

The NativeMethods class wraps the global functions exported by 
CppDynamicLinkLibrary.dll.

The interoperability features supported by Visual C++/CLI offer a 
particular advantage over other .NET languages when it comes to 
interoperating with native modules. Apart from the traditional explicit 
P/Invoke, C++/CLI allows implicit P/Invoke, also known as C++ Interop, or 
It Just Work (IJW), which mixes managed code and native code almost 
invisibly. The feature provides better type safety, easier coding, greater  
performance, and is more forgiving if the native API is modified. You can 
use the technology to build .NET wrappers for native C++ classes and 
functions if their source code is available, and allow any .NET clients to 
access the native C++ classes and functions through the wrappers.

This source is subject to the Microsoft Public License.
See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/

#pragma once

// Include the header file of the native C++ module.
#include "CppDynamicLinkLibrary.h"

using namespace System;
using namespace System::Runtime::InteropServices;


namespace CppCLINativeDllWrapper
{
    /// <summary>
    /// Function delegate of the 'PFN_COMPARE' callback function. 
    /// </summary>
    /// <remarks>
    /// The delegate type has the UnmanagedFunctionPointerAttribute. Using 
    /// this attribute, you can specify the calling convention of the native 
    /// function pointer type. In the native API's header file, the callback 
    /// PFN_COMPARE is defined as __stdcall (CALLBACK), so here we specify 
    /// CallingConvention::StdCall.
    /// </remarks>
    [UnmanagedFunctionPointer(CallingConvention::StdCall)]
    public delegate int CompareCallback(int a, int b);


    /// <summary>
    /// This C++/CLI class wraps the functions exported by the native C++ 
    /// module CppDynamicLinkLibrary.dll. 
    /// </summary>
    public ref class NativeMethods
    {
    public:
        static int GetStringLength1(String ^ str);
        static int GetStringLength2(String ^ str);
        static int Max(int a, int b, CompareCallback ^ cmpFunc);
    };
}