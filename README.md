# CSharp-UnmanagedCPP
## Sample of a quick & C++ UnManaged DLL and C# console
1) C++ UnManaged DLL has a method "INIT" that as parameter receives a CallBack function. On calling INIT, we also start a timer that every 12 seconds will fire an event and call that CallBack procedure. The second method is "STOP" which stops the the Timer. The third method is the CallBack procedure that should send a struct with 2 fields (does not matter what types) and the Current Time.

2) Managed C# console that uses the C++ DLL. The console app should call the C++ INIT passing the CallBack procedure and on Close should call the C++ STOP method. The CallBack procedure should display the values
