# CRUDPessoasEntity

Este módulo mostra a evolução do projeto anterior para começar a usar **Entity Framework Core (EF Core)** em uma aplicação WPF com SQL Server.

> O README de `/home/runner/work/PAN_DS43A_2_SEM_26/PAN_DS43A_2_SEM_26/CRUDPessoas/README.md` já explica a estrutura geral do CRUD, as camadas e o funcionamento básico da aplicação.  
> Aqui o foco é **somente a introdução ao Entity Framework** e o que mudou neste projeto.

---

## 1) O que é Entity Framework?

Entity Framework Core é um **ORM** (*Object-Relational Mapper*).

Na prática, ele permite trabalhar com o banco de dados usando **classes e objetos C#**, em vez de escrever manualmente todo o SQL para cada operação.

Em vez de pensar primeiro em:

- `INSERT`
- `SELECT`
- `UPDATE`
- `DELETE`

você pode pensar em:

- criar um objeto
- alterar propriedades
- adicionar esse objeto ao contexto
- salvar as mudanças

O EF Core faz a ponte entre:

- **mundo orientado a objetos** → classes como `Pessoa`
- **mundo relacional** → tabelas como `Pessoas`

---

## 2) Por que usar Entity Framework?

Ao comparar com acesso manual via `SqlConnection`, `SqlCommand` e `SqlDataReader`, o Entity Framework traz algumas facilidades:

- reduz código repetitivo de acesso a dados
- aproxima o banco do modelo de objetos da aplicação
- facilita manutenção
- ajuda na criação e evolução do banco com **migrations**
- melhora a legibilidade em operações simples de persistência

Isso não significa que SQL deixa de existir. O banco continua relacional, mas o EF Core abstrai boa parte do trabalho operacional.

---

## 3) Pacotes usados no projeto

No arquivo `/home/runner/work/PAN_DS43A_2_SEM_26/PAN_DS43A_2_SEM_26/CRUDPessoasEntity/CRUDPessoas/CRUDPessoas.csproj`, o projeto referencia:

- `Microsoft.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore.SqlServer`
- `Microsoft.EntityFrameworkCore.Tools`

Cada pacote tem um papel:

### `Microsoft.EntityFrameworkCore`
É a base do EF Core. Contém os tipos principais, como `DbContext`, `DbSet<>` e o mecanismo de rastreamento de entidades.

### `Microsoft.EntityFrameworkCore.SqlServer`
É o provedor que ensina o EF Core a conversar com o **SQL Server**.

### `Microsoft.EntityFrameworkCore.Tools`
Fornece suporte às ferramentas de desenvolvimento, principalmente para:

- criar migrations
- atualizar o banco
- sincronizar modelo e estrutura física

---

## 4) A entidade do sistema

No arquivo `/home/runner/work/PAN_DS43A_2_SEM_26/PAN_DS43A_2_SEM_26/CRUDPessoasEntity/CRUDPessoas/modelo/Pessoa.cs`, a classe `Pessoa` representa os dados do domínio:

- `id`
- `nome`
- `rg`
- `cpf`
- `email`

Essa classe é importante porque, no EF Core, uma entidade C# normalmente corresponde a uma tabela do banco.

Neste projeto:

- a classe `Pessoa` representa a entidade do domínio
- a tabela gerada no banco é `Pessoas`
- cada objeto `Pessoa` pode virar um registro da tabela

---

## 5) O que é `DbContext`?

O coração do Entity Framework é o **contexto**, que neste projeto está em:

`/home/runner/work/PAN_DS43A_2_SEM_26/PAN_DS43A_2_SEM_26/CRUDPessoasEntity/CRUDPessoas/DAL/AppDbContext.cs`

`AppDbContext` herda de `DbContext` e representa a sessão de trabalho com o banco.

Ele é responsável por:

- configurar a conexão
- mapear entidades
- rastrear objetos carregados ou adicionados
- enviar alterações para o banco com `SaveChanges()`

### Papel do `DbSet<Pessoa>`

Dentro do contexto existe:

- `DbSet<Pessoa> Pessoas`

Esse `DbSet` funciona como a coleção de entidades `Pessoa` controlada pelo EF Core.

É por meio dele que o código passa a fazer operações como:

- adicionar uma pessoa
- consultar pessoas
- alterar pessoas
- remover pessoas

---

## 6) Como a conexão é configurada

Em `AppDbContext`, o método `OnConfiguring` usa:

- `optionsBuilder.UseSqlServer(...)`

Esse comando informa que:

1. o banco utilizado é SQL Server
2. o EF Core deve usar a string de conexão fornecida
3. todas as operações do contexto devem ser direcionadas a esse banco

Em outras palavras, é nesse ponto que o EF Core aprende **onde** salvar e buscar os dados.

---

## 7) Como o cadastro usando EF funciona

O melhor exemplo introdutório deste projeto está em:

`/home/runner/work/PAN_DS43A_2_SEM_26/PAN_DS43A_2_SEM_26/CRUDPessoasEntity/CRUDPessoas/DAL/PessoaDAO.cs`

No método `CadastrarPessoa`, o fluxo com EF Core é:

1. criar o contexto: `AppDbContext contexto = new AppDbContext();`
2. adicionar o objeto: `contexto.Pessoas.Add(pessoa);`
3. persistir no banco: `contexto.SaveChanges();`

Didaticamente, isso significa:

- **`Add`** diz ao EF Core que existe uma nova entidade a ser inserida
- **`SaveChanges`** transforma essa intenção em SQL e executa o `INSERT` no banco

Ou seja, o desenvolvedor trabalha com o objeto `Pessoa`, e o EF Core se encarrega de gerar a operação relacional correspondente.

---

## 8) O que são migrations?

As **migrations** são arquivos que registram a evolução da estrutura do banco com base no modelo da aplicação.

Neste projeto, elas estão em:

- `/home/runner/work/PAN_DS43A_2_SEM_26/PAN_DS43A_2_SEM_26/CRUDPessoasEntity/CRUDPessoas/Migrations/20260915194045_inicial.cs`
- `/home/runner/work/PAN_DS43A_2_SEM_26/PAN_DS43A_2_SEM_26/CRUDPessoasEntity/CRUDPessoas/Migrations/AppDbContextModelSnapshot.cs`

Pela migration inicial, é possível ver que o EF Core criou a tabela `Pessoas` com colunas compatíveis com a classe `Pessoa`, incluindo o campo `email`.

### Ideia principal de migration

Em vez de criar tudo manualmente no banco, você:

1. define ou altera as classes do modelo
2. gera uma migration
3. aplica a migration no banco

Assim, o banco acompanha a evolução do código.

---

## 9) Comandos usados com EF no projeto

No próprio `AppDbContext.cs` existe a orientação para usar o Console do Gerenciador de Pacotes do Visual Studio:

- `Add-Migration Inicial`
- `Update-Database`

### O que cada comando faz

#### `Add-Migration Inicial`
Cria os arquivos de migration com base no estado atual das entidades e do contexto.

#### `Update-Database`
Aplica as migrations pendentes no banco configurado, criando ou alterando tabelas conforme necessário.

---

## 10) O que mudou em relação ao projeto anterior

Este projeto é uma **introdução** ao Entity Framework, não uma migração completa de todo o CRUD.

Hoje, o código mostra um cenário didático e misto:

- **Cadastro (`CadastrarPessoa`)** já usa EF Core
- **Pesquisa, edição e exclusão** ainda usam `SqlConnection`, `SqlCommand` e `SqlDataReader`

Isso é importante porque permite comparar as duas abordagens no mesmo projeto:

- acesso manual com ADO.NET
- acesso orientado a entidades com EF Core

Esse tipo de transição é comum no aprendizado, pois ajuda a entender exatamente o que o ORM está simplificando.

---

## 11) Resumo conceitual

Para entender o Entity Framework neste projeto, basta guardar esta sequência:

1. a classe `Pessoa` representa a entidade
2. o `AppDbContext` representa a conexão lógica e o controle das entidades
3. o `DbSet<Pessoa>` representa a coleção mapeada para a tabela
4. `Add()` registra uma nova entidade para inserção
5. `SaveChanges()` envia as alterações ao banco
6. as migrations criam e evoluem a estrutura física do banco

---

## 12) Próximo passo natural de aprendizado

Depois desta introdução, a evolução mais natural do projeto seria:

- converter também pesquisa, edição e exclusão para EF Core
- substituir gradualmente o SQL manual restante
- aprender consultas com LINQ
- entender melhor rastreamento de entidades e estados (`Added`, `Modified`, `Deleted`)

Assim, este módulo serve como ponte entre:

- o CRUD tradicional com ADO.NET
- e um CRUD orientado a entidades com EF Core
