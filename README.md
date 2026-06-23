# ChatApp

📌 **Trabalho Prático – Sistema de Chat Cliente-Servidor em C#**

Desenvolva uma aplicação desktop em **C# utilizando Windows Forms Application (.NET)** que implemente um sistema de comunicação em tempo real entre usuários utilizando arquitetura **cliente-servidor** e conceitos de **Sockets TCP**.

O objetivo é criar uma aplicação semelhante a um mensageiro instantâneo, permitindo que usuários conectados iniciem conversas privadas entre si.

🎮 **Requisitos obrigatórios:**

### Servidor

O sistema deverá possuir um servidor capaz de:

* Aceitar múltiplas conexões simultâneas;
* Manter uma lista de clientes conectados;
* Informar aos clientes quando novos usuários entrarem ou saírem do sistema;
* Encaminhar solicitações de conversa entre usuários.

### Saguão (Lobby)

Ao conectar-se ao servidor, o cliente deverá visualizar uma tela de saguão contendo:

* Seu nome de usuário;
* A lista de todos os usuários atualmente conectados;
* Atualização automática da lista quando usuários entrarem ou saírem.

### Solicitação de Conversa

O usuário deverá ser capaz de:

* Selecionar outro usuário conectado;
* Solicitar a abertura de uma conversa privada por meio de um clique ou botão apropriado.

Quando uma solicitação for enviada:

* O usuário destinatário deverá receber uma notificação;
* O destinatário poderá aceitar ou recusar a solicitação.

### Sala de Chat

Caso a solicitação seja aceita:

* Uma janela de conversa privada deverá ser aberta para ambos os usuários;
* A interface deverá seguir o modelo de um aplicativo de mensagens instantâneas (estilo WhatsApp ou Telegram);
* As mensagens enviadas por um usuário deverão aparecer imediatamente para o outro;
* O histórico da conversa deverá permanecer visível durante a sessão.

### Mensagens

A sala de chat deverá permitir:

* Envio de mensagens de texto;
* Visualização das mensagens enviadas e recebidas;
* Identificação clara do remetente de cada mensagem.

💻 **O projeto deve utilizar conceitos de Programação Orientada a Objetos**, como:

* Classes e objetos;
* Encapsulamento;
* Construtores;
* Associação entre classes;
* Métodos e atributos.

Sugestão de classes:

* Servidor;
* Cliente;
* Usuario;
* SalaChat;
* Mensagem;
* GerenciadorDeConexoes.

⚠️ **Observações importantes:**

* O sistema deve permitir múltiplos clientes conectados simultaneamente.
* Não serão aceitas implementações que utilizem apenas troca de mensagens em console.
* A comunicação deve ocorrer através de sockets.
* A abertura da sala de chat deve ocorrer somente após a aceitação da solicitação pelo usuário convidado.

📄 **Entrega:**

A entrega deverá ser realizada até a próxima **próximo dia 28 às 23h59**.

Entregar:

1. O projeto completo (cliente e servidor);
2. Um relatório em PDF contendo:

   * Explicação das classes desenvolvidas;
   * Descrição da arquitetura cliente-servidor utilizada;
   * Explicação do protocolo de mensagens implementado;
   * Capturas de tela demonstrando o funcionamento do sistema.

✅ **Critérios de avaliação:**

* Funcionamento correto da comunicação entre clientes;
* Implementação adequada do servidor;
* Atualização correta da lista de usuários conectados;
* Criação e gerenciamento das salas de chat;
* Uso adequado de Programação Orientada a Objetos;
* Organização e clareza do código;
* Qualidade do relatório.