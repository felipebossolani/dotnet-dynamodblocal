# Projeto DynamoDB com C#

Este projeto contém uma aplicação simples em C# que interage com um banco de dados local DynamoDB.

## Pré-requisitos

- Docker
- .NET 5.0 ou superior
- Amazon DynamoDB Local Docker image

## Como rodar o projeto

1. Inicie o DynamoDB localmente com Docker usando o seguinte comando:
```
docker run -p 8000:8000 amazon/dynamodb-local
```

2. Clone este repositório.
3. Abra a solução no Visual Studio ou em seu editor de código preferido.
4. Rode a aplicação.

## Estrutura do código

A aplicação C# contém os seguintes métodos:

- `Main`: O ponto de entrada do programa. Ele inicializa o cliente DynamoDB, tenta carregar a tabela. Se a tabela não existir, ele chama `CreateTableAsync` para criar a tabela. Ele então chama `PutItemExampleAsync` para inserir um item, `ScanTableExampleAsync` para listar todos os itens, e `GetItemExampleAsync` para obter um item específico.

- `CreateTableAsync`: Cria uma nova tabela com o nome especificado e aguarda até que a tabela esteja ativa.

- `WaitUntilTableExistsAsync`: Verifica o status da tabela a cada segundo até que o status seja `ACTIVE`.

- `PutItemExampleAsync`: Insere um novo item na tabela.

- `GetItemExampleAsync`: Obtém um item específico da tabela usando sua chave primária.

- `ScanTableExampleAsync`: Lista todos os itens na tabela.

Cada item na tabela tem um atributo `Id` (uma chave primária), um `Nome` e uma `Descricao`. O `Id` é gerado automaticamente quando um item é inserido.

## Detalhes da implementação

O programa usa a biblioteca Amazon.DynamoDBv2 do AWS SDK para .NET para interagir com o DynamoDB. Ele usa a classe `AmazonDynamoDBClient` para criar e gerenciar a conexão com o DynamoDB, a classe `Table` para interagir com tabelas individuais, e a classe `Document` para representar itens.

## Contribuição

Sinta-se à vontade para contribuir com este projeto. Se encontrar algum problema, por favor, abra uma issue ou se preferir, abra um Pull Request.

## Licença

Este projeto está livre para uso.
