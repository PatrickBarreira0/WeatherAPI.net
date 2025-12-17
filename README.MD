# Integração da API de Clima com C# (.NET Core) e React

Este projeto implementa um **API Gateway** em C# (.NET Core) para consumir dados da WeatherAPI.com e uma interface de usuário em **React** para exibir esses dados. A segurança da aplicação é garantida por um Middleware personalizado de autenticação via **API Key**.

---

## Escopo do Projeto

O objetivo deste teste foi criar uma arquitetura que abstrai a complexidade da API externa e protege o acesso através de uma chave de API interna.

*   **Back-End:** API Gateway em .NET Core que recebe solicitações, valida a segurança e busca dados externos.
*   **Front-End:** Aplicação React que consome o Gateway enviando os cabeçalhos de segurança corretos.
*   **Segurança:** Implementação de autenticação via Header (`X-API-Key`).
*   **Documentação:** Swagger (OpenAPI) configurado para aceitar testes com autenticação.

---

##  Tecnologias Envolvidas

*   **C# / ASP.NET Core Web API**
*   **React.js** (Front-end)
*   **Axios** (Requisições HTTP)
*   **Swagger / Swashbuckle** (Documentação Interativa)
*   **WeatherAPI.com** (Fonte de dados externa)

---

## Estrutura do Projeto (.NET Core)

A organização do projeto segue o padrão de responsabilidade única:

1.  **/Controllers**: Contém o `WeatherController`, responsável por receber as requisições, chamar o serviço externo e retornar a resposta.
2.  **/Middleware**: Contém o `ApiKeyMiddleware`, que intercepta todas as requisições para validar o cabeçalho `X-API-Key` antes de chegarem ao controller.
3.  **/DTOs (Data Transfer Objects)**: Contém o `SimplifiedWeatherDto`, usado para filtrar os dados da API externa e entregar apenas o necessário ao Front-end (Cidade, Temperatura, Ícone, etc).

---

## Detalhes da Implementação

### 1. Middleware de Autenticação
O `ApiKeyMiddleware` funciona como um segurança. Ele verifica se o cabeçalho `X-API-Key` está presente e se corresponde à chave configurada no `appsettings.json`. Se não for válido, retorna erro `401 Unauthorized`.

### 2. Fluxo no Controller
O `WeatherController`:
*   Recebe a cidade via URL.
*   Lê a chave da WeatherAPI externa das configurações.
*   Faz a chamada HTTP para `api.weatherapi.com`.
*   Deserializa o JSON complexo e mapeia para o nosso DTO simplificado.

### 3. Configurações (Program.cs)
*   **CORS**: Configurado (`AllowReactApp`) para permitir que o front-end (localhost:3000) acesse a API.
*   **Swagger**: Configurado para permitir a inserção da API Key (botão "Authorize") durante os testes manuais.

---

## Como Executar o Projeto

### Pré-requisitos
*   [.NET SDK](https://dotnet.microsoft.com/download) instalado.
*   [Node.js](https://nodejs.org/) instalado.
*   Uma chave gratuita obtida em [WeatherAPI.com](https://www.weatherapi.com/).

### Passo 1: Configuração do Back-End

1.  Navegue até a pasta do projeto .NET:
    ```bash
    cd WeatherApiGateway
    ```

2.  **Importante:** Crie ou edite o arquivo `appsettings.json` na raiz do projeto para incluir suas chaves. O arquivo deve ficar assim:

    ```json
    {
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft.AspNetCore": "Warning"
        }
      },
      "AllowedHosts": "*",
      "ApiKey": "senha"
      "WeatherApiKey": "COLE_SUA_CHAVE_DA_WEATHERAPI_AQUI"
    }
    ```
    *   `ApiKey`: A chave interna que o Front-end usará (ex: "admin").
    *   `WeatherApiKey`: A chave que você recebeu ao se cadastrar no site WeatherAPI.

3.  Execute a aplicação:
    ```bash
    dotnet run
    ```
    *O servidor iniciará (geralmente em `http://localhost:5283`).*

### Passo 2: Configuração do Front-End

1.  Navegue até a pasta do projeto React:
    ```bash
    cd weather-frontend
    ```

2.  Instale as dependências:
    ```bash
    npm install
    ```

3.  Configure as variáveis de ambiente:
    - Certifique-se de que existe um arquivo `.env` na pasta `weather-frontend`
    - O arquivo deve conter:
      ```
      REACT_APP_API_KEY="senha"
      REACT_APP_API_URL=http://localhost:5283
      ```
    - Ajuste o `REACT_APP_API_KEY` para corresponder ao valor configurado no `appsettings.json` do backend
    - Ajuste o `REACT_APP_API_URL` se a API estiver rodando em uma porta diferente

4.  Execute a aplicação:
    ```bash
    npm start
    ```
    *O navegador abrirá em `http://localhost:3000`.*

---

## Como Testar

### Via Aplicação (React)
1.  Digite o nome de uma cidade (ex: "Paris", "Sao Paulo") no campo de texto.
2.  Clique em "Buscar".
3.  Os dados (Temperatura, Ícone, Região) serão exibidos.

### Via Swagger (Documentação Interativa)
1.  Com o Back-end rodando, acesse `http://localhost:5283/swagger` (ajuste a porta se necessário).
2.  Clique no botão verde **Authorize**.
3.  No campo "Value", digite sua chave interna (ex: `senha`) e clique em "Authorize".
4.  Abra o endpoint `GET /Weather/{city}`, clique em "Try it out", digite uma cidade e execute.