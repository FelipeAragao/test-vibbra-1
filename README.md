# PROJETO ECOMMERCE VIBBRA

Este sistema tem como público-alvo os profissionais da área de tecnologia e tem como objetivo facilitar a venda, compra e troca de produtos de tecnologia, facilitando e agilizando o negócio entre as partes.


## ESCOPO

- Os usuários devem fazer o cadastro no sistema para poder ter acesso ao sistema;
- Haverá dois métodos de login, a escolha do usuário. Uma delas é fazendo o cadastro de forma tradicional com o preenchimento das informações necessárias. Outra forma será via SSO (Single Sign-On) via Google. Desta segunda forma ainda haverá um cadastro com os dados básicos, porém sem necessidade da informação de senha;
    - > Definido junto ao cliente que ficasse a plataforma SSO ficaria à minha escolha neste momento.***
- Uma vez logado no sistema o usuário pode criar uma Negociação (Deal) de um produto, onde ele informa alguns dados sobre o produto, o preço e imagens.
    - > Definido com o cliente que as imagens ficariam armazenadas em algum servidor de escolha futura, na nossa Api só guardaremos o endereço destas imagens.
- Outros usuário podem fazer Ofertas (Bid) nas negociações de outros usuários. O usuário da negociação pode aceitar ou não aquela oferta;
- Poderão haver trocas de mensagens entre os usuários e os usuários que lançaram uma negociação. Qualquer usuário pode ver as mensagens de uma negociação, porém não pode mandar mensagens caso não esteja logado no sistema;
- Ao fechar um negócio um usuário informa o interesse e cria uma Entrega (Delivery) do produto. Em uma entrega o valor de frete será fechado entre o endereço em que o produto está cadastrado e o endereço do usuário que comprou. O preço é definido pelos Correios;
    - > A Api dos Correios é fechada para quem tem contrato com ela, portanto é necessário um CNPJ para usar a Api tanto em homologação quanto desenvolvimento. Informado isto ao cliente, definimos que seria feito a implementação do recurso com base no manual porém sem aplicação no momento.
- Pagamento: está fora do escopo;
- Convites: será possível enviar convites às pessoas para que se cadastrem no sistema.

## ESTIMATIVA DE HORAS

Foi enviado ao cliente a estimativa de horas de desenvolvimento e a data prevista de término do desenvolvimento. Abaixo o detalhamento e o total das horas:


> ### Autenticação
> - 2 endpoints de autenticação
> - Integração com SSO Google
>
> ### Usuário
> - 3 endpoints (GET, POST, PUT)
>
> ### Negociação
> - 3 endpoints (GET, POST, PUT)
>
> ### Oferta em negociação
> 4 endpoints (GET, GET, POST, PUT)
>
> ### Mensagens
> - 4 endpoints (GET, GET, POST, PUT)
> 
> ### Frete
> - 2 endpoints (GET, POST)
> - Integração com API dos Correios
>
> ### Convite
> - 4 endpoints (GET, GET, POST, PUT)
>
> ## TOTAL
> - Levantamento de Requisitos: 3 horas
> - Desenvolvimento: 16 horas
> - Testes: 13 horas
> 
> #### TOTAL: 32 horas
> #### NÚMERO DE DIAS PARA ENTREGA: 6 DUAS

## APONTAMENTO DAS HORAS

Os apontamento das horas e atividades diárias foram feitas no portal.


# REQUISITOS PARA RODAR A APLICAÇÃO

Além da própria aplicação é necessário um banco de dados MySql e um browser ou aplicação para testar as rotas, podendo ser o próprio Swagger via browser ou o Postman. É importante também que a aplicação rode na porta 8080 pois o SSO do Google está configurado para o localhost:8080.

Abaixo segue como subir a aplicação com o banco de dados.

## Via Docker

Baixando o projeto é uma forma de gerar um release e preparar o ambiente. Segue como fazer:
- Faça um clone do projeto:
> git clone https://git.vibbra.com.br/marcio-1670964779/EcommerceVibbra.git
- Na raiz do projeto entre na pasta Docker e então rode o seguinte comando:
> docker-compose up -d
- Com o docker compose subirão 2 conteineres, um do MySql e outro da aplicação Ecommerce Vibbra. Falta pouco!
- Será necessário rodar o migrations para gerar o banco de dados.
- Volte para a pasta raiz do projeto e entre na pasta src. Nele digite o seguinte comando: 
> dotnet ef database update --project ./src/EcommerceVibbra.csproj

Após isso é só acessar o Swagger em http://localhost:8080/swagger/index.html.

## Apresentação

Gravei também um vídeo mostrando algumas rotas da API e explicando alguns pontos. Ele pode ser visto [neste link](https://youtu.be/Y3Qk4z5i_Hs).
