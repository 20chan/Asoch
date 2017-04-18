# Asoch

Minimalized simple async socket library

## Usage

```csharp
Client c = new Client("localhost", 8080);
c.Received += (buffer) => Console.WriteLine(Encoding.UTF8.GetString(buffer))
c.Connect();
c.Send("It works!");
```

```csharp
Server s = new Server(8080);
s.Connected += (client) => Console.WriteLine($"{client.IP} Connected!")
s.Received += (buffer) => Console.WriteLine(Encoding.UTF8.GetString(buffer))
s.Run();
```