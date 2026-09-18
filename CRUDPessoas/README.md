# CRUDPessoas

## Visão geral
Este projeto é uma aplicação **desktop WPF (.NET 10)** para cadastro e manutenção de pessoas em banco SQL Server, com operações de **CRUD**:
- **Create**: cadastrar pessoa
- **Read**: pesquisar por ID e por nome
- **Update**: editar pessoa
- **Delete**: excluir pessoa

A solução está organizada para separar interface, regras e acesso a dados.

---

## Estrutura da pasta
Dentro de `CRUDPessoas/CRUDPessoas`:

- `apresentacao/`
  - `frmPrincipal.xaml`: janela inicial com menu de navegação.
  - `frmCadastrar.xaml`: tela para cadastrar pessoa.
  - `frmPEE.xaml`: tela para pesquisar, editar e excluir.
- `modelo/`
  - `Pessoa.cs`: entidade com `id`, `nome`, `rg`, `cpf`.
  - `Validacao.cs`: validações de ID e campos.
  - `Controle.cs`: camada intermediária entre tela e DAO.
- `DAL/`
  - `Conexao.cs`: conexão com SQL Server e mensagens globais de operação.
  - `PessoaDAO.cs`: operações SQL (insert, select, update, delete).
- `App.xaml`: inicializa a aplicação (`StartupUri` em `frmPrincipal.xaml`).

---

## Fluxo da aplicação
1. Usuário interage com a tela (camada `apresentacao`).
2. A tela envia dados para `Controle`.
3. `Controle` chama `Validacao`.
4. Se os dados forem válidos, `Controle` cria um objeto `Pessoa` e chama `PessoaDAO`.
5. `PessoaDAO` executa SQL usando `Conexao`.
6. Mensagem de sucesso/erro retorna para a tela.

Essa organização reduz acoplamento da interface com SQL direto.

---

## Funcionalidades implementadas
- **Cadastro** (`frmCadastrar`):
  - Coleta nome, RG e CPF.
  - Usa `Controle.CadastrarPessoa`.
- **Pesquisa por ID** (`frmPEE`):
  - Usa `Controle.PesquisarPessoaPorId`.
  - Preenche os campos ao encontrar registro.
- **Edição** (`frmPEE`):
  - Usa `Controle.EditarPessoa`.
- **Exclusão** (`frmPEE`):
  - Usa `Controle.ExcluirPessoa`.
- **Pesquisa por nome**:
  - Existe na camada de regra (`Controle.PesquisarPessoaPorNome`) e DAO (`PessoaDAO.PesquisarPessoaPorNome`).
  - Observação: o evento `btnPesquisarNome_Click` está criado na tela, mas sem implementação no code-behind.

---

## Regras de validação atuais
Em `Validacao.cs`:
- ID deve ser numérico.
- Nome:
  - obrigatório
  - mínimo de 3 caracteres
  - máximo de 50 caracteres
- RG: máximo de 11 caracteres.
- CPF: máximo de 13 caracteres.

As mensagens são agregadas e exibidas ao usuário.

---

## Banco de dados
O projeto usa tabela `Pessoas` no SQL Server com os campos:
- `id` (int, identity, chave primária)
- `nome` (varchar(50), obrigatório)
- `rg` (varchar(11))
- `cpf` (varchar(13))

O script base de criação está comentado em `DAL/Conexao.cs`.

> Importante: ajuste a `stringConexao` em `Conexao.cs` para seu ambiente antes de executar.

---

## Padrão DAO (Data Access Object) no projeto
O padrão **DAO** separa as regras de negócio do acesso físico ao banco.

### Como ele aparece aqui
- `PessoaDAO` é o DAO da entidade `Pessoa`.
- Cada método do DAO representa uma operação de persistência:
  - `CadastrarPessoa` → `INSERT`
  - `PesquisarPessoaPorId` / `PesquisarPessoaPorNome` → `SELECT`
  - `EditarPessoa` → `UPDATE`
  - `ExcluirPessoa` → `DELETE`
- `Conexao` centraliza abrir/fechar conexão.

### Papel de cada camada com DAO
- **Apresentação**: só lida com campos e eventos.
- **Controle/Validação**: aplica regras de entrada e monta objetos.
- **DAO**: executa SQL parametrizado e devolve dados/resultados.

### Benefícios práticos neste projeto
- Facilita manutenção de SQL em um único ponto (`PessoaDAO`).
- Melhora organização e leitura do código.
- Permite evoluir regras de negócio sem misturar com comandos SQL.
- Reduz risco de SQL Injection ao usar parâmetros (`@nome`, `@id`, etc.).

---

## Como executar (resumo)
1. Criar banco e tabela `Pessoas` no SQL Server.
2. Ajustar a `stringConexao` em `DAL/Conexao.cs`.
3. Abrir `CRUDPessoas.slnx` no Visual Studio.
4. Restaurar pacotes e executar o projeto WPF.

