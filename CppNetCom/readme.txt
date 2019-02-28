Sample to use .NET COM object from C++ code
1. .NET classes should have [ComVisible(true)] attribute
2. C++ should have:
2.1 #import "..\NetCom\bin\Debug\NetCom.tlb" raw_interfaces_only //It generated netcom.tlh header
2.2 #include "Debug\netcom.tlh" //
2.3 NetCom::IMyClassPtr ptr; //Smart pointer generated in netcom.tlh
2.4 hr = ptr.CreateInstance(__uuidof(NetCom::Class1)); //Instantinate smart pointer