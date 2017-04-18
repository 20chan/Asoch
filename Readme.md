# Asoch

Minimalized simple async socket library

## Usage

```csharp
Client c = new Client("localhost", 8080);
c.Received += async (buffer) => await Task.Run(() => Console.WriteLine(Encoding.UTF8.GetString(buffer)));
await c.Connect();
await c.Send("It works!");
```

```csharp
Server s = new Server(8080);
s.Connected += async (client) => await Task.Run(() => Console.WriteLine($"{client.IP} Connected!"));
s.Received += async (client, buffer) => await Task.Run(() => Console.WriteLine(Encoding.UTF8.GetString(buffer)));
await s.Run();
```