# Arkive API — .NET

API RESTful desenvolvida em **ASP.NET Core** para o gerenciamento do catálogo clínico veterinário da plataforma **ArkIve**, solução proposta para o **Challenge 2026 FIAP × Clyvo Vet**.

A API é responsável pelo cadastro e manutenção de **espécies, raças, categorias de doenças, doenças, predisposições genéticas e feedbacks NPS**, servindo como base de dados compartilhada entre as APIs do ecossistema ArkIve.

---

## Integrantes

| Nome | RM |
|------|----|
| Victor Sabelli | RM566224 |
| Gustavo Crevelari | RM561408 |
| Lucca Gomes | RM561996 |
| Rafaela Ferreira | RM561671 |

---

## Estrutura do Projeto

```
Arkive_API/
├── Controllers/         # Endpoints REST
├── Data/                # ApplicationContext (EF Core)
├── Migrations/          # Migrations do banco Oracle
├── Models/              # Entities mapeadas
│   └── External/        # Entities somente leitura (API Java)
└── prints/              # Evidências dos testes por endpoint
```

---

## Tecnologias

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core + Oracle Provider
- Swashbuckle (Swagger / OpenAPI)
- Oracle Database (compartilhado com API Java)

---

## Instalação e Execução

### Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Acesso ao banco Oracle configurado pela equipe

### 1. Clone o repositório

```bash
git clone https://github.com/<seu-usuario>/dotnet-advanced-bizdev.git
cd dotnet-advanced-bizdev
```

### 2. Configure a string de conexão

Devido à necessidade de acesso às tabelas criadas previamente pela equipe no banco Oracle compartilhado, as credenciais já estão configuradas no `Arkive_API/appsettings.json` do repositório. Basta clonar e executar — nenhuma alteração é necessária.

### 3. Migrations

As migrations foram geradas com o comando abaixo e estão disponíveis na pasta `Migrations/`:

```bash
Add-Migration InicialMigration
```

> O banco Oracle foi modelado e criado previamente pela equipe. As migrations existem para fins de versionamento e rastreabilidade do schema — não são necessárias para criação das tabelas.

### 4. Execute a API

```bash
dotnet run
```

A API estará disponível em `https://localhost:7000` (ou a porta configurada em `launchSettings.json`).

### 5. Acesse o Swagger

```
https://localhost:7000/swagger
```

---

## Rotas

### Espécies — `/api/especies`

| Método | Rota | Descrição | Retorno |
|--------|------|-----------|---------|
| GET | `/api/especies` | Lista todas as espécies | 200 / 204 |
| GET | `/api/especies/ativos` | Lista espécies ativas | 200 / 204 |
| GET | `/api/especies/inativos` | Lista espécies inativas | 200 / 204 |
| GET | `/api/especies/{id}` | Busca espécie por ID | 200 / 404 |
| POST | `/api/especies` | Cria nova espécie | 201 / 400 |
| PUT | `/api/especies/{id}` | Atualiza espécie | 200 / 404 / 400 |
| PUT | `/api/especies/reativar/{id}` | Reativa espécie inativa | 200 / 404 |
| DELETE | `/api/especies/{id}` | Inativa espécie (soft delete) | 200 / 404 |

**POST / PUT — Body:**
```json
{ "especie": "Cachorro" }
```

---

### Categorias de Doença — `/api/categorias-doenca`

| Método | Rota | Descrição | Retorno |
|--------|------|-----------|---------|
| GET | `/api/categorias-doenca` | Lista todas as categorias | 200 / 204 |
| GET | `/api/categorias-doenca/ativos` | Lista categorias ativas | 200 / 204 |
| GET | `/api/categorias-doenca/inativos` | Lista categorias inativas | 200 / 204 |
| GET | `/api/categorias-doenca/{id}` | Busca categoria por ID | 200 / 404 |
| POST | `/api/categorias-doenca` | Cria nova categoria | 201 / 400 |
| PUT | `/api/categorias-doenca/{id}` | Atualiza categoria | 200 / 404 / 400 |
| PUT | `/api/categorias-doenca/reativar/{id}` | Reativa categoria inativa | 200 / 404 |
| DELETE | `/api/categorias-doenca/{id}` | Inativa categoria (soft delete) | 200 / 404 |

**POST / PUT — Body:**
```json
{ "nome": "Infecciosa" }
```

---

### Raças — `/api/racas`

| Método | Rota | Descrição | Retorno |
|--------|------|-----------|---------|
| GET | `/api/racas` | Lista todas as raças | 200 / 204 |
| GET | `/api/racas/ativos` | Lista raças ativas | 200 / 204 |
| GET | `/api/racas/inativos` | Lista raças inativas | 200 / 204 |
| GET | `/api/racas/{id}` | Busca raça por ID | 200 / 404 |
| GET | `/api/racas/especie/{idEspecie}` | Lista raças por espécie | 200 / 204 |
| POST | `/api/racas` | Cria nova raça | 201 / 404 / 400 |
| PUT | `/api/racas/{id}` | Atualiza raça | 200 / 404 / 400 |
| PUT | `/api/racas/reativar/{id}` | Reativa raça inativa | 200 / 404 |
| DELETE | `/api/racas/{id}` | Inativa raça (soft delete) | 200 / 404 |

**POST / PUT — Body:**
```json
{ "raca": "Labrador", "idEspecie": 1, "porte": "GRANDE" }
```
> `porte` aceita: `PEQUENO`, `MEDIO`, `GRANDE`

---

### Doenças — `/api/doencas`

| Método | Rota | Descrição | Retorno |
|--------|------|-----------|---------|
| GET | `/api/doencas` | Lista todas as doenças | 200 / 204 |
| GET | `/api/doencas/ativos` | Lista doenças ativas | 200 / 204 |
| GET | `/api/doencas/inativos` | Lista doenças inativas | 200 / 204 |
| GET | `/api/doencas/{id}` | Busca doença por ID | 200 / 404 |
| GET | `/api/doencas/nome/{nome}` | Busca doenças por nome (parcial) | 200 / 204 |
| GET | `/api/doencas/categoria/{idCategoria}` | Lista doenças por categoria | 200 / 204 |
| POST | `/api/doencas` | Cria nova doença | 201 / 404 / 400 |
| PUT | `/api/doencas/{id}` | Atualiza doença | 200 / 404 / 400 |
| PUT | `/api/doencas/reativar/{id}` | Reativa doença inativa | 200 / 404 |
| DELETE | `/api/doencas/{id}` | Inativa doença (soft delete) | 200 / 404 |

**POST / PUT — Body:**
```json
{
  "nome": "Leishmaniose",
  "idCategoria": 1,
  "descricao": "Doença parasitária transmitida por flebotomíneos",
  "cid": "B55",
  "sintomas": "Perda de peso, queda de pelo, lesões cutâneas"
}
```
> `idCategoria` é opcional

---

### Predisposições — `/api/predisposicoes`

| Método | Rota | Descrição | Retorno |
|--------|------|-----------|---------|
| GET | `/api/predisposicoes` | Lista todas as predisposições | 200 / 204 |
| GET | `/api/predisposicoes/{id}` | Busca predisposição por ID | 200 / 404 |
| GET | `/api/predisposicoes/especie/{idEspecie}` | Lista por espécie | 200 / 204 |
| GET | `/api/predisposicoes/raca/{idRaca}` | Lista por raça | 200 / 204 |
| GET | `/api/predisposicoes/doenca/{idDoenca}` | Lista por doença | 200 / 204 |
| POST | `/api/predisposicoes` | Cria vínculo de predisposição | 201 / 404 / 400 |
| DELETE | `/api/predisposicoes/{id}` | Remove vínculo (delete físico) | 200 / 404 |

> Sem `PUT` — para alterar um vínculo, delete o antigo e crie um novo.

**POST — Body (com raça):**
```json
{ "idEspecie": 1, "idRaca": 1, "idDoenca": 3 }
```

**POST — Body (sem raça — predisposição por espécie):**
```json
{ "idEspecie": 1, "idDoenca": 3 }
```

---

### Feedbacks NPS — `/api/feedbacks-nps`

| Método | Rota | Descrição | Retorno |
|--------|------|-----------|---------|
| GET | `/api/feedbacks-nps` | Lista todos os feedbacks | 200 / 204 |
| GET | `/api/feedbacks-nps/{id}` | Busca feedback por ID | 200 / 404 |
| GET | `/api/feedbacks-nps/nota/{nota}` | Lista por nota (0–10) | 200 / 204 / 400 |
| GET | `/api/feedbacks-nps/responsavel/{id}` | Lista por responsável | 200 / 204 |
| GET | `/api/feedbacks-nps/animal/{id}` | Lista por animal | 200 / 204 |
| GET | `/api/feedbacks-nps/clinica/{id}` | Lista por clínica | 200 / 204 |
| GET | `/api/feedbacks-nps/veterinario/{id}` | Lista por veterinário | 200 / 204 |
| GET | `/api/feedbacks-nps/data/{data}` | Lista por data (`yyyy-MM-dd`) | 200 / 204 / 400 |
| POST | `/api/feedbacks-nps` | Registra novo feedback | 201 / 404 / 400 |
| DELETE | `/api/feedbacks-nps/{id}` | Remove feedback (delete físico) | 200 / 404 |

> Sem `PUT` — feedback NPS é registro imutável.  
> Ao menos um contexto é obrigatório: `idResponsavel`, `idAnimal`, `idClinica`, `idConsulta` ou `idVeterinario`.

**POST — Body:**
```json
{
  "idResponsavel": 1,
  "nota": 9,
  "comentario": "Ótimo atendimento"
}
```

---

## Evidências de Testes

Prints de todos os endpoints testados estão na pasta `prints/`, organizados por recurso:

```
prints/
├── CategoriaDoenca/   (8 endpoints)
├── Doenca/            (10 endpoints)
├── Especie/           (8 endpoints)
├── FeedbackNPS/       (10 endpoints)
├── Predisposicao/     (7 endpoints)
└── Raca/              (9 endpoints)
```

| Controller | Endpoints | Evidência |
| :--- | :---: | :--- |
| **CategoriaDoenca** | 8 endpoints | [Visualizar Prints](prints/CategoriaDoenca/) |
| **Doenca** | 10 endpoints | [Visualizar Prints](prints/Doenca/) |
| **Especie** | 8 endpoints | [Visualizar Prints](prints/Especie/) |
| **FeedbackNPS** | 10 endpoints | [Visualizar Prints](prints/FeedbackNPS/) |
| **Predisposicao** | 7 endpoints | [Visualizar Prints](prints/Predisposicao/) |
| **Raca** | 9 endpoints | [Visualizar Prints](prints/Raca/) |

> **Total:** 52 endpoints testados e documentados.

---

## Observações

- O banco Oracle é compartilhado com a **API Java** da equipe. As tabelas de `Responsavel`, `Animal`, `Clinica`, `Consulta` e `Veterinario` são gerenciadas pela API Java — a API .NET realiza apenas leitura dessas tabelas para validação de FKs.
- Exclusões nas entidades principais (`Especie`, `Raca`, `CategoriaDoenca`, `Doenca`) são **lógicas** via `ST_ATIVO`, preservando a integridade referencial do banco.
- Exclusões em `Predisposicao` e `FeedbackNPS` são **físicas**, pois são registros de vínculo e imutáveis por natureza.