# Asoch

Minimalized simple async socket library

## Usage

```csharp
Client c = new Client("localhost", 8080);
await c.Connect();
await c.Send("It works!");
Console.WriteLine(Encoding.UTF8.GetString(await c.Receive()));
```

