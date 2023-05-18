using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

class Program
{
    static async Task Main(string[] args)
    {
        var serviceUrl = "http://localhost:8000";
        var tableName = "TabelaExemplo"; 

        var config = new AmazonDynamoDBConfig
        {
            ServiceURL = serviceUrl,
            UseHttp = true
        };
        var client = new AmazonDynamoDBClient(config);

        if (!Table.TryLoadTable(client, tableName, out var table))
        {
            await CreateTableAsync(client, tableName);
            table = Table.LoadTable(client, tableName);
        }        

        await PutItemExampleAsync(table);
        var id = await ScanTableExampleAsync(table);
        await GetItemExampleAsync(table, id);

        Console.WriteLine("Pressione qualquer tecla para sair...");
        Console.ReadKey();
    }

    static async Task CreateTableAsync(AmazonDynamoDBClient client, string tableName)
    {
        var request = new CreateTableRequest
        {
            TableName = tableName,
            AttributeDefinitions = new List<AttributeDefinition>
            {
                new AttributeDefinition("Id", ScalarAttributeType.S)
            },
            KeySchema = new List<KeySchemaElement>
            {
                new KeySchemaElement("Id", KeyType.HASH)
            },
            ProvisionedThroughput = new ProvisionedThroughput
            {
                ReadCapacityUnits = 5,
                WriteCapacityUnits = 5
            }
        };

        await client.CreateTableAsync(request);

        await WaitUntilTableExistsAsync(client, tableName);
    }

    static async Task WaitUntilTableExistsAsync(AmazonDynamoDBClient client, string tableName)
    {
        var request = new DescribeTableRequest
        {
            TableName = tableName
        };

        while (true)
        {
            var response = await client.DescribeTableAsync(request);
            if (response.Table.TableStatus == TableStatus.ACTIVE)
            {
                Console.WriteLine("Tabela criada com sucesso.");
                break;
            }

            Console.WriteLine("Aguardando a criação da tabela...");
            await Task.Delay(1000);
        }
    }

    static async Task PutItemExampleAsync(Table table)
    {
        var id = Guid.NewGuid();
        var item = new Document();
        item["Id"] = id;
        item["Nome"] = $"Nome {id.ToString().Substring(5)}";
        item["Descricao"] = "Isso é apenas um exemplo.";

        await table.PutItemAsync(item);

        Console.WriteLine("Item inserido na tabela.");
    }

    static async Task GetItemExampleAsync(Table table, Guid id)
    {
        var item = await table.GetItemAsync(id);

        Console.WriteLine($"Item encontrado na tabela: Id={item["Id"]}, Nome={item["Nome"]}, Descrição={item["Descricao"]}");
    }

    static async Task<Guid> ScanTableExampleAsync(Table table)
    {
        var scanFilter = new ScanFilter();
        var search = table.Scan(scanFilter);
        var id = Guid.Empty;

        Console.WriteLine("Itens encontrados na tabela:");

        do
        {
            var items = await search.GetNextSetAsync();

            foreach (var item in items)
            {
                Console.WriteLine($"Id={item["Id"]}, Nome={item["Nome"]}, Descrição={item["Descricao"]}");
                id = Guid.Parse(item["Id"]);
            }
        } while (!search.IsDone);
        return id;
    }
}