# CRUDPessoas

## 1) Visão geral do projeto
`CRUDPessoas` é uma aplicação desktop em **WPF (.NET 10)** para gerenciar pessoas em banco SQL Server com operações de CRUD:

- **Create**: cadastrar pessoa
- **Read**: pesquisar por ID e por nome
- **Update**: editar pessoa
- **Delete**: excluir pessoa

O projeto segue organização em **3 camadas**, usando o padrão **DAO** para o acesso ao banco.

---

## 2) Modelo de 3 camadas

### Camada 1: Apresentação (`apresentacao`)
Responsável pela interface com o usuário (janelas, campos, botões e eventos).

### Camada 2: Modelo/Controle (`modelo`)
Responsável por validar dados, aplicar regras e coordenar as chamadas para persistência.

### Camada 3: Acesso a dados (`DAL`)
Responsável por abrir conexão com o banco e executar SQL (INSERT, SELECT, UPDATE, DELETE).

Essa separação evita SQL direto na interface e facilita manutenção.

---

## 3) Padrão DAO (Data Access Object)
No projeto, o DAO é a classe `PessoaDAO`:

- Encapsula todo acesso à tabela `Pessoas`
- Recebe e retorna objetos do domínio (`Pessoa`)
- Executa comandos SQL parametrizados

Benefícios no contexto deste projeto:

- Organização: SQL fica centralizado
- Menor acoplamento: UI não depende de detalhes de banco
- Manutenção mais simples
- Mais segurança contra SQL Injection (uso de parâmetros `@id`, `@nome`, etc.)

---

## 4) Estrutura de pastas e classes
Dentro de `/home/runner/work/PAN_DS43A_2_SEM_26/PAN_DS43A_2_SEM_26/CRUDPessoas/CRUDPessoas`:

### `apresentacao/`
- `frmPrincipal.xaml` e `frmPrincipal.xaml.cs` (`MainWindow`): tela inicial com menu.
- `frmCadastrar.xaml` e `frmCadastrar.xaml.cs` (`frmCadastrar`): tela de cadastro.
- `frmPEE.xaml` e `frmPEE.xaml.cs` (`frmPEE`): tela de pesquisar/editar/excluir.

### `modelo/`
- `Pessoa.cs`: entidade com `id`, `nome`, `rg`, `cpf`.
- `Validacao.cs`: validação de ID e campos de pessoa.
- `Controle.cs`: orquestra validação + operações de DAO.

### `DAL/`
- `Conexao.cs`: conecta/desconecta do SQL Server e mantém mensagem de retorno.
- `PessoaDAO.cs`: operações de persistência da entidade `Pessoa`.

### Arquivos de inicialização
- `App.xaml` / `App.xaml.cs`: configuração da aplicação WPF e `StartupUri` para `apresentacao/frmPrincipal.xaml`.
- `CRUDPessoas.csproj`: projeto .NET WPF com `Microsoft.Data.SqlClient`.

---

## 5) Métodos implementados de todas as classes

## `App` (`App.xaml.cs`)
- Não possui métodos próprios implementados; herda o ciclo de vida de `Application`.

## `MainWindow` (`apresentacao/frmPrincipal.xaml.cs`)
- `MainWindow()`: inicializa a janela principal.
- `mniCadastrar_Click(...)`: abre `frmCadastrar` em modo modal.
- `mniPEE_Click(...)`: abre `frmPEE` em modo modal.

## `frmCadastrar` (`apresentacao/frmCadastrar.xaml.cs`)
- `frmCadastrar()`: inicializa a janela.
- `btnCadastrar_Click(...)`: monta lista de dados da pessoa, chama `Controle.CadastrarPessoa` e exibe a mensagem ao usuário.

## `frmPEE` (`apresentacao/frmPEE.xaml.cs`)
- `frmPEE()`: inicializa a janela.
- `btnPesquisarId_Click(...)`: busca pessoa por ID via `Controle.PesquisarPessoaPorId`; preenche os campos se encontrada.
- `btnPesquisarNome_Click(...)`: método criado, atualmente sem implementação.
- `btnEditar_Click(...)`: envia dados para `Controle.EditarPessoa` e mostra retorno.
- `btnExcluir_Click(...)`: chama `Controle.ExcluirPessoa` e mostra retorno.

## `Pessoa` (`modelo/Pessoa.cs`)
- Classe de entidade (POCO) com propriedades:
  - `id` (`int`)
  - `nome` (`string`)
  - `rg` (`string`)
  - `cpf` (`string`)

## `Validacao` (`modelo/Validacao.cs`)
- Propriedades:
  - `id` (`int`): ID convertido e validado
  - `mensagem` (`string`): acumula erros de validação
- `ValidarId(string numId)`: tenta converter texto para inteiro; em erro, registra mensagem.
- `ValidarDadosPessoa(List<string> listaDadosPessoa)`: valida ID, obrigatoriedade/tamanho do nome e tamanho de RG/CPF.

## `Controle` (`modelo/Controle.cs`)
- Propriedade:
  - `mensagem` (`string`): resposta da validação ou da camada DAO
- `CadastrarPessoa(List<string> listaDadosPessoa)`: força ID `0`, valida dados, cria `Pessoa` e chama `PessoaDAO.CadastrarPessoa`.
- `PesquisarPessoaPorId(string numId)`: valida ID, pesquisa por ID via DAO e retorna `Pessoa`.
- `EditarPessoa(List<string> listaDadosPessoa)`: valida dados, cria `Pessoa` e chama `PessoaDAO.EditarPessoa`.
- `ExcluirPessoa(string numId)`: valida ID e chama `PessoaDAO.ExcluirPessoa`.
- `PesquisarPessoaPorNome(string nome)`: valida nome, chama `PessoaDAO.PesquisarPessoaPorNome` e retorna lista.

## `Conexao` (`DAL/Conexao.cs`)
- Campos/propriedades estáticas:
  - `con`: objeto de conexão SQL compartilhado
  - `mensagem`: texto global de retorno da camada DAL
  - `stringConexao`: string de conexão com SQL Server
- `Conectar()`: abre conexão (se fechada), retorna `SqlConnection` e registra erro em `mensagem` se necessário.
- `Desconectar()`: fecha conexão (se aberta) e registra erro em `mensagem` se necessário.

## `PessoaDAO` (`DAL/PessoaDAO.cs`)
- Campo:
  - `mensagem` (`string`) — declarado na classe
- `CadastrarPessoa(Pessoa pessoa)`: executa `INSERT` na tabela `Pessoas`.
- `PesquisarPessoaPorId(Pessoa pessoa)`: executa `SELECT` por ID e preenche objeto `Pessoa`.
- `EditarPessoa(Pessoa pessoa)`: executa `UPDATE` por ID.
- `contarRegistros(int id)`: conta registros com o ID informado (`COUNT(*)`).
- `ExcluirPessoa(Pessoa pessoa)`: valida existência do ID (via `contarRegistros`) e executa `DELETE`.
- `PesquisarPessoaPorNome(Pessoa pessoa)`: executa `SELECT` com `LIKE` no nome e retorna `List<Pessoa>`.

---

## 6) Fluxo de dados (fim a fim)
Fluxo típico para cadastro, edição ou exclusão:

1. Usuário preenche campos e clica em botão na camada **apresentação**.
2. Evento da janela chama um método da classe **Controle**.
3. `Controle` usa **Validacao** para validar entrada.
4. Com dados válidos, `Controle` monta objeto **Pessoa**.
5. `Controle` chama método correspondente em **PessoaDAO**.
6. `PessoaDAO` obtém conexão por **Conexao.Conectar()**.
7. `PessoaDAO` executa SQL parametrizado no SQL Server.
8. `PessoaDAO` finaliza com **Conexao.Desconectar()** e define mensagem de retorno.
9. `Controle` propaga essa mensagem para a tela.
10. A tela mostra resultado ao usuário com `MessageBox`.

---

## 7) Banco de dados e pré-requisitos
Tabela esperada no SQL Server:

- `id` `int` identity, chave primária
- `nome` `varchar(50)` obrigatório
- `rg` `varchar(11)`
- `cpf` `varchar(13)`

O script base está comentado em `DAL/Conexao.cs`.

Antes de executar:
1. Ajuste `stringConexao` em `DAL/Conexao.cs`.
2. Abra `CRUDPessoas.slnx` no Visual Studio.
3. Restaure os pacotes e execute o projeto.
